using RiskAnalisys.Domain.Entities;
using RiskAnalisys.Domain.Enums;

namespace RiskAnalisys.Tests.Domain.Entities;

/// <summary>
/// Testes unitários para a classe Trade
/// Padrão: Arrange-Act-Assert (AAA)
/// Nomenclatura: Method_Scenario_ExpectedResult
/// </summary>
public class TradeTests
{
    [Fact]
    public void ClassifyRisk_WithValueLessThanOneMillion_ReturnsLowRisk()
    {
        // Arrange
        var trade = new Trade
        {
            Value = 500000,
            ClientSector = ClientSector.PUBLIC
        };

        // Act
        var result = trade.ClassifyRisk();

        // Assert
        Assert.Equal(RiskCategory.LOWRISK, result);
    }

    [Fact]
    public void ClassifyRisk_WithValueLessThanOneMillionPrivateSector_ReturnsLowRisk()
    {
        // Arrange
        var trade = new Trade
        {
            Value = 999999,
            ClientSector = ClientSector.PRIVATE
        };

        // Act
        var result = trade.ClassifyRisk();

        // Assert
        Assert.Equal(RiskCategory.LOWRISK, result);
    }

    [Fact]
    public void ClassifyRisk_WithValueEqualToOneMillionPublicSector_ReturnsMediumRisk()
    {
        // Arrange
        var trade = new Trade
        {
            Value = 1000000,
            ClientSector = ClientSector.PUBLIC
        };

        // Act
        var result = trade.ClassifyRisk();

        // Assert
        Assert.Equal(RiskCategory.MEDIUMRISK, result);
    }

    [Fact]
    public void ClassifyRisk_WithValueGreaterThanOneMillionPublicSector_ReturnsMediumRisk()
    {
        // Arrange
        var trade = new Trade
        {
            Value = 5000000,
            ClientSector = ClientSector.PUBLIC
        };

        // Act
        var result = trade.ClassifyRisk();

        // Assert
        Assert.Equal(RiskCategory.MEDIUMRISK, result);
    }

    [Fact]
    public void ClassifyRisk_WithValueEqualToOneMillionPrivateSector_ReturnsHighRisk()
    {
        // Arrange
        var trade = new Trade
        {
            Value = 1000000,
            ClientSector = ClientSector.PRIVATE
        };

        // Act
        var result = trade.ClassifyRisk();

        // Assert
        Assert.Equal(RiskCategory.HIGHRISK, result);
    }

    [Fact]
    public void ClassifyRisk_WithValueGreaterThanOneMillionPrivateSector_ReturnsHighRisk()
    {
        // Arrange
        var trade = new Trade
        {
            Value = 10000000,
            ClientSector = ClientSector.PRIVATE
        };

        // Act
        var result = trade.ClassifyRisk();

        // Assert
        Assert.Equal(RiskCategory.HIGHRISK, result);
    }

    [Fact]
    public void ClassifyRisk_WithZeroValue_ReturnsLowRisk()
    {
        // Arrange
        var trade = new Trade
        {
            Value = 0,
            ClientSector = ClientSector.PUBLIC
        };

        // Act
        var result = trade.ClassifyRisk();

        // Assert
        Assert.Equal(RiskCategory.LOWRISK, result);
    }

    [Theory]
    [InlineData(500000, ClientSector.PUBLIC, RiskCategory.LOWRISK)]
    [InlineData(999999.99, ClientSector.PRIVATE, RiskCategory.LOWRISK)]
    [InlineData(1000000, ClientSector.PUBLIC, RiskCategory.MEDIUMRISK)]
    [InlineData(5000000, ClientSector.PUBLIC, RiskCategory.MEDIUMRISK)]
    [InlineData(1000000, ClientSector.PRIVATE, RiskCategory.HIGHRISK)]
    [InlineData(10000000, ClientSector.PRIVATE, RiskCategory.HIGHRISK)]
    public void ClassifyRisk_WithVariousValuesAndSectors_ReturnsCorrectRiskCategory(
        decimal value, 
        ClientSector sector, 
        RiskCategory expectedRisk)
    {
        // Arrange
        var trade = new Trade
        {
            Value = value,
            ClientSector = sector
        };

        // Act
        var result = trade.ClassifyRisk();

        // Assert
        Assert.Equal(expectedRisk, result);
    }

    [Fact]
    public void Trade_PropertyAssignment_WorksCorrectly()
    {
        // Arrange
        var trade = new Trade();

        // Act
        trade.Value = 2500000;
        trade.ClientSector = ClientSector.PRIVATE;

        // Assert
        Assert.Equal(2500000, trade.Value);
        Assert.Equal(ClientSector.PRIVATE, trade.ClientSector);
    }
}
