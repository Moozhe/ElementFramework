using System;
using System.Collections;

namespace BLUE.Mail.DAO.Imap
{
    public class Quota
    {
        private string _resource;
        private string _usage;
        private int _used;
        private int _max;

        public virtual int Used
        {
            get { return _used; }
        }

        public virtual int Max
        {
            get { return _max; }
        }

        public Quota(string resourceName, string usage, int used, int max)
        {
            _resource = resourceName;
            _usage = usage;
            _used = used;
            _max = max;
        }

    }
}