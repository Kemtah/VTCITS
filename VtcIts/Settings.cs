using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using VtcIts.Models;


namespace VtcIts {

    public class Settings {

        private static Models.Setting SettingRecord {
            get {
                using (var context = new VtcContext()) {
                    return context.Settings.First();
                }
            }
        }


        internal static string SurveyLink {
            get { return SettingRecord.SurveyLink; }
        }

        internal static string SystemMailName {
            get { return SettingRecord.SystemMailName; }
        }

        public static bool AllowRegistration {
            get {
                return SettingRecord.RegistrationAllowed;
            }
        }

        internal static bool EnableEmail {
            get {
                return SettingRecord.EmailEnabled;
            }
        }
            

        internal static string SystemMailAddress {
            get { return SettingRecord.SystemMailAddress; }
        }
            

        internal static MailAddress SystemAddress {
            get { return new MailAddress(SystemMailAddress, SystemMailName); }
        }

        /// <summary>
        /// Gets the value of the named request parameter key, as the specified type, or get the
        /// default value
        /// </summary>
        /// <param name="key">Parameter Key</param>
        /// <param name="defaultValue">Value to return if desired value is invalid</param>
        /// <returns>A valid value of the passed type</returns>
        /// <remarks>This handles string values</remarks>
        internal static string SafeAppSetting(string key, string defaultValue = "")
        {
            return (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                       ? ConfigurationManager.AppSettings[key]
                       : defaultValue;
        }



        /// <summary>
        /// Gets the value of the named request parameter key, as the specified type, or get the
        /// default value
        /// </summary>
        /// <param name="key">Parameter Key</param>
        /// <param name="defaultValue">Value to return if desired value is invalid</param>
        /// <returns>A valid value of the passed type</returns>
        internal static T SafeAppSetting<T>(string key, T defaultValue = default(T)) where T : struct
        {
            var output = defaultValue;

            try
            {
                var type = typeof(T);

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
                {

                    if (type.BaseType != typeof(Enum))
                    {
                        var converted = Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
                        output = (converted != null) ? (T)converted : defaultValue;

                    }
                    else
                    {
                        var intValue = SafeAppSetting(key, Convert.ToInt32(defaultValue));
                        output = (Enum.IsDefined(type, intValue)) ? (T)Enum.ToObject(type, intValue) : defaultValue;

                    }
                }

            }
            catch { }

            return output;
        }


    }
}