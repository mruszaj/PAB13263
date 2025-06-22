using AdminPanel.Data;
using AdminPanel.Services;
using ApiRest.SoapService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AdminPanel
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireUser", policy => policy.RequireRole("User"));
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("AdminPanelDb"));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddScoped<SoapProductsClient>();
            builder.Services.AddHttpClient<IProductsSoapService, ProductsSoapService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });
            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient("api", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
            });
            var app = builder.Build();
            app.Lifetime.ApplicationStarted.Register(async () =>
            {
                using var scope = app.Services.CreateScope();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                string[] roles = { "Admin", "User" };
                foreach (var role in roles)
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));

                var admin = new IdentityUser { UserName = "test@test.pl", Email = "test@test.pl" };
                if (await userManager.FindByEmailAsync(admin.Email) == null)
                {
                    await userManager.CreateAsync(admin, "Password123!");
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            });



            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            builder.Services.AddAuthorization();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapGet("/ping", () => "pong");
            app.Run();
        }
    }
}
