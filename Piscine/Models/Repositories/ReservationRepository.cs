using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Piscine.Models;
using Piscine.Data;
using Piscine.Controllers;
using Piscine.Models.contract;

namespace Piscine.Models.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _db;
        public ReservationRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Create(Reservation entity)
        {
            var reservation = _db.reservations.Add(entity);
            return Save();
        }

        public bool Delete(Reservation entity)
        {
            var reservation = _db.reservations.Remove(entity);
            return Save();
        }

        public List<Reservation> GetAll()
        {
            var reservation = _db.reservations.Include(x => x.Client).Include(x => x.TypeReservation).ToList();
             
            return reservation;
        }

        public Reservation GetById(int id)
        {
            var reservation = _db.reservations.Include(x=>x.Client).Include(x=>x.TypeReservation).FirstOrDefault(x=>x.Id==id);

            return reservation;
        }

        public bool IsExist(int id)
        {
            var exists = _db.reservations.Any(x => x.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(Reservation entity)
        {
            var reservation = _db.reservations.Update(entity);
            return Save();
        }
    }
}
