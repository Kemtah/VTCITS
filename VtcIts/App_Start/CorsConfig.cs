using System.Web.Http;
using Thinktecture.IdentityModel.Http.Cors.WebApi;


namespace VtcIts {

    public class CorsConfig {

        public static void RegisterCors(HttpConfiguration httpConfig) {

            var corsConfig = new WebApiCorsConfiguration();

            // this adds the CorsMessageHandler to the HttpConfiguration’s
            // MessageHandlers collection
            corsConfig.RegisterGlobal(httpConfig);
          
            corsConfig
                .ForAllResources()
                .AllowAllOrigins()
                .AllowAllMethods()
                .AllowAll();
        }

    }

}