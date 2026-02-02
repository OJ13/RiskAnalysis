using System.Text.Json.Serialization;

namespace RiskAnalisys.Domain.Structs;
public struct TradeMetrics
{
    [JsonInclude]
    public int Count;
    [JsonInclude]
    public decimal TotalValue;
    [JsonInclude]
    public string TopClient;
    
    public decimal MaxValue;

    public void Sum(string client, decimal valor)
    {
        Count++;
        TotalValue += valor;
        if (valor > MaxValue)
        {
            MaxValue = valor;
            TopClient = client;
        }
    }
}