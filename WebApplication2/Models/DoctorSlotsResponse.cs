using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class DoctorSlotsResponse
    {
        public string DoctorId { get; set; }
        public List<String> Slots { get; set; }


    }
}
