using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.ComponentModel;

namespace BLUE.Mail.DAO
{
    public abstract class MailClient
    {
        #region Constants
        private const int DefaultEncodingPage = 1252;
        private const string AlreadyConnectedString = "Already Connected";
        private const string NotConnectedString = "Not Connected";
        private const string NotAuthenticatedString = "Not Authenticated";

        private const int BufferSize = 8192;
        private const int ConnectTimeout = 5000;
        private const int ReadTimeout = 10000;
        private const int SendTimeout = 10000;
        #endregion

        #region Events
        public event EventHandler<WarningEventArgs> Warning;
        #endregion

        #region Fields
        protected object _locker = new object();
        protected TcpClient _connection;
        protected Stream _stream;
        private byte[] _buffer = new byte[BufferSize];
        #endregion

        #region Events
        public event EventHandler StateChanged;
        #endregion

        #region Properties
        public virtual string Host { get; private set; }
        public virtual int Port { get; private set; }
        public virtual bool Ssl { get; private set; }
        public virtual bool IsDisposed { get; private set; }
        public virtual Encoding Encoding { get; set; }

        private ConnectionState _state;
        public virtual ConnectionState State
        {
            get
            {
                return _state;
            }
            private set
            {
                if (_state != value)
                {
                    _state = value;

                    if (StateChanged != null)
                        StateChanged(this, EventArgs.Empty);
                }
            }
        }
        #endregion

        #region Constructor
        public MailClient()
        {
            Encoding = Encoding.GetEncoding(DefaultEncodingPage);
        }
        #endregion

        #region Abstract Methods
        public abstract void GetMessageCount(Action<int, Exception> callback);
        public abstract void GetMessage(Action<MailMessage, Exception> callback, int index, bool headersOnly);
        public abstract void GetMessage(Action<MailMessage, Exception> callback, string uid, bool headersOnly);
        public abstract void DeleteMessage(Action<Exception> error, string uid);
        public abstract void DeleteMessage(Action<Exception> error, MailMessage msg);

        internal abstract void OnLogin(string username, string password);
        internal abstract void OnLogout();
        internal abstract void CheckResultOK(string result);
        #endregion

        #region Public Methods
        public virtual void Connect(Action<Exception> callback, string hostname, int port, bool ssl, bool skipSslValidation)
        {
            RemoteCertificateValidationCallback validationCertificate = null;

            if (skipSslValidation)
                validationCertificate = (sender, cert, chain, err) => true;

            Connect(callback, hostname, port, ssl, validationCertificate);
        }

        public virtual void Connect(Action<Exception> callback, string hostname, int port, bool ssl, RemoteCertificateValidationCallback validateCertificate)
        {
            if (State != ConnectionState.Disconnected)
                throw new InvalidOperationException(AlreadyConnectedString);

            Host = hostname;
            Port = port;
            Ssl = ssl;
            State = ConnectionState.Connecting;

            var callingThread = SynchronizationContext.Current;

            Action connectAction = () =>
            {
                TcpClient connection = new TcpClient();
                connection.SendTimeout = SendTimeout;
                connection.ReceiveTimeout = ReadTimeout;

                IAsyncResult ar = connection.BeginConnect(hostname, port, null, null);
                WaitHandle waitHandle = ar.AsyncWaitHandle;

                try
                {
                    if (!waitHandle.WaitOne(TimeSpan.FromMilliseconds(ConnectTimeout), false))
                        throw new TimeoutException();

                    connection.EndConnect(ar);
                }
                finally
                {
                    waitHandle.Close();
                }

                Stream stream = connection.GetStream();

                if (ssl)
                {
                    SslStream sslStream;

                    if (validateCertificate != null)
                        sslStream = new SslStream(stream, false, validateCertificate);
                    else
                        sslStream = new SslStream(stream, false);

                    sslStream.AuthenticateAsClient(hostname);

                    stream = sslStream;
                }

                lock (_locker)
                {
                    _connection = connection;
                    _stream = stream;
                }

                OnConnected(GetResponse());
            };

            AsyncCallback completeAction = (IAsyncResult result) =>
            {
                callingThread.Post(_ =>
                {
                    Exception ex = null;

                    try
                    {
                        connectAction.EndInvoke(result);

                        State = ConnectionState.Authorization;
                    }
                    catch (Exception exception)
                    {
                        ex = exception;

                        Cleanup();
                    }
                    finally
                    {
                        if (callback != null)
                            callback(ex);
                    }
                }, null);
            };

            connectAction.BeginInvoke(completeAction, null);
        }

        public virtual void Login(Action<Exception> callback, string username, string password)
        {
            if (State == ConnectionState.Disconnected)
                throw new Exception(NotConnectedString);

            State = ConnectionState.Authorization;

            var callingThread = SynchronizationContext.Current;

            Action loginAction = () =>
            {
                OnLogin(username, password);
            };

            AsyncCallback completeAction = (IAsyncResult result) =>
            {
                callingThread.Post(_ =>
                {
                    Exception ex = null;

                    try
                    {
                        loginAction.EndInvoke(result);

                        State = ConnectionState.Transaction;
                    }
                    catch (Exception exception)
                    {
                        ex = exception;
                    }
                    finally
                    {
                        if (callback != null)
                            callback(ex);
                    }
                }, null);
            };

            loginAction.BeginInvoke(completeAction, null);
        }

        public virtual void Logout(Action<Exception> callback)
        {
            if (State == ConnectionState.Disconnected)
                throw new Exception(NotConnectedString);

            var callingThread = SynchronizationContext.Current;

            Action logoutAction = () =>
            {
                OnLogout();
            };

            AsyncCallback completeAction = (IAsyncResult result) =>
            {
                callingThread.Post(_ =>
                {
                    Exception ex = null;

                    try
                    {
                        logoutAction.EndInvoke(result);

                        State = ConnectionState.Authorization;
                    }
                    catch (Exception exception)
                    {
                        ex = exception;
                    }
                    finally
                    {
                        if (callback != null)
                            callback(ex);
                    }
                }, null);
            };

            logoutAction.BeginInvoke(completeAction, null);
        }

        public virtual void Disconnect()
        {
            if (State == ConnectionState.Transaction)
            {
                Logout((Exception ex) =>
                {
                    Cleanup();
                });
            }
            else
            {
                Cleanup();
            }
        }
        #endregion

        #region Private Methods
        protected virtual void OnConnected(string result)
        {
            CheckResultOK(result);
        }

        protected virtual void CheckConnectionStatus()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(this.GetType().Name);
            if (State == ConnectionState.Disconnected)
                throw new Exception(NotConnectedString);
            if (State == ConnectionState.Authorization)
                throw new Exception(NotAuthenticatedString);
        }

        protected virtual void SendCommand(string command)
        {
            byte[] bytes = Encoding.Default.GetBytes(command + "\r\n");
            _stream.Write(bytes, 0, bytes.Length);
        }

        protected virtual string SendCommandGetResponse(string command)
        {
            SendCommand(command);
            return GetResponse();
        }

        protected virtual string GetResponse()
        {
            int max = 0;
            return Utilities.ReadLine(_stream, ref max, Encoding, null);
        }

        protected virtual void SendCommandCheckOK(string command)
        {
            CheckResultOK(SendCommandGetResponse(command));
        }

        protected virtual void RaiseWarning(MailMessage mailMessage, string message)
        {
            if (Warning != null)
                Warning(this, new WarningEventArgs(message, mailMessage));
        }

        protected void Cleanup()
        {
            State = ConnectionState.Disconnected;
            Utilities.TryDispose(ref _connection);
            Utilities.TryDispose(ref _stream);
        }

        protected virtual void OnDispose()
        {
        }
        #endregion

        #region IDisposable
        public virtual void Dispose()
        {
            if (IsDisposed)
                return;

            lock (this)
            {
                IsDisposed = true;
                Disconnect();

                try
                {
                    OnDispose();
                }
                catch
                {
                }

                _stream = null;
                _connection = null;
            }

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
