using System.Collections.Generic;


namespace VtcIts.Models.Api {


    /// <summary>
    /// This class is used to build the JSON object consumed by fullcalendar.js
    /// </summary>
    public class Series {

        public int Id { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string RuleText { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        private Series() {}



        /// <summary>
        /// Constructor
        /// </summary>
        public static Series GetSeries(Models.Series source) {
            Series output = null;

            if (source != null) {
                output = new Series {
                    Id = source.Id,
                    StartDate = source.StartDate,
                    EndDate = source.EndDate
                };


            }

            return output;
        }


    }
}