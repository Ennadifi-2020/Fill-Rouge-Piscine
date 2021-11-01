using Piscine.Data;
using Piscine.Models;
using Piscine.Models.contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piscine.Models.Repositories
{
    public class ReservationTypeRepository : IReservationTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public ReservationTypeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool Create(TypeReservation entity)
        {
            var reservationType = _db.typeReservations.Add(entity);
            return Save();
        }

        public bool Delete(TypeReservation entity)
        {
            var reservationType = _db.typeReservations.Remove(entity);
            return Save();
        }

        public List<TypeReservation> GetAll()
        {
            var reservationTypes = _db.typeReservations.ToList();
            return reservationTypes;
        }

        public TypeReservation GetById(int id)
        {
            var reservationType = _db.typeReservations.Find(id);
            return reservationType;
        }

        public bool IsExist(int id)
        {
            var exists = _db.typeReservations.Any(x => x.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(TypeReservation entity)
        {
            var reservationType = _db.typeReservations.Update(entity);
            return Save();
        }
    }
}
