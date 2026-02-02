using Microsoft.Extensions.Logging;
using RiskAnalisys.Application.DTO.Requests;
using RiskAnalisys.Application.DTO.Responses;
using RiskAnalisys.Domain.Entities;
using RiskAnalisys.Domain.Enums;
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

        ValidateRequest(request.Select(p => p.Value));

        var response = request
                        .AsParallel()
                        .AsOrdered()
                        .Select(p => GetClassification(p.Value, p.ClientSector)).ToArray();

        _logger.LogInformation("Classificação concluída.");

        return new ClassifyRiskDTO(response);
    }

    public DistribuitionCalculatedDTO DistributionRisk(DistributionRiskRequestDTO[] request)
    {
        _logger.LogInformation("Iniciando processamento de {Length} trades", request.Length);

        ValidateRequest(request.Select(p => p.Value));

        var resume = new ConcurrentDictionary<string, TradeMetrics>();
        
        string[] categorias = request
                                .AsParallel()
                                .AsOrdered()
                                .Select(p => 
        {
            var category = GetClassification(p.Value, p.ClientSector);

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
            return category;
        }).ToArray();

        _logger.LogInformation("Relatorio distribuido concluido.");

        return new DistribuitionCalculatedDTO(
            Categories: categorias,
            Summary: resume.ToDictionary(k => k.Key, v => v.Value)
            );
    }

    private string GetClassification(decimal value, ClientSector sector)
    {
        var trade = new Trade { Value = value, ClientSector = sector };
        return trade.ClassifyRisk().ToString();
    }

    private void ValidateRequest(IEnumerable<decimal> values)
    {
        if (values.Any(v => v < 0))
            throw new InvalidOperationException("A lista contém trades com valores negativos.");
    }
}
