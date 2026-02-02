using Microsoft.AspNetCore.Mvc;
using RiskAnalisys.Application.DTO.Requests;
using RiskAnalisys.Application.DTO.Responses;
using RiskAnalisys.Domain.Structs;
using System.Text.Json.Serialization;

namespace RiskAnalisys.Application.Serialization;

[JsonSourceGenerationOptions(
    WriteIndented = false, 
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, 
    PropertyNameCaseInsensitive = true, 
    GenerationMode = JsonSourceGenerationMode.Default,
    Converters = [typeof(JsonStringEnumConverter)]
)]
[JsonSerializable(typeof(ClassifyRiskRequestDTO))]
[JsonSerializable(typeof(ClassifyRiskRequestDTO[]))]
[JsonSerializable(typeof(DistributionRiskRequestDTO))]
[JsonSerializable(typeof(DistributionRiskRequestDTO[]))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(DistribuitionCalculatedDTO))]
[JsonSerializable(typeof(DistribuitionCalculatedDTO[]))]
[JsonSerializable(typeof(TradeMetrics))]
[JsonSerializable(typeof(TradeMetrics[]))]
[JsonSerializable(typeof(ClassifyRiskDTO))]
[JsonSerializable(typeof(DistribuitionCalculatedDTO))]
public partial class RiskAnalisysJsonContext : JsonSerializerContext
{
}
