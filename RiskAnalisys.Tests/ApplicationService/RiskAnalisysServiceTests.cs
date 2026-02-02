using System.Linq;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using RiskAnalisys.Application.Services;
using RiskAnalisys.Application.DTO.Requests;
using RiskAnalisys.Application.DTO.Responses;
using RiskAnalisys.Domain.Structs;
using RiskAnalisys.Domain.Enums;

namespace RiskAnalisys.Tests.ApplicationService;

public class RiskAnalisysServiceTests
{
    private readonly RiskAnalisysService _service;

    public RiskAnalisysServiceTests()
    {
        var loggerMock = new Mock<ILogger<RiskAnalisysService>>();
        _service = new RiskAnalisysService(loggerMock.Object);
    }

    [Fact]
    public void ClassifyRisk_ShouldReturnExpectedCategories()
    {
        // Arrange
        var requests = new[] {
            new ClassifyRiskRequestDTO(500000m, ClientSector.PRIVATE),
            new ClassifyRiskRequestDTO(2000000m, ClientSector.PUBLIC),
            new ClassifyRiskRequestDTO(2000000m, ClientSector.PRIVATE),
        };

        // Act
        ClassifyRiskDTO result = _service.ClassifyRisk(requests);

        // Assert
        Assert.Equal(new[] { "LOWRISK", "MEDIUMRISK", "HIGHRISK" }, result.Categories);
    }

    [Fact]
    public void DistributionRisk_ShouldReturnAggregatedSummaryAndCategories()
    {
        // Arrange
        var requests = new[] {
            new DistributionRiskRequestDTO(500000m, ClientSector.PRIVATE, "ClientA"),
            new DistributionRiskRequestDTO(500000m, ClientSector.PRIVATE, "ClientB"),
            new DistributionRiskRequestDTO(2000000m, ClientSector.PUBLIC, "ClientC"),
            new DistributionRiskRequestDTO(3000000m, ClientSector.PRIVATE, "ClientD")
        };

        // Act
        DistribuitionCalculatedDTO result = _service.DistributionRisk(requests);

        // Assert - categories preserved in order
        Assert.Equal(new[] { "LOWRISK", "LOWRISK", "MEDIUMRISK", "HIGHRISK" }, result.Categories);

        // Assert - summary contains aggregated metrics
        Assert.True(result.Summary.ContainsKey("LOWRISK"));
        var lowMetrics = result.Summary["LOWRISK"];
        Assert.Equal(2, lowMetrics.Count);
        Assert.Equal(1000000m, lowMetrics.TotalValue);

        Assert.True(result.Summary.ContainsKey("MEDIUMRISK"));
        var medMetrics = result.Summary["MEDIUMRISK"];
        Assert.Equal(1, medMetrics.Count);
        Assert.Equal(2000000m, medMetrics.TotalValue);

        Assert.True(result.Summary.ContainsKey("HIGHRISK"));
        var highMetrics = result.Summary["HIGHRISK"];
        Assert.Equal(1, highMetrics.Count);
        Assert.Equal(3000000m, highMetrics.TotalValue);

        // Processing time should be non-negative
        Assert.True(result.ProcessingTimeMS >= 0);
    }

    [Fact]
    public void ClassifyRisk_WithNegativeValue_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var requests = new[] {
            new ClassifyRiskRequestDTO(-100m, ClientSector.PRIVATE),
            new ClassifyRiskRequestDTO(500000m, ClientSector.PRIVATE)
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => _service.ClassifyRisk(requests));
        Assert.Contains("negativos", ex.Message);
    }

    [Fact]
    public void DistributionRisk_WithNegativeValue_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var requests = new[] {
            new DistributionRiskRequestDTO(-100m, ClientSector.PRIVATE, "ClientA"),
            new DistributionRiskRequestDTO(500000m, ClientSector.PRIVATE, "ClientB")
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => _service.DistributionRisk(requests));
        Assert.Contains("negativos", ex.Message);
    }
}