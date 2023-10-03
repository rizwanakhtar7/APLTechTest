using APLTechChallenge.Data;
using APLTechChallenge.Helpers;
using APLTechChallenge.Services;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAzureClients(clientBuilder =>
{

    clientBuilder.AddBlobServiceClient(
        builder.Configuration["AzureConfiguration:ConnectionString"]);
});


builder.Services.AddSingleton<IBlobStorageService, BlobStorageService>();


builder.Services.AddTransient<IImageService, ImageService>();

builder.Services.Configure<AzureDTO>(builder.Configuration.GetSection("AzureConfiguration"));

builder.Services.AddDbContext<ApplicationDBContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Upload}/{id?}");

app.Run();
