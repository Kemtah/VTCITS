namespace VtcIts.Models.Api {
    
    
    public class KemtahTech {
    
        public int Id { get; set; }
        public string KemtahId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        private KemtahTech() { }


        public static KemtahTech GetKemtahTech(Models.KemtahTech source) {
            KemtahTech output = null;

            if (source != null) {
                output = new KemtahTech {
                    Id = source.Id,
                    KemtahId = source.KemtahId,
                    Email = source.Email,
                    FirstName = source.FirstName,
                    LastName = source.LastName
                };
            }

            return output;
        }
    
    }

}
