using Microsoft.EntityFrameworkCore;
using Bloggie.Data;
using Bloggie.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Inject DbContext inside Services using DI
// Define the connection string as a value
var connectionString = builder.Configuration.GetConnectionString("MSSQLSRV");
builder.Services.AddDbContext<BloggieDbContext>(options =>
options.UseSqlServer(connectionString)
);

// Injecting ITagRepository
builder.Services.AddScoped<ITagRepository, TagRepository>();

// Injecting IBlogPostRepository
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();

// Injecting Cloudinary 
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();
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
    pattern: "{controller=home}/{action=Index}/{id?}");

app.Run();
