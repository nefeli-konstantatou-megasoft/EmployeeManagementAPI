using EmployeeManagementFrontend;
using EmployeeManagementFrontend.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var backendURI = "https://localhost:7219";

builder.Services.AddHttpClient<EmployeeService>(httpClient => { httpClient.BaseAddress = new Uri(backendURI); });
builder.Services.AddHttpClient<DepartmentService>(httpClient => { httpClient.BaseAddress = new Uri(backendURI); });
builder.Services.AddDevExpressBlazor();

await builder.Build().RunAsync();
