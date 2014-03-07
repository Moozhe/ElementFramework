using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;

namespace BLUE.Mail.DAO
{
    public class Pop3Client : MailClient
    {
        public Pop3Client()
        {
        }

        public override void GetMessageCount(Action<int, Exception> callback)
        {
            CheckConnectionStatus();

            var callingThread = SynchronizationContext.Current;
            int retVal = 0;

            Action action = () =>
            {
                string result = SendCommandGetResponse("STAT");
                CheckResultOK(result);

                retVal = Int32.Parse(result.Split(' ')[1]);
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

        public override void GetMessage(Action<MailMessage, Exception> callback, int index, bool headersOnly)
        {
            GetMessage(callback, (index + 1).ToString(), headersOnly);
        }

        private static Regex rxOctets = new Regex(@"(\d+)\s+octets", RegexOptions.IgnoreCase);
        public override void GetMessage(Action<MailMessage, Exception> callback, string uid, bool headersOnly)
        {
            CheckConnectionStatus();

            var callingThread = SynchronizationContext.Current;
            MailMessage retVal = null;

            Action action = () =>
            {
                MailMessage msg;

                lock (_stream)
                {
                    string line = SendCommandGetResponse(String.Format(headersOnly ? "TOP {0} 0" : "RETR {0}", uid));
                    int size = Convert.ToInt32(rxOctets.Match(line).Groups[1].Value);
                    CheckResultOK(line);

                    msg = new MailMessage();
                    msg.Load(_stream, headersOnly, size, '.');
                    msg.Uid = uid;
                    string last = GetResponse();
                    if (String.IsNullOrEmpty(last))
                        last = GetResponse();

                    if (last != ".")
                    {
                        Debugger.Break();
                        RaiseWarning(msg, "Expected \".\" in stream, but received \"" + last + "\"");
                    }
                }

                retVal = msg;
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

        public override void DeleteMessage(Action<Exception> error, string uid)
        {
            CheckConnectionStatus();

            var callingThread = SynchronizationContext.Current;

            Action action = () =>
            {
                SendCommandCheckOK("DELE " + uid);
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

        public virtual void DeleteMessage(Action<Exception> error, int index)
        {
            DeleteMessage(error, (index + 1).ToString());
        }

        public override void DeleteMessage(Action<Exception> error, MailMessage msg)
        {
            DeleteMessage(error, msg.Uid);
        }

        internal override void OnLogin(string username, string password)
        {
            SendCommandCheckOK("USER " + username);
            SendCommandCheckOK("PASS " + password);
        }

        internal override void OnLogout()
        {
            if (_stream != null)
            {
                SendCommand("QUIT");
            }
        }

        internal override void CheckResultOK(string result)
        {
            if (!result.StartsWith("+OK", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception(result.Substring(result.IndexOf(' ') + 1).Trim());
            }
        }
    }
}
