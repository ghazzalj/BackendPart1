using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class SlotRequest
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DoctorID { get; set; }
        public string PatientID { get; set; }
    }
}
