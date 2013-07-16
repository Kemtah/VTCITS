using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using VtcIts.Models.Api;


namespace VtcIts.Controllers.Api {


    public class SeriesController : ApiController {


        // GET api/MeetingSeries
        public IQueryable<Series> Get() {
            var output = new List<Series>();

            using (var context = new Models.VtcContext()) {
                foreach (var item in context.Series) {
                    output.Add(Series.GetSeries(item));
                }
            }
            return output.AsQueryable();
        }


        // GET api/MeetingSeries/5
        public Series Get(int id) {
            using (var context = new Models.VtcContext()) {
                var result = context.Series.Find(id);
                if (result == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
                return Series.GetSeries(result);
            }
            return null;
        }

        //// POST api/MeetingSeries
        //public void Post([FromBody]MeetingSeries value) {
        //}

        //// PUT api/MeetingSeries/5
        //public void Put(int id, [FromBody]MeetingSeries value)
        //{
        //}

        //// DELETE api/MeetingSeries/5
        //public void Delete(int id)
        //{
        //}

    }

}
