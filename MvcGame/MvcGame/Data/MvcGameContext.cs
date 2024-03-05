using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcGame.Models;

namespace MvcGame.Data
{
    public class MvcGameContext : DbContext
    {
        public MvcGameContext (DbContextOptions<MvcGameContext> options)
            : base(options)
        {
        }

        public DbSet<MvcGame.Models.Game> Game { get; set; }
    }
}
