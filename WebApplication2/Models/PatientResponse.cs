using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class PatientResponse
    {
        public string Id { get; set; }
        public List<Appointment> History { get; set; }
    }
}
