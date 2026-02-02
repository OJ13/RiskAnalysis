using RiskAnalisys.Domain.Enums;

namespace RiskAnalisys.Domain.Entities;

public class Trade
{
    public decimal Value { get; set; }
    public ClientSector ClientSector { get; set; }

    // Como era uma lógica simples, optei por implementar o método de classificação de risco diretamente com switch expression.
    // Caso a lógica fosse mais complexa, poderia ter criado classes específicas para cada regra de negócio com o padrao Strategy por exemplo.
    public RiskCategory ClassifyRisk() => (Value, ClientSector) switch
    {
        ( < 1000000, _) => RiskCategory.LOWRISK,
        ( >= 1000000, ClientSector.PUBLIC) => RiskCategory.MEDIUMRISK,
        ( >= 1000000, ClientSector.PRIVATE) => RiskCategory.HIGHRISK,
        _ => RiskCategory.NOTCLASSIFIED
    };
}
