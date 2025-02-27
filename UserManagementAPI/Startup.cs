public class Startup
{
    // ...existing code...

    public void ConfigureServices(IServiceCollection services)
    {
        // ...existing code...
        services.AddHttpsRedirection(options =>
        {
            options.HttpsPort = 7136; // Set the HTTPS port explicitly
        });
        // ...existing code...
    }

    // ...existing code...
}
