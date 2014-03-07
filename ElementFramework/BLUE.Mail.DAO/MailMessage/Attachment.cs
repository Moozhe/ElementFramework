using System;
using System.IO;
using System.Net.Mime;

namespace BLUE.Mail.DAO
{
    public class Attachment : HeaderedObject
    {
        public virtual string Filename
        {
            get
            {
                return Utilities.NotEmpty(
                    Headers["Content-Disposition"]["filename"], 
                    Headers["Content-Disposition"]["name"],
                    Headers["Content-Type"]["filename"],
                    Headers["Content-Type"]["name"]);
            }
        }

        private ContentDisposition _contentDisposition;
        public ContentDisposition ContentDisposition
        {
            get
            {
                if (_contentDisposition == null)
                {
                    var value = Utilities.RemoveDiacritics(Headers["Content-Disposition"].RawValue);

                    _contentDisposition = string.IsNullOrEmpty(value)
                        ? new ContentDisposition { Inline = true }
                        : new ContentDisposition(value);
                }
                return _contentDisposition;
            }
        }

        public virtual bool OnServer { get; internal set; }

        internal bool IsAttachment
        {
            get
            {
                return ContentDisposition.DispositionType == DispositionTypeNames.Attachment || !string.IsNullOrEmpty(Filename);
            }
        }

        public virtual void Save(string filename)
        {
            using (var file = new FileStream(filename, FileMode.Create))
                Save(file);
        }

        public virtual void Save(Stream stream)
        {
            var data = GetData();
            stream.Write(data, 0, data.Length);
        }

        public virtual byte[] GetData()
        {
            byte[] data;
            var body = Body;

            if (Utilities.Is(ContentTransferEncoding, "base64") && Utilities.IsValidBase64String(ref body))
            {
                try
                {
                    data = Convert.FromBase64String(body);
                }
                catch (Exception)
                {
                    data = Encoding.GetBytes(body);
                }
            }
            else
            {
                data = Encoding.GetBytes(body);
            }

            return data;
        }
    }
}