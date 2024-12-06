using HospitalManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure Application Cookie for login and access denial paths
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Configure IdentityOptions
builder.Services.Configure<IdentityOptions>(options =>
{
    // Disabling email confirmation requirement, can be modified for production
    options.SignIn.RequireConfirmedEmail = false;
    // You can also configure other Identity options like Password and Lockout settings here
});

// Register any additional services for your app
builder.Services.AddAuthorization(options =>
{
    // Custom authorization policies can be added here if needed
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

// Seed roles and users (do not run this in production unless necessary)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Admin", "Patient" }; // List of roles to create

    foreach (var role in roles)
    {
        var roleExist = await roleManager.RoleExistsAsync(role);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed admin user
    var adminEmail = "admin@example.com";
    var adminPassword = "Admin@123"; // Use a strong password in production

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true // Set to true for production
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    // Optionally, seed patient user
    var patientEmail = "patient@example.com";
    var patientPassword = "Patient@123"; // Use a strong password in production

    if (await userManager.FindByEmailAsync(patientEmail) == null)
    {
        var patientUser = new IdentityUser
        {
            UserName = patientEmail,
            Email = patientEmail,
        };

        var patientResult = await userManager.CreateAsync(patientUser, patientPassword);
        if (patientResult.Succeeded)
        {
            await userManager.AddToRoleAsync(patientUser, "Patient");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Use HTTPS Strict Transport Security (HSTS) in production
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add Authentication and Authorization middleware
app.UseAuthentication();  // Make sure authentication comes before authorization
app.UseAuthorization();   // Ensures that authorization policies are applied

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
