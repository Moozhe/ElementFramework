using System;
using System.Collections.Generic;
using System.Text;

namespace BLUE.Mail.DAO.Imap
{
    public class Mailbox
    {
        public virtual string Name { get; internal set; }
        public virtual int NumNewMsg { get; internal set; }
        public virtual int NumMsg { get; internal set; }
        public virtual int NumUnSeen { get; internal set; }
        public virtual string[] Flags { get; internal set; }
        public virtual bool IsWritable { get; internal set; }

        public Mailbox()
            : this(string.Empty)
        {
        }
        
        public Mailbox(string name)
        {
            Name = ModifiedUtf7Encoding.Decode(name);
            Flags = new string[0];
        }

        internal void SetFlags(string flags)
        {
            Flags = flags.Split(' ');
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
