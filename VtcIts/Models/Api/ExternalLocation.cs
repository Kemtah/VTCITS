namespace VtcIts.Models.Api {
 
    
    public class ExternalLocation {
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string PointOfContact { get; set; }
        public string TechnicalContactPhone { get; set; }
        public string TechnicalContactEmail { get; set; }
        

        private ExternalLocation() { }


        public static ExternalLocation GetExternalLocation(Models.ExternalLocation source) {
            ExternalLocation output = null;

            if (source != null) {
                output = new ExternalLocation {
                    Id = source.Id,
                    Name = source.Name,
                    IpAddress = source.IpAddress,
                    PointOfContact = source.PointOfContact,
                    TechnicalContactPhone = source.TechnicalContactPhone,
                    TechnicalContactEmail = source.TechnicalContactEmail
                };
            }

            return output;
        }


    }


}
