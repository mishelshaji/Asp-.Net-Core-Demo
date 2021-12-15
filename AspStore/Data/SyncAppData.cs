namespace AspStore.Data
{
    public class SyncAppData
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public SyncAppData(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;

            CreateRoles().Wait();
            CreateUser().Wait();
        }

        private async Task CreateRoles()
        {
            if(!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = UserRoles.Admin });
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.Staff))
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = UserRoles.Staff });
            }
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = UserRoles.User });
            }
        }

        public async Task CreateUser()
        {
            var adminUser = new ApplicationUser()
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@admin.com",
                UserName = "admin"
            };
            string adminUserPassword = "Pass@123";

            var adminUsersFromdb = await _userManager.GetUsersInRoleAsync(UserRoles.Admin);
            if (!adminUsersFromdb.Any())
            {
                var userCreated = await _userManager.CreateAsync(adminUser, adminUserPassword);
                if (!userCreated.Succeeded)
                {
                    throw new ApplicationException("Admin user creation failed");
                }
                var addedToRole = await _userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                if (!addedToRole.Succeeded)
                {
                    throw new ApplicationException("Unable to add admin to role");
                }
            }
        }
    }
}
