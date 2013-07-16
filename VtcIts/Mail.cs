using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;


namespace VtcIts {

[Serializable()]
    public class Mail {


        /// <summary>
        /// Sends an e-mail message
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static void SendMail(string from, string to, string subject, string body) {
            SendMail(new MailMessage(from, to, subject, body));
        }



        /// <summary>
        /// Sends an e-mail message
        /// </summary>
        /// <param name="message">Message to send</param>
        public static void SendMail(MailMessage message) {
            SmtpClient sender;

            if (string.IsNullOrEmpty(SmtpAddress)) {
                throw new IpabsMailException("No SMTP Address provided", new NullReferenceException(), message);
            }

            try {
                sender = new SmtpClient(SmtpAddress, SmtpPort);
                sender.UseDefaultCredentials = true;
            } catch (Exception ex) {
                throw new IpabsMailException("Error instantiating SMTP Client", ex, message);
            }

            try {
                sender.Send(message);
            } catch (Exception ex) {
                throw new IpabsMailException("Error sending e-mail", ex, message);
            }

        }


        public static string PrintMessage(MailMessage message) {
            var sb = new StringBuilder();

            var toRecipients = message.To.Aggregate(string.Empty, (current, to) => current + (to.ToString() + ", "));
            var bccRecipients = message.Bcc.Aggregate(string.Empty, (current, to) => current + (to.ToString() + ", "));

            if (toRecipients.Length > 0) {
                toRecipients = toRecipients.Substring(0, toRecipients.Length - 2);
            }

            if (bccRecipients.Length > 0) {
                bccRecipients = bccRecipients.Substring(0, bccRecipients.Length - 2);
            }

            sb.Append("Subject:\t" + message.Subject);
            sb.Append("From:\t" + message.From.ToString());
            sb.Append("To:\t" + toRecipients + "\n");
            sb.Append("BCC:\t" + bccRecipients + "\n");
            sb.Append(message.Body);

            return sb.ToString();
        }
        
        /// <summary>
        /// Gets address of the SMTP Server to use to send e-mail
        /// </summary>
        private static string SmtpAddress {
            get { return Settings.SafeAppSetting("SmtpAddress"); }
        }


        /// <summary>
        /// Gets the port of the SMTP Server to use to send e-mail
        /// </summary>
        private static int SmtpPort {
            get { return Settings.SafeAppSetting<int>("SmtpPort", 25); }
        }




        /// <summary>
        /// Contains information about errors encountered while sending e-mail.
        /// </summary>
        [Serializable()]
        public class IpabsMailException : Exception {


            private const string MESSAGE_BODY_KEY = "Message Body";
            private const string MESSAGE_FROM_KEY = "Message Sender";
            private const string MESSAGE_SUBJECT_KEY = "Message Subject";
            private const string MESSAGE_TO_KEY = "Message Recipient";
            private const string SMTP_ADDRESS_KEY = "SMTP Address";
            private const string SMTP_PORT_KEY = "SMTP KEY";


            /// <summary>
            /// Gets the name of the Data File being used
            /// </summary>
            public string SmtpAddress {
                get { return (string)Data[SMTP_ADDRESS_KEY]; }
            }



            /// <summary>
            /// Gets the name of the Data File being used
            /// </summary>
            public int SmtpPort {
                get { return (int)Data[SMTP_PORT_KEY]; }
            }



            /// <summary>
            /// Gets the name of the Data File being used
            /// </summary>
            public string MessageSender {
                get { return (string)Data[MESSAGE_FROM_KEY]; }
            }



            /// <summary>
            /// Gets the name of the Data File being used
            /// </summary>
            public string MessageRecipient {
                get { return (string)Data[MESSAGE_TO_KEY]; }
            }



            /// <summary>
            /// Gets the name of the Data File being used
            /// </summary>
            public string MessageSubject {
                get { return (string)Data[MESSAGE_SUBJECT_KEY]; }
            }



            /// <summary>
            /// Gets the name of the Data File being used
            /// </summary>
            public string MessageBody {
                get { return (string)Data[MESSAGE_BODY_KEY]; }
            }



            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="message">Message</param>
            /// <param name="innerException">Exception causing the message</param>
            /// <param name="email">Email sent</param>
            public IpabsMailException(string message, Exception innerException, MailMessage email)
                : base(message, innerException) {

                Data.Add(SMTP_ADDRESS_KEY, Mail.SmtpAddress);
                Data.Add(SMTP_PORT_KEY, Mail.SmtpPort);
                Data.Add(MESSAGE_FROM_KEY, email.From.Address);
                Data.Add(MESSAGE_TO_KEY, BuildCsv(email.To));
                Data.Add(MESSAGE_SUBJECT_KEY, email.Subject);
                Data.Add(MESSAGE_BODY_KEY, email.Body);
            }



            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="message">Message</param>
            /// <param name="innerException">Exception causing the message</param>
            /// <param name="sender">Sender of the e-mail</param>
            /// <param name="recipient">Intended recipient of the e-mail</param>
            /// <param name="subject">Subject of the e-mail</param>
            /// <param name="body">Body of the e-mail</param>
            public IpabsMailException(string message, Exception innerException, string sender, string recipient, string subject, string body)
                : base(message, innerException) {

                Data.Add(SMTP_ADDRESS_KEY, Mail.SmtpAddress);
                Data.Add(SMTP_PORT_KEY, Mail.SmtpPort);
                Data.Add(MESSAGE_FROM_KEY, sender);
                Data.Add(MESSAGE_TO_KEY, recipient);
                Data.Add(MESSAGE_SUBJECT_KEY, subject);
                Data.Add(MESSAGE_BODY_KEY, body);
            }



            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="message">Message</param>
            /// <param name="innerException">Exception causing the message</param>
            /// <param name="smtpAddress">SMTP Address used</param>
            /// <param name="port">SMTP Port used</param>
            /// <param name="email">Email sent</param>
            public IpabsMailException(string message, Exception innerException, string smtpAddress, int port, MailMessage email)
                : base(message, innerException) {

                Data.Add(SMTP_ADDRESS_KEY, smtpAddress);
                Data.Add(SMTP_PORT_KEY, port);
                Data.Add(MESSAGE_FROM_KEY, email.From.Address);
                Data.Add(MESSAGE_TO_KEY, BuildCsv(email.To));
                Data.Add(MESSAGE_SUBJECT_KEY, email.Subject);
                Data.Add(MESSAGE_BODY_KEY, email.Body);
            }



            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="message">Message</param>
            /// <param name="innerException">Exception causing the message</param>
            /// <param name="smtpAddress">SMTP Address used</param>
            /// <param name="port">SMTP Port used</param>
            /// <param name="sender">Sender of the e-mail</param>
            /// <param name="recipient">Intended recipient of the e-mail</param>
            /// <param name="subject">Subject of the e-mail</param>
            /// <param name="body">Body of the e-mail</param>
            public IpabsMailException(string message, Exception innerException, string smtpAddress, int port, string sender, string recipient, string subject, string body)
                : base(message, innerException) {

                Data.Add(SMTP_ADDRESS_KEY, smtpAddress);
                Data.Add(SMTP_PORT_KEY, port);
                Data.Add(MESSAGE_FROM_KEY, sender);
                Data.Add(MESSAGE_TO_KEY, recipient);
                Data.Add(MESSAGE_SUBJECT_KEY, subject);
                Data.Add(MESSAGE_BODY_KEY, body);
            }



            /// <summary>
            /// Construction
            /// </summary>
            /// <param name="info">Serialization information</param>
            /// <param name="context">Streaming context information</param>
            public IpabsMailException(SerializationInfo info, StreamingContext context) : base(info, context) { }



            /// <summary>
            /// Converts a collection of MailAddresses to a CSV string
            /// </summary>
            /// <param name="addresses">Addresses to turn to a string</param>
            /// <returns></returns>
            private string BuildCsv(IEnumerable<MailAddress> addresses) {
                var output = addresses.Aggregate(string.Empty, (current, address) => current + (address.Address + ", "));

                if (output.Length > 2) {
                    output = output.Substring(0, output.Length - 2);
                }

                return output;
            }


        }


    }


} 