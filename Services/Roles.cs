using Microsoft.AspNetCore.Identity;

namespace ST2_2026_Team_4_GameLibrary.Services
{
    public class Roles
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public Roles(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedRolesAsync()
        {
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    
    
    
    
    
    }
}
