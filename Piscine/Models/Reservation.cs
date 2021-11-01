using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Piscine.Models
{
    public class Reservation
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }

        [ForeignKey("ClientId,TypeReservationId")]
        public string ClientId { get; set; }
        public Client Client { get; set; }

        public int TypeReservationId { get; set; }
        public TypeReservation TypeReservation { get; set; }


    }
}
