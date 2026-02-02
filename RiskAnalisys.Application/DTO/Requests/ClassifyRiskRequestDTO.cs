using RiskAnalisys.Domain.Enums;
using System.Text.Json.Serialization;

namespace RiskAnalisys.Application.DTO.Requests;
public record ClassifyRiskRequestDTO(
    [property: JsonRequired]
    decimal Value,

    [property: JsonRequired]
    ClientSector ClientSector);

public record DistributionRiskRequestDTO(
    [property: JsonRequired]
    decimal Value,

    [property: JsonRequired]
    ClientSector ClientSector, 

    string Client);

