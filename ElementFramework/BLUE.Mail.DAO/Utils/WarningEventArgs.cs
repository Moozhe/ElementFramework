using System;
using System.Collections.Generic;
using System.Text;

namespace BLUE.Mail.DAO
{
    public class WarningEventArgs : EventArgs
    {
        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        private MailMessage _mailMessage;
        public MailMessage MailMessage
        {
            get
            {
                return _mailMessage;
            }
            set
            {
                _mailMessage = value;
            }
        }

        public WarningEventArgs(string message, MailMessage mailMessage)
        {
            Message = message;
            MailMessage = mailMessage;
        }
    }
}
