using Microsoft.EntityFrameworkCore;
using Muse.Models.Twitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muse.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<TokenResponse> TwitchTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TokenResponse>()
                .Property(e => e.Scope)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
            base.OnModelCreating(modelBuilder);
        }
    }
}
