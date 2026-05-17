using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CryptoViz.UI;
using MudBlazor.Services;
using CryptoViz.Core.Brokers.Maths;
using CryptoViz.Core.Services.Foundations.ElGamals;
using CryptoViz.Core.Services.Foundations.ECElGamals;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add MudBlazor for "Antigravity" UI
builder.Services.AddMudServices();

// The Standard: Register Brokers
builder.Services.AddTransient<IMathBroker, MathBroker>();
builder.Services.AddTransient<IECMathBroker, ECMathBroker>();

// The Standard: Register Services
builder.Services.AddTransient<IElGamalService, ElGamalService>();
builder.Services.AddTransient<IECElGamalService, ECElGamalService>();

await builder.Build().RunAsync();
