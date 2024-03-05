using Microsoft.EntityFrameworkCore;
using MvcGame.Data;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;


namespace MvcGame.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcGameContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcGameContext>>()))
            {
                if (context.Game.Any())
                {
                    return;   // DB has been seeded
                }
                context.Game.AddRange(
                    new Game
                    {
                        Title = "Elden Ring",
                        Description = "Elden Ring is an action role-playing game, set in third-person perspective. It includes elements that are similar to those in other FromSoftware-developed games such as the Dark Souls series, Bloodborne, and Sekiro: Shadows Die Twice.",
                        Genre = "Souls Like",
                        Price = 70.00M, //price in euro
                        ReleaseDate = DateTime.Parse("2022-5-24")


                    });
                context.SaveChanges();
            }
        }
    }
}
