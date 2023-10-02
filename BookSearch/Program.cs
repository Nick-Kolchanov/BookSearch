using BookSearch.Services;
using BookSearchAPI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<BookDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Db")));
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<IRequestService, RequestService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(12);
    options.Cookie.Name = ".BookSearch.Session";
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/LoginPage";
    });

builder.Services.AddAuthorization(options =>
      options.AddPolicy("IsAdmin",
      policy => policy.RequireClaim("role", "Admin")));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
