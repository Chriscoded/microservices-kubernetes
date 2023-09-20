using Microsoft.EntityFrameworkCore;
using PlatformService.Model;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                Console.WriteLine($"is environment Production : {isProd}");
                //get db context service
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        public static void SeedData(AppDbContext context,bool isProd)
        {
            
            if(isProd){
                 Console.WriteLine("--> Attempting to apply migrations...");
                try{
                    context.Database.Migrate();
                }
                catch(Exception ex){
                    Console.WriteLine($"--> could not run migrations: {ex.Message}");
                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine(" --> Seeding Data...");
                context.Platforms.AddRange(
                        new Platform()
                        {
                            Name = "Dot Net",
                            Publisher = "Microsoft",
                            Cost = "Free"
                        },
                        new Platform()
                        {
                            Name = "Sql Server Express",
                            Publisher = "Microsoft",
                            Cost = "Free"
                        },
                        new Platform()
                        {
                            Name = "Kubernetes",
                            Publisher = "Cloud native computing foundation",
                            Cost = "Free"
                        }
                    );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine(" --> We already have data");
            }
        }
    }
}
