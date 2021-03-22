using System;
using System.Collections.Generic;
using System.Text;

namespace EFDataAccessLibrary.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public User User { get; set; }
        public Bike Bike { get; set; }
    }
}
