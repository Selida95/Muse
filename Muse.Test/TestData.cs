using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using Muse.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muse.Test
{
    public static class TestData
    {
        public static IDbContextFactory<DataContext> GetDbContextFactory()
        {
            DbContextOptionsBuilder<DataContext> builder = new DbContextOptionsBuilder<DataContext>();

            // Use Sqlite DB
            builder.UseSqlite("Datasource=./test.db", x => { });

            var options = builder.Options;

            DataContext dbContext = new DataContext(options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            //dbContext.Database.Migrate();

            // SQLite needs to open connection to the DB.
            dbContext.Database.OpenConnection();

            var mockDbContextFactory = new Mock<IDbContextFactory<DataContext>>();
            mockDbContextFactory.Setup(c => c.CreateDbContext()).Returns(dbContext);

            return mockDbContextFactory.Object;
        }
    }
}
