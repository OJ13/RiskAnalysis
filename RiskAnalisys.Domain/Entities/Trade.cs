using RiskAnalisys.Domain.Enums;

namespace RiskAnalisys.Domain.Entities;

public class Trade
{
    public decimal Value { get; set; }
    public ClientSector ClientSector { get; set; }

    public RiskCategory ClassifyRisk() => (Value, ClientSector) switch
    {
        ( < 1000000, _) => RiskCategory.LOWRISK,
        ( >= 1000000, ClientSector.PUBLIC) => RiskCategory.MEDIUMRISK,
        ( >= 1000000, ClientSector.PRIVATE) => RiskCategory.HIGHRISK,
        _ => RiskCategory.NOTCLASSIFIED
    };
}
