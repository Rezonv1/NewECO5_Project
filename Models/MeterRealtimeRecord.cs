using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewECO5.Models
{
    public class MeterRealtimeRecord
    {
        [Key]
        public int Id { get; set; }

        public int SerialNr { get; set; }

        public float TotalKW { get; set; }

        public float VoltageA { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
