using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using stec_util.Data;
using stec_util.Data.jira;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//Add configuration
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<IJiraService, JiraService>(x =>
{
  var config = x.GetRequiredService<IConfiguration>();
  var jiraConfig = config.GetSection("Jira").Get<JiraConfig>();
  return new JiraService(jiraConfig);
});

var app = builder.Build();

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

app.Run();
