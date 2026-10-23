
using Microsoft.AspNetCore.Identity;
using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Services
{
    public class Admin
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public Admin(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task SeedAdminAsync()
        {
            var adminUserName = _config["AdminDefaults:UserName"];
            var adminEmail = _config["AdminDefaults:Email"];
            var adminPassword = _config["AdminDefaults:Password"];

            var adminExists = await _userManager.FindByNameAsync(adminUserName);
            if (adminExists == null)
            {
                var admin = new User
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
