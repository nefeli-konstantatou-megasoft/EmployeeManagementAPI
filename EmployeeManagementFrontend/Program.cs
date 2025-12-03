using EmployeeManagementFrontend;
using EmployeeManagementFrontend.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var httpClient = new
    HttpClient { BaseAddress = new Uri("https://localhost:7219") };

builder.Services.AddScoped(sp => httpClient);
builder.Services.AddScoped<EmployeeService>();

await builder.Build().RunAsync();
