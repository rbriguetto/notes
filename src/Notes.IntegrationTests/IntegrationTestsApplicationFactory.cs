using Microsoft.AspNetCore.Mvc.Testing;

namespace Notes.IntegrationTests;

public class IntegrationTestsApplicationFactory: WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
    }
}
