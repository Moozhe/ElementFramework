using System;
using System.Net.Mime;
using System.Text;

namespace BLUE.Mail.DAO
{
    public abstract class HeaderedObject
    {
        public virtual string RawHeaders { get; internal set; }
        public virtual string Body { get; set; }

        private HeaderDictionary _headers;
        public virtual HeaderDictionary Headers
        {
            get
            {
                return _headers ?? (_headers = HeaderDictionary.Parse(RawHeaders, _defaultEncoding));
            }
            internal set
            {
                _headers = value;
            }
        }

        public virtual string ContentTransferEncoding
        {
            get
            {
                return Headers["Content-Transfer-Encoding"].Value ?? string.Empty;
            }
            internal set
            {
                Utilities.Set(Headers, "Content-Transfer-Encoding", new HeaderValue(value));
            }
        }

        private ContentType _contentType;
        public virtual ContentType ContentType
        {
            get
            {
                if (_contentType == null)
                {
                    var contentType = Headers["Content-Type"].Value;

                    _contentType = Utilities.IsNullOrWhiteSpace(contentType) ? new ContentType() : new ContentType(contentType);
                }
                return _contentType;
            }
        }

        public virtual string Charset
        {
            get
            {
                return Utilities.NotEmpty(Headers["Content-Transfer-Encoding"]["charset"], Headers["Content-Type"]["charset"]);
            }
        }

        protected Encoding _defaultEncoding = Encoding.GetEncoding(1252);
        protected Encoding _encoding;
        public virtual Encoding Encoding
        {
            get
            {
                return _encoding ?? (_encoding = Utilities.ParseCharsetToEncoding(Charset, _defaultEncoding));
            }
            set
            {
                _defaultEncoding = value ?? _defaultEncoding;
                if (_encoding != null) //Encoding has been initialized from the specified Charset
                    _encoding = value ?? _defaultEncoding;
            }
        }

        internal void SetContentType(string value)
        {
            Utilities.Set(Headers, "Content-Type", new HeaderValue(value));
            _contentType = null;
        }

        internal void SetBody(string value)
        {
            if (Utilities.Is(ContentTransferEncoding, "quoted-printable"))
            {
                value = Utilities.DecodeQuotedPrintable(value, Encoding);

            }
            //only decode the content if it is a text document
            else if (Utilities.Is(ContentTransferEncoding, "base64")
                     && ContentType.MediaType.StartsWith("text/", StringComparison.OrdinalIgnoreCase)
                     && Utilities.IsValidBase64String(ref value))
            {
                var data = Convert.FromBase64String(value);
                using (var mem = new System.IO.MemoryStream(data))
                using (var str = new System.IO.StreamReader(mem, Encoding))
                    value = str.ReadToEnd();

                ContentTransferEncoding = string.Empty;
            }

            Body = value;
        }
    }
}
