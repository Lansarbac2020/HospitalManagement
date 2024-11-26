using HospitalManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Configure Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add services
builder.Services.AddControllersWithViews();

var app = builder.Build();

//// Seed roles
//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

//    // Ensure roles exist
//    string[] roles = { "Admin", "User" };
//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//    }

//    // Optionally seed an admin user
//    var adminEmail = "admin@example.com";
//    var adminPassword = "Admin@123"; // Ensure this is strong in production

//    if (await userManager.FindByEmailAsync(adminEmail) == null)
//    {
//        var adminUser = new IdentityUser
//        {
//            UserName = adminEmail,
//            Email = adminEmail,
//            EmailConfirmed = true
//        };

//        var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);

//        if (createAdminResult.Succeeded)
//        {
//            await userManager.AddToRoleAsync(adminUser, "Admin");
//        }
//    }
//}

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
