namespace VtcIts.Models.Api {

    
    public class Participant {

        public int Id { get; set; }
        public int MeetingId { get; set; }
        public string BadgeNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int LocationId { get; set; }
        public int? BuildingId { get; set; }
        public bool Attended { get; set; }
        public int TravelAvoided { get; set; }
        public int Type { get; set; }
        public Location Location { get; set; }
        public Building Building { get; set; }


        private Participant() { }


        public static Participant GetParticipant(Models.MeetingParticipant source) {
            Participant output = null;

            if (source != null) {
                output = new Participant {
                    Id = source.Person.Id,
                    MeetingId = source.MeetingId,
                    BadgeNumber = source.Person.BadgeNumber,
                    Email = source.Person.Email,
                    FirstName = source.Person.FirstName,
                    LastName = source.Person.LastName,
                    Phone = source.Person.Phone,
                    LocationId = source.Person.LocationId,
                    BuildingId = source.Person.BuildingId,
                    Attended = source.Attended,
                    TravelAvoided = (int)source.TravelAvoided,
                    Type = (int)source.Type,
                    Location = Location.GetLocation(source.Person.Location),
                    Building = Building.GetBuilding(source.Person.Building)
                };
            }

            return output;
        }


    }

}
