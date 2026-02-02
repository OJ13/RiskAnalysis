using RiskAnalisys.Domain.Structs;

namespace RiskAnalisys.Application.DTO.Responses;

public record DistribuitionCalculatedDTO(
    string[] Categories,
    IReadOnlyDictionary<string, TradeMetrics> Summary,
    long ProcessingTimeMS
    );

