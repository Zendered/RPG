using Microsoft.EntityFrameworkCore;
using RPG.Data;

namespace RPG.Services
{
    public static class DatabaseManagementService
    {
        public static void MigrationInitialisation(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var serviceDb = serviceScope.ServiceProvider.GetService<CharactersContext>();

                serviceDb.Database.Migrate();
            }
        }
    }
}
