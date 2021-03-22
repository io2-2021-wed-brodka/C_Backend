using System;
using System.Collections.Generic;
using System.Text;

namespace EFDataAccessLibrary.Models
{
    public class Malfunction
    {
        public int Id { get; set; }
        public Bike Bike { get; set; }
        public DateTime DetectionDate { get; set; }
        public User ReportingUser { get; set; }
        public string Description { get; set; }
        public MalfunctionState State { get; set; }
    }


    public enum MalfunctionState
    {
        Fixed,
        NotFixed
    }
}
