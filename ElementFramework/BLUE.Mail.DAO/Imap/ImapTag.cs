using System;
using System.Collections.Generic;
using System.Text;

namespace BLUE.Mail.DAO.Imap
{
    public class ImapTag
    {
        private int _i = 0;

        public static implicit operator string(ImapTag tag)
        {
            return tag.ToString();
        }

        public string GetNext()
        {
            _i++;
            return this.ToString();
        }

        public ImapTag()
        {
        }

        public override string ToString()
        {
            return String.Format("xm{0:000} ", _i);
        }
    }
}
