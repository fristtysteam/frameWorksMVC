﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcGame.Models;

namespace MvcGame.Data
{
    public class MvcGameContext : IdentityDbContext
    {
        public MvcGameContext (DbContextOptions<MvcGameContext> options)
            : base(options)
        {
        }

        public DbSet<MvcGame.Models.Game> Game { get; set; }
        public DbSet<MvcGame.Models.Studio> Studio { get; set; } = default!;
    }




}
