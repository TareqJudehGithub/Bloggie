using Microsoft.EntityFrameworkCore;
using Bloggie.Data;
using Bloggie.Repositories;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Inject DbContext - DB connection inside Services using DI
// Define the connection string as a value
var dBConnectionString = builder.Configuration.GetConnectionString("MSSQLSRV");
builder.Services.AddDbContext<BloggieDbContext>(options =>
options.UseSqlServer(dBConnectionString)
);

// Inject DbContext - Identity connection 
var authConnectionString = builder.Configuration.GetConnectionString("BloogieAuthDb");
builder.Services.AddDbContext<AuthDbContext>(options =>
options.UseSqlServer(authConnectionString)
);
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

// Injecting repository services into the app:
// ITagRepository
builder.Services.AddScoped<ITagRepository, TagRepository>();
// IBlogPostRepository
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
// BlogPostLikesRepitory
builder.Services.AddScoped<IBlogPostLikesRepository, BlogPostLikesRepository>();
// Cloudinary 
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

// Authenticate the Identity users
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=Index}/{id?}");

app.Run();
