using Microsoft.AspNetCore.Mvc.Testing;

namespace RiskAnalisys.Tests.IntegrationTests;

internal class RiskAnalisysWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);
    }
}
