using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using VtcIts.Models.Api;


namespace VtcIts.Controllers.Api {


    public class EventController : ApiController {


        // GET api/event
        public IQueryable<Event> Get() {
            var output = new List<Event>();

            using (var context = new Models.VtcContext()) {
                foreach (var item in context.Meetings) {
                    output.Add(new Event(item));
                }
            }
            return output.AsQueryable();
        }


        // GET api/event/5
        public Event Get(int id) {
            using (var context = new Models.VtcContext()) {
                var result = context.Meetings.Find(id);
                if (result == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                return new Event(result);
            }
            return null;
        }

        //// POST api/event
        //public void Post([FromBody]Event value) {
        //}

        //// PUT api/event/5
        //public void Put(int id, [FromBody]Event value)
        //{
        //}

        //// DELETE api/event/5
        //public void Delete(int id)
        //{
        //}

    }

}
