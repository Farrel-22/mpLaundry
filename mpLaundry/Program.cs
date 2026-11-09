using mpLaundry.Services.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──
builder.Services.AddControllersWithViews();
builder.Services.AddMpLaundryServices(builder.Configuration);

var app = builder.Build();

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
