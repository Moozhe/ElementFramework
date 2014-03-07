using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BLUE.Mail.DAO
{
    public class AlternateViewCollection : Collection<Attachment>
    {
        /// <summary>
        /// Find views matching a specific content-type.
        /// </summary>
        /// <param name="contentType">The content-type to search for; such as "text/html"</param>
        public Attachment OfType(string contentType)
        {
            contentType = (contentType ?? string.Empty).ToLower();
            return OfType(x => Utilities.Is(x, contentType));
        }

        /// <summary>
        /// Find views where the content-type matches a condition
        /// </summary>
        public Attachment OfType(Func<string, bool> predicate)
        {
            foreach (var attachment in this)
            {
                if (predicate(attachment.ContentType.MediaType))
                    return attachment;
            }

            return null;
        }

        public Attachment GetHtmlView()
        {
            return OfType("text/html") ?? OfType(ct => ct.Contains("html"));
        }

        public Attachment GetTextView()
        {
            return OfType("text/plain") ?? OfType(ct => ct.StartsWith("text/"));
        }
    }
}