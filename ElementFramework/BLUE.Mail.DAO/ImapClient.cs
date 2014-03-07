using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using BLUE.Mail.DAO.Imap;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Specialized;
using System.Diagnostics;

namespace BLUE.Mail.DAO
{
    public class ImapClient : MailClient
    {
        #region Fields
        private ImapTag _tag = new ImapTag();
        private string[] _capability;
        private bool _idling;
        private Thread _idleEvents;
        private string _fetchHeaders;
        #endregion

        #region Properties
        private string _selectedMailbox;
        public virtual string SelectedMailbox
        {
            get
            {
                return _selectedMailbox;
            }
        }

        public virtual AuthMethods AuthMethod { get; set; }
        #endregion

        #region Constructor
        public ImapClient()
        {
            AuthMethod = AuthMethods.Login;
        }
        #endregion

        #region Public Methods
        public override void GetMessageCount(Action<int, Exception> callback)
        {
            GetMessageCount(callback, null);
        }

        public virtual void GetMessageCount(Action<int, Exception> callback, string mailbox)
        {
            CheckConnectionStatus();

            var callingThread = SynchronizationContext.Current;
            int retVal = 0;

            Action action = () =>
            {
                lock (_locker)
                {
                    IdlePause();

                    try
                    {
                        CheckMailboxSelected();
                        retVal = GetMessageCount(mailbox);
                    }
                    finally
                    {
                        IdleResume();
                    }
                }
            };

            AsyncCallback completeAction = (IAsyncResult result) =>
            {
                callingThread.Post(_ =>
                {
                    Exception ex = null;

                    try
                    {
                        action.EndInvoke(result);
                    }
                    catch (Exception exception)
                    {
                        ex = exception;
                    }
                    finally
                    {
                        if (callback != null)
                            callback(retVal, ex);
                    }
                }, null);
            };

            action.BeginInvoke(completeAction, null);
        }

        public override void GetMessage(Action<MailMessage, Exception> callback, string uid, bool headersOnly = false)
        {
            GetMessage(callback, uid, headersOnly, true);
        }

        public override void GetMessage(Action<MailMessage, Exception> callback, int index, bool headersOnly = false)
        {
            GetMessage(callback, index, headersOnly, true);
        }

        public virtual void GetMessage(Action<MailMessage, Exception> callback, string uid, bool headersOnly = false, bool setSeen = true)
        {
            GetMessage(callback, uid, true, headersOnly, setSeen);
        }

        public virtual void GetMessage(Action<MailMessage, Exception> callback, int index, bool headersOnly = false, bool setSeen = true)
        {
            GetMessage(callback, (index + 1).ToString(), false, headersOnly, setSeen);
        }

        public virtual void GetMessage(Action<MailMessage, Exception> callback, string id, bool uid, bool headersOnly = false, bool setSeen = true)
        {
            CheckConnectionStatus();

            var callingThread = SynchronizationContext.Current;
            MailMessage retVal = null;

            Action action = () =>
            {
                lock (_locker)
                {
                    IdlePause();

                    try
                    {
                        retVal = Utilities.FirstOrDefault(GetMessages(id, id, uid, headersOnly, setSeen));
                    }
                    finally
                    {
                        IdleResume();
                    }
                }
            };

            AsyncCallback completeAction = (IAsyncResult result) =>
            {
                callingThread.Post(_ =>
                {
                    Exception ex = null;

                    try
                    {
                        action.EndInvoke(result);
                    }
                    catch (Exception exception)
                    {
                        ex = exception;
                    }
                    finally
                    {
                        if (callback != null)
                            callback(retVal, ex);
                    }
                }, null);
            };

            action.BeginInvoke(completeAction, null);
        }

        public virtual void GetMessages(Action<MailMessage[], Exception> callback, string startUID, string endUID, bool headersOnly = true, bool setSeen = false)
        {
            GetMessages(callback, startUID, endUID, true, headersOnly, setSeen);
        }

        public virtual void GetMessages(Action<MailMessage[], Exception> callback, int startIndex, int endIndex, bool headersOnly = true, bool setSeen = false)
        {
            GetMessages(callback, (startIndex + 1).ToString(), (endIndex + 1).ToString(), false, headersOnly, setSeen);
        }

        public virtual void GetMessages(Action<MailMessage[], Exception> callback, string start, string end, bool uid, bool headersOnly = true, bool setSeen = false)
        {
            CheckConnectionStatus();

            var callingThread = SynchronizationContext.Current;
            MailMessage[] retVal = null;

            Action action = () =>
            {
                lock (_locker)
                {
                    IdlePause();

                    try
                    {
                        retVal = GetMessages(start, end, uid, headersOnly, setSeen);
                    }
                    finally
                    {
                        IdleResume();
                    }
                };
            };

            AsyncCallback completeAction = (IAsyncResult result) =>
            {
                callingThread.Post(_ =>
                {
                    Exception ex = null;

                    try
                    {
                        action.EndInvoke(result);
                    }
                    catch (Exception exception)
                    {
                        ex = exception;
                    }
                    finally
                    {
                        if (callback != null)
                            callback(retVal, ex);
                    }
                }, null);
            };

            action.BeginInvoke(completeAction, null);
        }

        public override void DeleteMessage(Action<Exception> error, MailMessage msg)
        {
            DeleteMessage(error, msg.Uid);
        }

        public override void DeleteMessage(Action<Exception> error, string uid)
        {
            CheckConnectionStatus();

            var callingThread = SynchronizationContext.Current;

            Action action = () =>
            {
                lock (_locker)
                {
                    IdlePause();

                    try
                    {
                        CheckMailboxSelected();
                        Store("UID " + uid, true, "\\Seen \\Deleted");
                    }
                    finally
                    {
                        IdleResume();
                    }
                }
            };

            AsyncCallback completeAction = (IAsyncResult result) =>
            {
                callingThread.Post(_ =>
                {
                    try
                    {
                        action.EndInvoke(result);
                    }
                    catch (Exception ex)
                    {
                        if (error != null)
                            error(ex);
                    }
                }, null);
            };

            action.BeginInvoke(completeAction, null);
        }

        public virtual void SelectMailbox(Action<Mailbox, Exception> callback, string mailbox)
        {
            CheckConnectionStatus();

            var callingThread = SynchronizationContext.Current;
            Mailbox retVal = null;

            Action action = () =>
            {
                lock (_locker)
                {
                    IdlePause();

                    try
                    {
                        retVal = SelectMailbox(mailbox);
                    }
                    finally
                    {
                        IdleResume();
                    }
                }
            };

            AsyncCallback completeAction = (IAsyncResult result) =>
            {
                callingThread.Post(_ =>
                {
                    Exception ex = null;

                    try
                    {
                        action.EndInvoke(result);
                    }
                    catch (Exception exception)
                    {
                        ex = exception;
                    }
                    finally
                    {
                        if (callback != null)
                            callback(retVal, ex);
                    }
                }, null);
            };

            action.BeginInvoke(completeAction, null);
        }
        #endregion

        #region Private Methods
        internal override void OnLogin(string username, string password)
        {
            string command = String.Empty;
            string result = String.Empty;
            string tag = _tag.GetNext();
            string key;

            switch (AuthMethod)
            {
                case AuthMethods.CRAMMD5:
                    command = tag + "AUTHENTICATE CRAM-MD5";
                    result = SendCommandGetResponse(command);
                    CheckResultOK(result);

                    // retrieve server key
                    key = result.Replace("+ ", "");
                    key = Encoding.Default.GetString(Convert.FromBase64String(key));

                    // calculate hash
                    using (var md5 = new HMACMD5(Encoding.ASCII.GetBytes(password)))
                    {
                        byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(key));
                        // Convert to hex string
                        key = BitConverter.ToString(hash).ToLower().Replace("-", "");
                        result = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + " " + key));
                        result = SendCommandGetResponse(result);
                    }
                    break;

                case AuthMethods.Login:
                    command = tag + "LOGIN " + Utilities.QuoteString(username) + " " + Utilities.QuoteString(password);
                    result = SendCommandGetResponse(command);
                    break;

                case AuthMethods.SaslOAuth:
                    string sasl = "user=" + username + "\x01" + "auth=Bearer " + password + "\x01" + "\x01";
                    string base64 = Convert.ToBase64String(Encoding.GetBytes(sasl));
                    command = tag + "AUTHENTICATE XOAUTH2 " + base64;
                    result = SendCommandGetResponse(command);
                    break;

                default:
                    throw new NotSupportedException();
            }

            if (result.StartsWith("* CAPABILITY "))
            {
                _capability = result.Substring(13).Trim().Split(' ');
                result = GetResponse();
            }

            if (!result.StartsWith(tag + "OK"))
            {
                if (result.StartsWith("+ ") && result.EndsWith("=="))
                {
                    string jsonErr = Utilities.DecodeBase64(result.Substring(2), Encoding.UTF7);
                    throw new Exception(jsonErr);
                }
                else
                {
                    throw new Exception(result);
                }
            }

            if (Supports("X-GM-EXT-1"))
                _fetchHeaders = "X-GM-MSGID X-GM-THRID X-GM-LABELS ";
        }

        internal override void OnLogout()
        {
            if (State == ConnectionState.Transaction)
                SendCommand(_tag.GetNext() + "LOGOUT");
        }

        internal override void CheckResultOK(string response)
        {
            if (!IsResultOK(response))
            {
                response = response.Substring(response.IndexOf(" ")).Trim();
                throw new Exception(response);
            }
        }

        internal bool IsResultOK(string response)
        {
            response = response.Substring(response.IndexOf("*") + 1).Trim();
            return response.ToUpper().StartsWith("OK");
        }

        protected int GetMessageCount(string mailbox)
        {
            string command = _tag.GetNext() + "STATUS " + Utilities.QuoteString(ModifiedUtf7Encoding.Encode(mailbox) ?? _selectedMailbox) + " (MESSAGES)";
            string response = SendCommandGetResponse(command);
            string reg = @"\* STATUS.*MESSAGES (\d+)";
            int result = 0;

            while (response.StartsWith("*"))
            {
                Match m = Regex.Match(response, reg);
                if (m.Groups.Count > 1)
                    result = Convert.ToInt32(m.Groups[1].ToString());
                response = GetResponse();
                m = Regex.Match(response, reg);
            }

            return result;
        }

        protected virtual MailMessage[] GetMessages(string start, string end, bool uid, bool headersOnly, bool setSeen)
        {
            var x = new List<MailMessage>();

            GetMessages(start, end, uid, headersOnly, setSeen,
                (stream, size, imapHeaders) =>
                {
                    var mail = new MailMessage { Encoding = Encoding };
                    mail.Size = size;

                    if (imapHeaders["UID"] != null)
                        mail.Uid = imapHeaders["UID"];

                    if (imapHeaders["Flags"] != null)
                        mail.SetFlags(imapHeaders["Flags"]);

                    mail.Load(stream, headersOnly, mail.Size);

                    string[] except = new string[] { "UID", "Flags", "BODY[]", "BODY[HEADER]" };
                    foreach (string key in imapHeaders.AllKeys)
                    {
                        bool exclude = false;

                        foreach (string other in except)
                        {
                            if (key.Equals(other, StringComparison.OrdinalIgnoreCase))
                            {
                                exclude = true;
                                break;
                            }
                        }

                        if (!exclude)
                            mail.Headers.Add(key, new HeaderValue(imapHeaders[key]));
                    }

                    x.Add(mail);

                    return null;
                });

            return x.ToArray();
        }

        protected virtual void GetMessages(string start, string end, bool uid, bool headersOnly, bool setSeen, Func<Stream, int, NameValueCollection, MailMessage> action)
        {
            CheckMailboxSelected();

            string tag = _tag.GetNext();

            string command = tag + (uid ? "UID " : null)
                                 + "FETCH " + start + ":" + end + " ("
                                 + _fetchHeaders + "UID FLAGS BODY"
                                 + (setSeen ? null : ".PEEK")
                                 + "[" + (headersOnly ? "HEADER" : null) + "])";

            string response;

            SendCommand(command);

            while (true)
            {
                response = GetResponse();

                if (String.IsNullOrEmpty(response) || response.Contains(tag + "OK"))
                    break;

                if (response[0] != '*' || !response.Contains("FETCH ("))
                    continue;

                var imapheaders = Utilities.ParseImapHeader(response.Substring(response.IndexOf('(') + 1));
                var size = Utilities.ToInt((imapheaders["BODY[HEADER]"] ?? imapheaders["BODY[]"]).Trim('{', '}'));
                var msg = action(_stream, size, imapheaders);

                response = GetResponse();

                var n = Utilities.LastOrDefault(response.Trim());
                if (n != ')')
                {
                    Debugger.Break();
                    RaiseWarning(null, "Expected \")\" in stream, but received \"" + response + "\"");
                }
            }
        }

        protected Mailbox SelectMailbox(string mailbox)
        {
            mailbox = ModifiedUtf7Encoding.Encode(mailbox);
            Mailbox x = null;
            string tag = _tag.GetNext();
            string command = tag + "SELECT " + Utilities.QuoteString(mailbox);
            string response = SendCommandGetResponse(command);

            if (response.StartsWith("*"))
            {
                x = new Mailbox(mailbox);

                while (response.StartsWith("*"))
                {
                    Match m;

                    m = Regex.Match(response, @"(\d+) EXISTS");
                    if (m.Groups.Count > 1)
                        x.NumMsg = Convert.ToInt32(m.Groups[1].ToString());

                    m = Regex.Match(response, @"(\d+) RECENT");
                    if (m.Groups.Count > 1)
                        x.NumNewMsg = Convert.ToInt32(m.Groups[1].ToString());

                    m = Regex.Match(response, @"UNSEEN (\d+)");
                    if (m.Groups.Count > 1)
                        x.NumUnSeen = Convert.ToInt32(m.Groups[1].ToString());

                    m = Regex.Match(response, @" FLAGS \((.*?)\)");
                    if (m.Groups.Count > 1)
                        x.SetFlags(m.Groups[1].ToString());

                    response = GetResponse();
                }

                if (IsResultOK(response))
                {
                    x.IsWritable = Regex.IsMatch(response, "READ.WRITE", RegexOptions.IgnoreCase);
                }

                _selectedMailbox = mailbox;
            }
            else
            {
                throw new Exception(response);
            }

            return x;
        }

        protected virtual bool Supports(string command)
        {
            if (_capability == null)
                Capability();

            foreach (var cap in _capability)
            {
                if (String.Equals(cap, command, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        protected virtual string[] Capability()
        {
            string command = _tag.GetNext() + "CAPABILITY";
            string response = SendCommandGetResponse(command);
            if (response.StartsWith("* CAPABILITY "))
                response = response.Substring(13);
            _capability = response.Trim().Split(' ');
            GetResponse();
            return _capability;
        }

        protected virtual void CheckMailboxSelected()
        {
            if (String.IsNullOrEmpty(_selectedMailbox))
                SelectMailbox("INBOX");
        }

        protected virtual void Store(string messageset, bool replace, string flags)
        {
            CheckMailboxSelected();

            string prefix = null;
            if (messageset.StartsWith("UID ", StringComparison.OrdinalIgnoreCase))
            {
                messageset = messageset.Substring(4);
                prefix = "UID ";
            }

            string command = String.Concat(_tag.GetNext(), prefix, "STORE ", messageset, " ", replace ? "" : "+", "FLAGS.SILENT (" + flags + ")");
            string response = SendCommandGetResponse(command);

            while (response.StartsWith("*"))
                response = GetResponse();

            CheckResultOK(response);
        }
        #endregion

        #region Idle Thread
        protected virtual void IdleStart()
        {
        }

        protected virtual void IdlePause()
        {
        }

        protected virtual void IdleResume()
        {
        }

        private void IdleResumeCommand()
        {
            SendCommandGetResponse(_tag.GetNext() + "IDLE");
            //_IdleARE.Set();
        }
        #endregion

        public enum AuthMethods
        {
            Login,
            CRAMMD5,
            SaslOAuth
        }
    }
}
