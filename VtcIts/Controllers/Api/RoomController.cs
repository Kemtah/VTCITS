using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using VtcIts.Models.Api;


namespace VtcIts.Controllers.Api {


    public class RoomController : ApiController {


        // GET api/room
        public IQueryable<Room> Get() {
            var output = new List<Room>();

            using (var context = new Models.VtcContext()) {
                foreach (var item in context.Rooms.OrderBy(r => r.Name)) {
                    output.Add(Room.GetRoom(item));
                }
            }
            return output.AsQueryable();
        }


        // GET api/room/5
        public Room Get(int id) {
            using (var context = new Models.VtcContext()) {
                var result = context.Rooms.Find(id);
                if (result == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                return Room.GetRoom(result);
            }
            return null;
        }

        //// POST api/room
        //public void Post([FromBody]Room value) {
        //}

        //// PUT api/room/5
        //public void Put(int id, [FromBody]Room value)
        //{
        //}

        //// DELETE api/room/5
        //public void Delete(int id)
        //{
        //}

    }

}
