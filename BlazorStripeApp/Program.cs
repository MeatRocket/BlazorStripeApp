using AngryMonkey.Core;
using BlazorStripeApp.Data;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

//StripeConfiguration.ApiKey = "sk_test_51Jv11MEKoXqfsULcr96mvrkfXkahxUEvPfN4DcS8m4Q9ctQqjovxEBLHgZkH19EeZM4zDhb4EPN5i3EiSVUrGtYJ00gd5XLKFd";

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

StripeClients.Data = new StripePaymentData(
    CoreConfig.Configuration["TableStorage:ConnectionString"],
    CoreConfig.Configuration["TableStorage:StripeUserTableName"]
    );

StripeClients.Settings = new(CoreConfig.Configuration["Stripe:PublishableKey"]);
StripeConfiguration.ApiKey = CoreConfig.Configuration["Stripe:SecretKey"];

app.Run();
