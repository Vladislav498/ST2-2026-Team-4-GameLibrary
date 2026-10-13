using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ST2_2026_Team_4_GameLibrary.Models;
using ST2_2026_Team_4_GameLibrary.Data;
using ST2_2026_Team_4_GameLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
   .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

//slagam seedovete v bazata 
//kato rabotite posle triite app.db i ako izleznat podobni failove

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
   
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        
        //GamesSeed.Initialize(context);

   // builder.Services.AddSingleton<FeaturedGamesSingleton>(FeaturedGamesSingleton.GetSingleInstance(scope.ServiceProvider.GetRequiredService<AppDbContext>()));


    // context.Add<FeaturedGamesSingleton>(FeaturedGamesSingleton.GetSingleInstance(context));
}

app.Run();
