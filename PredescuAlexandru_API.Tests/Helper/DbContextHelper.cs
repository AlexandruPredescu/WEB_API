using Microsoft.EntityFrameworkCore;
using PredescuAlexandru_API.DataContext;
using PredescuAlexandru_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredescuAlexandru_API.Tests.Helper
{
    internal class DbContextHelper
    {
        public static ClubLibraDataContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ClubLibraDataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())  //configurare si utilizarea unei baze de date in memorie
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            var databaseContext = new ClubLibraDataContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        public static Announcement AddAnnouncement(ClubLibraDataContext dbContext, Announcement model)
        {
            dbContext.Add(model);
            dbContext.SaveChanges();
            dbContext.Entry(model).State = EntityState.Detached;
            return model;
        }
    }
}
