namespace RiskAnalisys.Application.DTO.Responses;

public record ClassifyRiskDTO(
    string[] Categories,
    long ProcessingTimeMS = 0
    );

