using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Piscine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Piscine.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Client> clients { get; set; }
        public DbSet<Reservation> reservations { get; set; }
        public DbSet<TypeReservation> typeReservations { get; set; }

    }
}
