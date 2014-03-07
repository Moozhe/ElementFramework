using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace BLUE.Mail.DAO.Imap
{
    public class Namespaces
    {
        private Collection<Namespace> _serverNamespace = new Collection<Namespace>();
        private Collection<Namespace> _userNamespace = new Collection<Namespace>();
        private Collection<Namespace> _sharedNamespace = new Collection<Namespace>();

        public virtual Collection<Namespace> ServerNamespace
        {
            get { return _serverNamespace; }
        }

        public virtual Collection<Namespace> UserNamespace
        {
            get { return _userNamespace; }
        }

        public virtual Collection<Namespace> SharedNamespace
        {
            get { return _sharedNamespace; }
        }
    }

    public class Namespace
    {
        public virtual string Prefix { get; internal set; }
        public virtual string Delimiter { get; internal set; }

        public Namespace()
        {
        }

        public Namespace(string prefix, string delimiter)
        {
            Prefix = prefix;
            Delimiter = delimiter;
        }
    }
}