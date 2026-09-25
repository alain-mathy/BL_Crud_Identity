using BL_Crud_Identity.Client.Handlers;
using BL_Crud_Identity.Client.Services;
using BL_Crud_Identity.Shared.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddTransient<CookieHandler>();

// Register HttpClient with the base address of the host server
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register the client-side user service
builder.Services.AddScoped<IUserService, ClientUserService>();

await builder.Build().RunAsync();
