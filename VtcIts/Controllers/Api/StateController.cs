using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using VtcIts.Models.Api;


namespace VtcIts.Controllers.Api {


    public class StateController : ApiController {


        // GET api/building
        public IQueryable<State> Get() {
            var output = new List<State>();

            using (var context = new Models.VtcContext()) {
                foreach (var item in context.Ref_State.OrderBy(b => b.Abbreviation)) {
                    output.Add(State.GetState(item));
                }
            }
            return output.AsQueryable();
        }


        // GET api/building/5
        public State Get(int id) {
            using (var context = new Models.VtcContext()) {
                var result = context.Ref_State.Find(id);
                if (result == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                return State.GetState(result);
            }
            return null;
        }

        //// POST api/building
        //public void Post([FromBody]Building value) {
        //}

        //// PUT api/building/5
        //public void Put(int id, [FromBody]Building value)
        //{
        //}

        //// DELETE api/building/5
        //public void Delete(int id)
        //{
        //}

    }

}
