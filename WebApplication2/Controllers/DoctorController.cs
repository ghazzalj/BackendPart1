using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorController : ControllerBase
    {
        private readonly MeetingsContext context;
        public DoctorController(MeetingsContext context)
        {
            this.context = context;
        }

        //All doctors => list of their names

        [HttpGet]
        public IEnumerable<String> GetAllDoctors()
        {

            string drRoleID = context.Roles.Where(d => d.Name.Equals("Doctor")).Select(x => x.Id).Single();
            List<String> drIds = context.UserRoles.Where(d => d.RoleId.Equals(drRoleID)).Select(x => x.UserId).ToList();
            List<String> drs = new List<string>();

            foreach (var n in drIds) {

                drs.Add(context.Users.Where(d => d.Id.Equals(n)).Select(x => x.UserName).Single());
            }


            return drs;


 
        }


        //Doctor information by ID
        //use find because we are returning 1 doctor
        [HttpGet("{id}")]
        public DoctorInfoResponse GetDoctorByID(string id)
        { /////////////////////// make a new response model?

            DoctorInfoResponse resp = new DoctorInfoResponse();
            resp.DoctorId = id;
            resp.Name = context.Users.Where(d => d.Id.Equals(id)).Select(x => x.UserName).Single();
            // resp.Slots = context.Appointments.Where(d => d.Id.Equals(id)).ToList();
            return resp;

        }

        //Doctor avail slots
        //get count and add duration, if the count is >12 or the duration is >=8 then no slots
        // 15 , 30, 45, 60, 75, 90, 105, 120

        [HttpGet("{id}/slots")]
        public IEnumerable<String> GetDoctorAvailSlots(string id) {

             int count = context.Appointment.Where(s => s.DoctorID.Equals(id)).Count();// slot count
             List<DateTime> startDuration = context.Appointment.Where(s => s.DoctorID.Equals(id)).Select(d => d.StartTime).ToList();
             List<DateTime> endDuration = context.Appointment.Where(s => s.DoctorID.Equals(id)).Select(d => d.EndTime).ToList();
             double totalDuration = 0.0;
             List<String> slots = new List<String>();//avail
             List<String> uslots = new List<String>();// unavail
             List<DateTime> sortedSlots = new List<DateTime>();//sorted list

             var duration = startDuration.Zip(endDuration, (s, e) => new { startDuration = s, endDuration = e }); 
            foreach (var se in duration)
            {
                //get time difference 
                System.TimeSpan timeSpan = se.endDuration.Subtract(se.startDuration);
                double mins = timeSpan.TotalMinutes; 
                totalDuration += mins; // total of mins allocated from all appointments 

                string unAvailSlot = se.startDuration.ToString("H:mm") + "-" + se.endDuration.ToString("H:mm");
                uslots.Add(unAvailSlot);

                //storing slots in a list (start,end)
                sortedSlots.Add(se.startDuration);
                sortedSlots.Add(se.endDuration);

            }

            // sort the list
            sortedSlots.Sort((a, b) => a.CompareTo(b));

            // if endtime1 != starttime2 then avail slot is endtime1-starttime

            for (int i = 0; i < sortedSlots.Count/2; i++) {

                if (sortedSlots[i + 1] != sortedSlots[i + 2])
                {
                    if (count != 12)
                    {
                        string slot = sortedSlots[i + 1].ToString("H:mm") + "-" + sortedSlots[i + 2].ToString("H:mm");
                        slots.Add(slot);
                        count++; 

                    }
                }
            
            }
            //to get the time in hrs
            totalDuration /= 60;

            if (count == 12 || totalDuration >= 8)
                return uslots;

            else
                return slots;

        }

        //All doctors that are avail

        [HttpGet("avail")]
        public IEnumerable<DoctorSlotsResponse> GetAllDoctorsAvailSlots() {

           List<string> ids =  context.Appointment.Select(x => x.DoctorID).Distinct().ToList();
           int totalDr = ids.Count();

            List<DoctorSlotsResponse> response = new List<DoctorSlotsResponse>();

            for (int i = 0; i < ids.Count;i++) {

                DoctorSlotsResponse dr = new DoctorSlotsResponse();
                dr.DoctorId = ids[i];
                dr.Slots = GetDoctorAvailSlots(ids[i]).ToList();
                response.Add(dr);
            }
            return response;

        }


        //Doctors with the most appointments 

        [HttpGet("mostSlots")]
        public IEnumerable<DoctorSlotsResponse> DoctorMostSlots()
        {

            List <DoctorSlotsResponse> dr = GetAllDoctorsAvailSlots().ToList();

            dr.Sort(delegate (DoctorSlotsResponse x, DoctorSlotsResponse y) {
                return y.Slots.Count().CompareTo(x.Slots.Count());
            });

            return dr;

        }


        //Doctors who have 6+ hours

        [HttpGet("sixHoursPlus")]
        public IEnumerable<DoctorSlotsResponse> DoctorSixHours()
        {

            List<DoctorSlotsResponse> dr = DoctorMostSlots().ToList();

            List<DoctorSlotsResponse> sixHoursList = new List<DoctorSlotsResponse>();

            for (int i = 0; i < dr.Count(); i++) {

                if (dr[i].Slots.Count()>6) {

                    sixHoursList.Add(dr[i]);                
                }
            
            }
            

            return sixHoursList;

        }





    }
}
