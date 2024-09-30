using Cloud_computing_project_LAST.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cloud_computing_project_LAST.Services;
using Microsoft.AspNetCore.Identity.UI.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();
app.UseSession();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Manager", "Customer" };

    foreach (var role in roles)
    {
        //if (!await roleManager.RoleExistsAsync(role))
        //    await roleManager.CreateAsync(new IdentityRole(role));
    }
}

//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

//    var email = "ayalahaim20@gmail.com";
//    var password = "AyalaHaim!1";

//    if (await userManager.FindByEmailAsync(email) == null)
//    {
//        var user = new IdentityUser();
//        user.UserName = email;
//        user.Email = email;
//        user.EmailConfirmed = true;
       

//        await userManager.CreateAsync(user, password);
//        await userManager.AddToRoleAsync(user, "Manager");
//    }

//}

app.Run();
