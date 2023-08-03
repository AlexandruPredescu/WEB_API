using Microsoft.EntityFrameworkCore;
using PredescuAlexandru_API.Models;
using System.Collections.Generic;

namespace PredescuAlexandru_API.DataContext
{
    public class ClubLibraDataContext : DbContext
    {
        
            public ClubLibraDataContext(DbContextOptions<ClubLibraDataContext> options) : base(options) { }


        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<CodeSnippet> CodeSnippets { get; set; }


    }
}
