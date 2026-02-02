using Microsoft.Extensions.Logging;
using RiskAnalisys.Application.DTO.Requests;
using RiskAnalisys.Application.DTO.Responses;
using RiskAnalisys.Domain.Entities;
using RiskAnalisys.Domain.Structs;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace RiskAnalisys.Application.Services;

public class RiskAnalisysService : IRiskAnalisysService
{
    private readonly ILogger<RiskAnalisysService> _logger;
    public RiskAnalisysService(ILogger<RiskAnalisysService> logger)
    {
        _logger = logger;
    }
    public ClassifyRiskDTO ClassifyRisk(ClassifyRiskRequestDTO[] request)
    {
        _logger.LogInformation("Iniciando processamento de {Length} trades", request.Length);

        var response = request
                        .AsParallel()
                        .AsOrdered()
                        .Select(p => Classification(p)).ToArray();

        _logger.LogInformation("Classificação concluída.");

        return new ClassifyRiskDTO(response);
    }

    public DistribuitionCalculatedDTO DistributionRisk(DistributionRiskRequestDTO[] request)
    {
        var processingTimeMS = Stopwatch.StartNew();
        _logger.LogInformation("Iniciando processamento de {Length} trades", request.Length);

        var resume = new ConcurrentDictionary<string, TradeMetrics>();
        List<string> categorias = [];
        request.AsParallel().ForAll(p =>
        {
            var category = Classification(p);
            categorias.Add(category);

            resume.AddOrUpdate(category,
                (_) =>
                {
                    var metric = new TradeMetrics();
                    metric.Sum(p.Client, p.Value);
                    return metric;
                },
                (_, existis) =>
                {
                    existis.Sum(p.Client, p.Value);
                    return existis;
                }
                );
        });

        processingTimeMS.Stop();

        _logger.LogInformation("Relatorio distribuido concluido.");

        return new DistribuitionCalculatedDTO(
            Categories: categorias.ToArray(),
            Summary: resume.ToDictionary(k => k.Key, v => v.Value),
            ProcessingTimeMS: 0
            );
    }

    private string Classification(ClassifyRiskRequestDTO dto)
    {
        var trade = new Trade { Value = dto.Value, ClientSector = dto.ClientSector };
        return trade.ClassifyRisk().ToString();
    }

    private string Classification(DistributionRiskRequestDTO dto)
    {
        var trade = new Trade { Value = dto.Value, ClientSector = dto.ClientSector };
        return trade.ClassifyRisk().ToString();
    }
}
