
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#if DEBUG
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
#endif

// Umbraco can fail to create the SQLite file on first run ("Boot failed"), so create an empty one.
if (builder.Environment.IsDevelopment())
{
    string dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "umbraco", "Data");
    string databaseFile = Path.Combine(dataDirectory, "Umbraco.sqlite.db");
    if (!File.Exists(databaseFile))
    {
        Directory.CreateDirectory(dataDirectory);
        File.Create(databaseFile).Dispose();
    }
}

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

WebApplication app = builder.Build();


await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
