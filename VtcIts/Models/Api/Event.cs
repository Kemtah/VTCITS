using System;


namespace VtcIts.Models.Api {


    /// <summary>
    /// This class is used to build the JSON object consumed by fullcalendar.js
    /// </summary>
    public class Event {


        // ReSharper disable InconsistentNaming
        public int id { get; set; }
        public string title { get; set; }
        public bool? allDay { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string url { get; set; }
        public string className { get; set; }
        public bool editable { get; set; }
        public int siteId { get; set; }
        public int buildingId { get; set; }
        public int roomId { get; set; }
        // ReSharper restore InconsistentNaming


        /// <summary>
        /// Constructor
        /// </summary>
        private Event() {}



        /// <summary>
        /// Constructor
        /// </summary>
        public Event(Models.Meeting meeting) {
            id = meeting.Id;
            title = meeting.Title;
            allDay = false;
            start = meeting.Start;
            end = meeting.End;
            url = "";
            className = "";
            editable = true;

            //using (var context = new VTCSchedulerDb()) {
            //    //var room = context.ConferenceRooms.Find(roomId);
            //    //buildingId = context.Buildings.Find(room.BuildingId).Id;
            //    //siteId = context.Buildings.Find(room.BuildingId).SiteId;
            //}
        }


    }

}