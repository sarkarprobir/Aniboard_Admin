using Workflow.Helper;
using Workflow.Service;
using Workflow.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddHttpClient<APIService>(client =>
{
    client.BaseAddress = new Uri(Settings.apiBaseurl); 
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    // Set the maximum file size for all multipart bodies to 3 MB
    options.MultipartBodyLengthLimit = 3145728;
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 3145728; // 3 MB
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
