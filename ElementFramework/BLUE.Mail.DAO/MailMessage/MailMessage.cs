using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace BLUE.Mail.DAO
{
    public class MailMessage : HeaderedObject
    {
        #region Static
        public static implicit operator System.Net.Mail.MailMessage(MailMessage msg)
        {
            var ret = new System.Net.Mail.MailMessage();

            ret.Subject = msg.Subject;
            ret.Sender = msg.Sender;

            foreach (var a in msg.Bcc)
                ret.Bcc.Add(a);

            ret.Body = msg.Body;
            ret.IsBodyHtml = msg.ContentType.MediaType.Contains("html");
            ret.From = msg.From;
            ret.Priority = (System.Net.Mail.MailPriority)msg.Importance;

            ret.ReplyTo = Utilities.FirstOrDefault(msg.ReplyTo);

            foreach (var a in msg.To)
                ret.To.Add(a);

            foreach (var a in msg.Attachments)
                ret.Attachments.Add(new System.Net.Mail.Attachment(new System.IO.MemoryStream(a.GetData()), a.Filename, a.ContentType.MediaType));

            foreach (var a in msg.AlternateViews)
                ret.AlternateViews.Add(new System.Net.Mail.AlternateView(new System.IO.MemoryStream(a.GetData()), a.ContentType));

            return ret;
        }
        #endregion

        #region Properties
        public virtual DateTime Date { get; set; }
        public virtual string[] RawFlags { get; set; }
        public virtual Flags Flags { get; set; }

        public virtual bool HeadersOnly { get; protected set; }
        public virtual int Size { get; internal set; }
        public virtual string Subject { get; set; }
        public virtual ICollection<MailAddress> To { get; private set; }
        public virtual ICollection<MailAddress> Cc { get; private set; }
        public virtual ICollection<MailAddress> Bcc { get; private set; }
        public virtual ICollection<MailAddress> ReplyTo { get; private set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual AlternateViewCollection AlternateViews { get; set; }
        public virtual MailAddress From { get; set; }
        public virtual MailAddress Sender { get; set; }
        public virtual string MessageID { get; set; }
        public virtual string Uid { get; internal set; }
        public virtual MailPriority Importance { get; set; }
        #endregion

        #region Constructor
        public MailMessage()
        {
            RawFlags = new string[0];
            To = new Collection<MailAddress>();
            Cc = new Collection<MailAddress>();
            Bcc = new Collection<MailAddress>();
            ReplyTo = new Collection<MailAddress>();
            Attachments = new Collection<Attachment>();
            AlternateViews = new AlternateViewCollection();
        }
        #endregion

        #region Methods
        public virtual void Load(string message, bool headersOnly = false)
        {
            if (string.IsNullOrEmpty(message)) return;
            using (var mem = new MemoryStream(_defaultEncoding.GetBytes(message)))
            {
                Load(mem, headersOnly, message.Length);
            }
        }

        public virtual void Load(Stream reader, bool headersOnly = false, int maxLength = 0, char? termChar = null)
        {
            HeadersOnly = headersOnly;
            Headers = null;
            Body = null;

            var headers = new StringBuilder();
            string line;
            while ((line = Utilities.ReadLine(reader, ref maxLength, _defaultEncoding, termChar)) != null)
            {
                if (line.Trim().Length == 0)
                    if (headers.Length == 0)
                        continue;
                    else
                        break;
                headers.AppendLine(line);
            }
            RawHeaders = headers.ToString();

            if (!headersOnly)
            {
                string boundary = Headers.GetBoundary();
                if (!string.IsNullOrEmpty(boundary))
                {
                    var atts = new List<Attachment>();
                    var body = ParseMime(reader, boundary, ref maxLength, atts, Encoding, termChar);
                    if (!string.IsNullOrEmpty(body))
                        SetBody(body);

                    foreach (var att in atts)
                        (att.IsAttachment ? Attachments : AlternateViews).Add(att);

                    if (maxLength > 0)
                        Utilities.ReadToEnd(reader, maxLength, Encoding);

                }
                else
                {
                    SetBody(Utilities.ReadToEnd(reader, maxLength, Encoding));
                }
            }

            if ((Utilities.IsNullOrWhiteSpace(Body) || ContentType.MediaType.StartsWith("multipart/")) && AlternateViews.Count > 0)
            {
                var att = AlternateViews.GetTextView() ?? AlternateViews.GetHtmlView();
                if (att != null)
                {
                    Body = att.Body;
                    ContentTransferEncoding = att.Headers["Content-Transfer-Encoding"].RawValue;
                    SetContentType(att.Headers["Content-Type"].RawValue);
                }
            }

            Date = Headers.GetDate();
            To = Utilities.ToList(Headers.GetMailAddresses("To"));
            Cc = Utilities.ToList(Headers.GetMailAddresses("Cc"));
            Bcc = Utilities.ToList(Headers.GetMailAddresses("Bcc"));
            Sender = Utilities.FirstOrDefault(Utilities.ToList(Headers.GetMailAddresses("Sender")));
            ReplyTo = Utilities.ToList(Headers.GetMailAddresses("Reply-To"));
            From = Utilities.FirstOrDefault(Headers.GetMailAddresses("From"));
            MessageID = Headers["Message-ID"].RawValue;

            Importance = Headers.GetEnum<MailPriority>("Importance");
            Subject = Headers["Subject"].RawValue;
        }

        private static string ParseMime(Stream reader, string boundary, ref int maxLength, ICollection<Attachment> attachments, Encoding encoding, char? termChar)
        {
            var maxLengthSpecified = maxLength > 0;
            string data = null,
                bounderInner = "--" + boundary,
                bounderOuter = bounderInner + "--";
            var n = 0;
            var body = new System.Text.StringBuilder();
            do
            {
                if (maxLengthSpecified && maxLength <= 0)
                    return body.ToString();
                if (data != null)
                {
                    body.Append(data);
                }
                data = Utilities.ReadLine(reader, ref maxLength, encoding, termChar);
                n++;
            } while (data != null && !data.StartsWith(bounderInner));

            while (data != null && !data.StartsWith(bounderOuter) && !(maxLengthSpecified && maxLength == 0))
            {
                data = Utilities.ReadLine(reader, ref maxLength, encoding, termChar);
                if (data == null) break;
                var a = new Attachment { Encoding = encoding };

                var part = new StringBuilder();
                // read part header
                while (!data.StartsWith(bounderInner) && data != string.Empty && !(maxLengthSpecified && maxLength == 0))
                {
                    part.AppendLine(data);
                    data = Utilities.ReadLine(reader, ref maxLength, encoding, termChar);
                    if (data == null) break;
                }
                a.RawHeaders = part.ToString();
                // header body

                // check for nested part
                var nestedboundary = a.Headers.GetBoundary();
                if (!string.IsNullOrEmpty(nestedboundary))
                {
                    ParseMime(reader, nestedboundary, ref maxLength, attachments, encoding, termChar);
                    while (!data.StartsWith(bounderInner))
                        data = Utilities.ReadLine(reader, ref maxLength, encoding, termChar);
                }
                else
                {
                    data = Utilities.ReadLine(reader, ref maxLength, a.Encoding, termChar);
                    if (data == null) break;
                    var nestedBody = new StringBuilder();
                    while (!data.StartsWith(bounderInner) && !(maxLengthSpecified && maxLength == 0))
                    {
                        nestedBody.AppendLine(data);
                        data = Utilities.ReadLine(reader, ref maxLength, a.Encoding, termChar);
                    }
                    a.SetBody(nestedBody.ToString());
                    attachments.Add(a);
                }
            }
            return body.ToString();
        }

        private static Dictionary<string, int> _flagCache;
        internal void SetFlags(string flags)
        {
            if (_flagCache == null)
            {
                _flagCache = new Dictionary<string,int>(StringComparer.OrdinalIgnoreCase);

                foreach (Flags flag in Enum.GetValues(typeof(Flags)))
                    _flagCache.Add(flag.ToString(), (int)flag);
            }

            List<string> distinctFlags = new List<string>();
            foreach (var flag in flags.Split(' '))
            {
                bool alreadyExists = false;
                foreach (var str in distinctFlags)
                {
                    if (String.Equals(str, flag, StringComparison.OrdinalIgnoreCase))
                    {
                        alreadyExists = true;
                        break;
                    }
                }

                if (!alreadyExists)
                    distinctFlags.Add(flag);
            }

            RawFlags = distinctFlags.ToArray();

            int flagSum = 0;
            foreach (var f in RawFlags)
            {
                int flag = 0;
                if (_flagCache.TryGetValue(f.TrimStart('\\'), out flag))
                    flagSum += flag;
            }

            Flags = (Flags)flagSum;
        }

        public override string ToString()
        {
            return Subject ?? base.ToString();
        }
        #endregion
    }

    public enum MailPriority
    {
        Normal = 3,
        High = 5,
        Low = 1
    }

    [Flags]
    public enum Flags
    {
        None = 0,
        Seen = 1,
        Answered = 2,
        Flagged = 4,
        Deleted = 8,
        Draft = 16
    }
}
