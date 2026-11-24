using System.Globalization;
using Microsoft.AspNetCore.Localization;
using mpLaundry.Services.Infrastructure;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// ── Services ──
builder.Services.AddControllersWithViews();
builder.Services.AddMpLaundryServices(builder.Configuration);

// ── Konfigurasi Culture agar Desimal (2.5) dari Browser Terbaca Presisi ──
var culture = new CultureInfo("id-ID");
culture.NumberFormat.NumberDecimalSeparator = ".";
culture.NumberFormat.NumberGroupSeparator = ",";
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var app = builder.Build();

// Pasang middleware Localization
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture),
    SupportedCultures = [culture],
    SupportedUICultures = [culture]
});

// ── Middleware Pipeline ──
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();