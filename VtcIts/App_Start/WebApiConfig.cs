using System.Linq;
using System.Web.Http;

namespace VtcIts {

    public static class WebApiConfig {

        public static void Register(HttpConfiguration config) {

            #region api/Meeting Special Snowflake operations
 
            config.Routes.MapHttpRoute(
                name: "MeetingApi",
                routeTemplate: "api/Meeting/{meetingId}",
                defaults: new { controller = "Meeting", action = "Get", meetingId = ""}
            );

            config.Routes.MapHttpRoute(
                name: "MeetingApiAvailableBadgeNumbers",
                routeTemplate: "api/Meeting/{id}/AvailableBadgeNumbers",
                defaults: new { controller = "Meeting", action = "AvailableBadgeNumbers", id = "" }
            );

            config.Routes.MapHttpRoute(
                name: "MeetingApiAvailableBuildings",
                routeTemplate: "api/Meeting/{id}/AvailableBuildings",
                defaults: new { controller = "Meeting", action = "AvailableBuildings", id = "" }
            );

            config.Routes.MapHttpRoute(
               name: "MeetingApiAvailableEmails",
               routeTemplate: "api/Meeting/{id}/AvailableEmails",
               defaults: new { controller = "Meeting", action = "AvailableEmails", id = "" }
            );

            config.Routes.MapHttpRoute(
                name: "MeetingApiAvailableExLocations",
                routeTemplate: "api/Meeting/{id}/AvailableExLocations",
                defaults: new { controller = "Meeting", action = "AvailableExLocations", id = "" }
            );

            config.Routes.MapHttpRoute(
                name: "MeetingApiAvailableRooms",
                routeTemplate: "api/Meeting/{meetingId}/AvailableRooms/{buildingId}",
                defaults: new { controller = "Meeting", action = "AvailableRooms", meetingId = "", buildingId = "" }
            );

            #endregion

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            config.EnableQuerySupport();

            // Sets WebApi to return JSON by default. http://stackoverflow.com/questions/9847564/how-do-i-get-asp-net-web-api-to-return-json-instead-of-xml-using-chrome
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(
                config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml")
            );
        }
    }
}