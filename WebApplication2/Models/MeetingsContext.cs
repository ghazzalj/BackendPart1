using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Authentication;

namespace WebApplication2.Models
{
    public class MeetingsContext : IdentityDbContext<ApplicationUser>
    {

        public MeetingsContext(DbContextOptions<MeetingsContext> context)
           : base(context)
        {
        }

        public DbSet<Appointment> Appointment { get; set; }
       // public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }




    }
}
