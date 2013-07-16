namespace VtcIts.Models.Api {

    
    public class Person {

        public int Id { get; set; }
        public int MeetingId { get; set; }
        public string BadgeNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int LocationId { get; set; }
        public int? BuildingId { get; set; }
        public Location Location { get; set; }
        public Building Building { get; set; }


        private Person() { }


        public static Person GetPerson(Models.Person source) {
            Person output = null;

            if (source != null) {
                output = new Person {
                    Id = source.Id,
                    BadgeNumber = source.BadgeNumber,
                    Email = source.Email,
                    FirstName = source.FirstName,
                    LastName = source.LastName,
                    Phone = source.Phone,
                    LocationId = source.LocationId,
                    BuildingId = source.BuildingId,
                    Location = Location.GetLocation(source.Location),
                    Building = Building.GetBuilding(source.Building)
                };
            }

            return output;
        }


        public string Name {
            get { return string.Format("{0}, {1}", LastName, FirstName); }
        }

        public string LocationName {
            get { return Location.Name; }
        }

        public string BuildingName {
            get { return (Building != null) ? Building.Name : string.Empty; }
        }

    }

}
