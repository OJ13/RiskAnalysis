using FluentAssertions;
using Newtonsoft.Json;
using RiskAnalisys.Application.DTO.Requests;
using RiskAnalisys.Domain.Enums;
using System.Net;
using System.Text;

namespace RiskAnalisys.Tests.IntegrationTests.Controllers;

public class RiskAnalisysControllerTests
{
    private const string BaseUrl = "/api/risk-analisys";
    
    [Fact]
    public async Task PostClassify_ReturnsOkResult()
    {
        // Arrange
        var application = new RiskAnalisysWebApplicationFactory();
        var client = application.CreateClient();

         var requests = new[] {
            new ClassifyRiskRequestDTO(500000m, ClientSector.PRIVATE),
            new ClassifyRiskRequestDTO(2000000m, ClientSector.PUBLIC),
            new ClassifyRiskRequestDTO(2000000m, ClientSector.PRIVATE),
        };

        var body = new StringContent(JsonConvert.SerializeObject(requests), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/classify", body);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostClassify_Returns400Erro()
    {
        // Arrange
        var application = new RiskAnalisysWebApplicationFactory();
        var client = application.CreateClient();

        var requests = new[] {
            new ClassifyRiskRequestDTO(-500000m, ClientSector.PRIVATE),
            new ClassifyRiskRequestDTO(2000000m, ClientSector.PUBLIC),
            new ClassifyRiskRequestDTO(2000000m, ClientSector.PRIVATE),
        };

        var body = new StringContent(JsonConvert.SerializeObject(requests), 
        Encoding.UTF8, 
        "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/classify", body);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    

    [Fact]
    public async Task PostDistribution_ReturnsOkResult()
    {
        // Arrange
        var application = new RiskAnalisysWebApplicationFactory();
        var client = application.CreateClient();

        var requests = new[] {
            new DistributionRiskRequestDTO(500000m, ClientSector.PRIVATE, "ClientA"),
            new DistributionRiskRequestDTO(500000m, ClientSector.PRIVATE, "ClientB"),
            new DistributionRiskRequestDTO(2000000m, ClientSector.PUBLIC, "ClientC"),
            new DistributionRiskRequestDTO(3000000m, ClientSector.PRIVATE, "ClientD")
        };

        var body = new StringContent(JsonConvert.SerializeObject(requests), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/classify", body);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostDistribution_Returns400Erro()
    {
        // Arrange
        var application = new RiskAnalisysWebApplicationFactory();
        var client = application.CreateClient();

        var requests = new[] {
            new DistributionRiskRequestDTO(-500000m, ClientSector.PRIVATE, "ClientA"),
            new DistributionRiskRequestDTO(500000m, ClientSector.PRIVATE, "ClientB"),
            new DistributionRiskRequestDTO(2000000m, ClientSector.PUBLIC, "ClientC"),
            new DistributionRiskRequestDTO(3000000m, ClientSector.PRIVATE, "ClientD")
        };

        var body = new StringContent(JsonConvert.SerializeObject(requests), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync($"{BaseUrl}/classify", body);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

}
