using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using VtcIts.Models.Api;


namespace VtcIts.Controllers.Api {


    public class MeetingController : ApiController {


        // GET api/meeting
        public IQueryable<Meeting> Get() {
            var output = new List<Meeting>();

            using (var context = new Models.VtcContext()) {
                foreach (var item in context.Meetings) {
                    output.Add(Meeting.GetMeeting(item));
                }
            }
            return output.AsQueryable();
        }


        // GET api/meeting/5
        public Meeting Get(int id) {
            using (var context = new Models.VtcContext()) {
                var result = context.Meetings.Find(id);
                if (result == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                return Meeting.GetMeeting(result);
            }
            return null;
        }


        [HttpGet]
        [ActionName("AvailableBuildings")]
        public IQueryable<Models.AvailableBuildings_Result> AvailableBuildings(int id) {
            var output = new List<Models.AvailableBuildings_Result>();
            using (var context = new Models.VtcContext()) {
                output.AddRange(context.AvailableBuildings(id));
            }
            return output.AsQueryable();
        }


        [HttpGet]
        [ActionName("AvailableRooms")]
        public IQueryable<Models.AvailableRooms_Result> AvailableRooms(int meetingId, int buildingId) {
            var output = new List<Models.AvailableRooms_Result>();
            using (var context = new Models.VtcContext()) {
                output.AddRange(context.AvailableRooms(meetingId, buildingId));
            }
            return output.AsQueryable();
        }


        [HttpGet]
        [ActionName("AvailableExLocations")]
        public IQueryable<Models.AvailableExLocations_Result> AvailableExLocations(int id) {
            var output = new List<Models.AvailableExLocations_Result>();
            using (var context = new Models.VtcContext()) {
                output.AddRange(context.AvailableExLocations(id));
            }
            return output.AsQueryable();
        }



        [HttpGet]
        [ActionName("AvailableBadgeNumbers")]
        public IQueryable<Models.AvailablePeople_Result> AvailableBadgeNumbers (int id) {
            var output = new List<Models.AvailablePeople_Result>();
            using (var context = new Models.VtcContext()) {
                output.AddRange(context.AvailablePeople(id).Where(p => !string.IsNullOrEmpty(p.BadgeNumber)));
            }
            return  output.AsQueryable();
        }



        [HttpGet]
        [ActionName("AvailableEmails")]
        public IQueryable<Models.AvailablePeople_Result> AvailableEmails (int id) {
            var output = new List<Models.AvailablePeople_Result>();
            using (var context = new Models.VtcContext()) {
                output.AddRange(context.AvailablePeople(id).Where(p => !string.IsNullOrEmpty(p.Email)));
            }
            return output.AsQueryable();
        }
        

    }

}
