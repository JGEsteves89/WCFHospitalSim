using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

using Mergecom;
using Mergecom.Logging;

namespace ADIU
{
    public class Ssl : MCsecureContext
    {
        /* timeouts are in msecs */
        public static int SEND_TIMEOUT = 15000;
        public static int SEND_BUFFER_SIZE = 131400;

        public static int RECEIVE_TIMEOUT = 15000;
        public static int RECEIVE_BUFFER_SIZE = 131400;

        object Sync = new object();
        Dictionary<IntPtr, SslState> connections = new Dictionary<IntPtr, SslState>();
        X509Certificate2 certificate;

        /// <summary>Asynchronous Ssl state</summary>
        public class SslState
        {
            public class Operation
            {
                public static readonly int TIMEOUT = 20; // milliseconds
                public static readonly int INIT = 0;
                public static readonly int PROGRESS = 1;
                public static readonly int COMPLETE = 2;
                public static readonly int ERROR = -1;

                public object Sync = new object();
                public ManualResetEvent Event;
                public int Op;
                public String name;

                public Operation(String name)
                {
                    this.name = name;
                    Event = new ManualResetEvent(false);
                    Op = INIT;
                }

                public override String ToString()
                {
                    String str = "INIT";

                    if (Op == COMPLETE) str = "COMPLETE";
                    else if (Op == PROGRESS) str = "PROGRESS";
                    else if (Op == ERROR) str = "ERROR";

                    return String.Format(" {0}:{1} ", name, str);
                }
            };

            public readonly Operation Authenticate = new Operation("AUTH");
            public readonly Operation Read = new Operation("READ");
            public readonly Operation Write = new Operation("WRITE");
            public readonly Operation Shutdown = new Operation("SHUTDOWN");

            public SslStream ssl;
            public SecureContext context;
            public IntPtr contextHandle;
            public int Type;
            public int bytes;

            WeakReference wr;
            public TcpClient Tcp
            {
                get
                {
                    TcpClient tcp = null;
                    if (wr != null)
                    {
                        tcp = (wr.IsAlive) ? wr.Target as TcpClient : null;
                    }
                    return tcp;
                }
                set
                {
                    wr = (value != null) ? new WeakReference(value) : null;
                }
            }

            public SslState() { }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(Authenticate.ToString()).Append(Read.ToString()).Append(Write.ToString());

                return String.Format("SslState: {0}{1}", this.GetHashCode().ToString("X"), sb.ToString());
            }
        }

        /// <summary>Application SSL context</summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class AppSslContext
        {
            /// <summary/>
            [MarshalAs(UnmanagedType.LPStr)]
            public String Certificate;
            /// <summary/>
            [MarshalAs(UnmanagedType.LPStr)]
            public String Password;
        }

        /// <summary>SSL context</summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class SecureContext
        {
            /// <summary/>
            public IntPtr Socket;
            /// <summary/>
            [MarshalAs(UnmanagedType.LPStr)]
            public String HashAlgorithm;
            /// <summary/>
            [MarshalAs(UnmanagedType.LPStr)]
            public String CipherAlgorithm;
            /// <summary/>
            [MarshalAs(UnmanagedType.LPStr)]
            public String KeyExchangeAlgorithm;
            /// <summary/>
            [MarshalAs(UnmanagedType.LPStr)]
            public String SslProtocol;
        }

        static Ssl()
        {
            RECEIVE_TIMEOUT = MCconfig.getIntConfigValue("INACTIVITY_TIMEOUT");
            if (RECEIVE_TIMEOUT > 0)
                RECEIVE_TIMEOUT *= 1000; // convert to msecs

            RECEIVE_BUFFER_SIZE = MCconfig.getIntConfigValue("TCPIP_RECEIVE_BUFFER_SIZE");
            if (RECEIVE_BUFFER_SIZE < 0)
                RECEIVE_BUFFER_SIZE = 131400;

            SEND_TIMEOUT = MCconfig.getIntConfigValue("WRITE_TIMEOUT");
            if (SEND_TIMEOUT > 0)
                SEND_TIMEOUT *= 1000; // convert to msecs

            SEND_BUFFER_SIZE = MCconfig.getIntConfigValue("TCPIP_SEND_BUFFER_SIZE");
            if (SEND_BUFFER_SIZE < 0)
                SEND_BUFFER_SIZE = 131400;
        }

        public Ssl()
        {
            StartFunction = new Start(SslStart);
            ReadFunction = new Read(SslRead);
            WriteFunction = new Write(SslWrite);
            ShutdownFunction = new Shutdown(SslShutdown);
            ApplicationContext = new AppSslContext();
        }

        bool disposed = false;
        ~Ssl()
        {
            lock (Sync)
            {
                if (disposed)
                    return;

                try
                {
                    IEnumerator<IntPtr> en = connections.Keys.GetEnumerator();
                    while (en.MoveNext())
                        shutdown(en.Current, false);
                }
                catch { }
                finally
                {
                    connections.Clear();
                    disposed = true;
                }
            }
        }

        public String Certificate
        {
            get { return ((AppSslContext)ApplicationContext).Certificate; }
            set { ((AppSslContext)ApplicationContext).Certificate = value; }
        }

        public String Password
        {
            get { return ((AppSslContext)ApplicationContext).Password; }
            set { ((AppSslContext)ApplicationContext).Password = value; }
        }

        public Status SslStart(IntPtr Socket, int ConnType, IntPtr ApplicationContext, ref IntPtr SecureContext)
        {
            lock (Sync)
            {
                if (Socket == IntPtr.Zero)
                {
                    MClog.error("SslStart error, invalid session socket");
                    return Status.ERROR;
                }

                /* example of usage of application context */
                if (ApplicationContext == IntPtr.Zero)
                {
                    MClog.error("SslStart error, invalid application context");
                    return Status.ERROR;
                }

                AppSslContext ctx = (AppSslContext)Marshal.PtrToStructure(ApplicationContext, typeof(AppSslContext));
                if (ctx == null)
                {
                    MClog.error("SslStart error, invalid application context");
                    return Status.ERROR;
                }

                certificate = getCertificate(ctx.Certificate, ctx.Password);
                if (certificate == null)
                {
                    MClog.error("SslStart failed to read certificate");
                    return Status.ERROR;
                }

                MClog.info("SslStart");
                SslState state = null;
                SslStream ssl = null;

                try
                {
                    CertificateInfo(certificate);

                    TcpClient tcp = TcpClient(Socket);
                    if (tcp == null)
                        return Status.ERROR;

                    MCassociation association = Association(Socket);
                    if (association == null)
                        return Status.ERROR;

                    tcp.Client.ReceiveBufferSize = RECEIVE_BUFFER_SIZE;
                    tcp.Client.SendBufferSize = SEND_BUFFER_SIZE;

                    state = new SslState();

                    state.Type = ConnType;
                    state.Authenticate.Op = SslState.Operation.PROGRESS;
                    state.Authenticate.Event.Reset();
                    state.Tcp = tcp;

                    if (ConnType == MCsocket.ACCEPTOR)
                    {
                        ssl = new SslStream(tcp.GetStream(), false);

                        ssl.ReadTimeout = RECEIVE_TIMEOUT;
                        ssl.WriteTimeout = SEND_TIMEOUT;

                        state.ssl = ssl;

                        ssl.BeginAuthenticateAsServer(certificate, false, SslProtocols.Tls, true, new AsyncCallback(AuthenticateCallback), state);
                    }
                    else
                    {
                        ssl = new SslStream(tcp.GetStream(), true, CertificateValidationCallback, CertificateSelectionCallback);

                        ssl.ReadTimeout = RECEIVE_TIMEOUT;
                        ssl.WriteTimeout = SEND_TIMEOUT;

                        state.ssl = ssl;

                        ssl.BeginAuthenticateAsClient(association.RemoteHostName, new X509Certificate2Collection(new X509Certificate2[] { certificate }), SslProtocols.Tls, false, new AsyncCallback(AuthenticateCallback), state);
                    }
                }
                catch (Exception ex)
                {
                    MClog.error(String.Format("SslStart authentication failed, connection type {0}, error '{1}'", ConnType, ex.Message));
                    if (ssl != null)
                    {
                        try { ssl.Close(); }
                        catch { }
                        finally { ssl = null; }
                    }
                    return Status.ERROR;
                }

                state.Authenticate.Event.WaitOne(ssl.ReadTimeout);
                if (state.Authenticate.Op != SslState.Operation.COMPLETE)
                {
                    ssl.Close();
                    ssl = null;
                }

                if (ssl == null)
                    return Status.ERROR;

                SslInfo(ssl);

                SecureContext secureContext = new SecureContext();

                secureContext.Socket = Socket;
                secureContext.CipherAlgorithm = ssl.CipherAlgorithm.ToString();
                secureContext.HashAlgorithm = ssl.HashAlgorithm.ToString();
                secureContext.KeyExchangeAlgorithm = ssl.KeyExchangeAlgorithm.ToString();
                secureContext.SslProtocol = ssl.SslProtocol.ToString();

                SecureContext = getSecureContextHandle(secureContext);
                if (SecureContext == IntPtr.Zero)
                    return Status.ERROR;

                state.context = secureContext;
                state.contextHandle = SecureContext;

                connections[SecureContext] = state;
            }

            return Status.NORMAL_COMPLETION;
        }

        /// <summary>
        /// Ssl stream Read method
        /// </summary>
        /// <param name="SecureContext">Secure context</param>
        /// <param name="ApplicationContext">Application context</param>
        /// <param name="Buffer">Byte buffer</param>
        /// <param name="BytesToRead">Number of bytes to read</param>
        /// <param name="BytesRead">Number of read bytes</param>
        /// <param name="Timeout">Timeout in seconds</param>
        /// <returns></returns>
        public Status SslRead(IntPtr SecureContext, IntPtr ApplicationContext, IntPtr Buffer, uint BytesToRead, ref uint BytesRead, int Timeout)
        {
            Status status = Status.NORMAL_COMPLETION;

            if (BytesToRead == 0)
            {
                BytesRead = 0;
                return status;
            }

            SslState state = null;
            SslStream ssl = null;
            int timeout = 0;

            lock (Sync)
            {

                state = connections[SecureContext];
                if (state == null)
                    return Status.ERROR;

                ssl = state.ssl;
                if (ssl == null)
                    return Status.ERROR;

                if ((state.Tcp == null) || (state.Tcp.Client == null))
                    return Status.ERROR;

                timeout = Timeout * 1000; // milliseconds
                if (timeout > 0)
                {
                    try { ssl.ReadTimeout = timeout; }
                    catch { }
                }
                else
                {
                    Socket socket = state.Tcp.Client;
                    if ((socket != null) && !socket.Poll(0, SelectMode.SelectRead))
                        return Status.TIMEOUT;
                }
            }

            lock (state.Read.Sync)
            {
                //MClog.info(String.Format("SSlRead {0} bytes to read {1} timeout {2}", state, BytesToRead, timeout));

                try
                {
                    int bytes = Convert.ToInt32(BytesToRead), read = 0, offset = 0;
                    byte[] buffer = new byte[bytes];
                    BytesRead = 0;

                    DateTime start = DateTime.Now;
                    for (int i = 0; ; i++)
                    {
                        //MClog.info(String.Format("SSlRead {0} bytes to read {1} in cycle {2}", state, BytesToRead, i));

                        TimeSpan ts = (DateTime.Now - start);
                        if ((i > 0) && (ts.TotalMilliseconds > timeout))
                        {
                            MClog.info(String.Format("SSlRead {0} exits on timeout {1}", state, timeout));
                            status = Status.TIMEOUT;
                            break;
                        }

                        if (state.Read.Op == SslState.Operation.PROGRESS)
                        {
                            Thread.Sleep(SslState.Operation.TIMEOUT);
                            continue;
                        }

                        read = 0;
                        state.bytes = 0;
                        state.Read.Op = SslState.Operation.PROGRESS;
                        state.Read.Event.Reset();

                        ssl.BeginRead(buffer, offset, bytes, new AsyncCallback(ReadCallback), state);

                        if (!state.Read.Event.WaitOne(timeout))
                        {
                            //MClog.error(String.Format("SSlRead {0} BeginRead exits on timeout {1}", state, timeout));
                            status = Status.TIMEOUT;
                            break;
                        }

                        read = state.bytes;
                        if (read == 0)
                            break;

                        bytes -= read;
                        offset += read;

                        BytesRead += Convert.ToUInt32(read);

                        //MClog.info(String.Format("SSlRead {0} bytes read {1} cycle {2}", state, read, i++));

                        if (bytes <= 0)
                            break;
                    }

                    if (BytesRead > 0)
                        Marshal.Copy(buffer, 0, Buffer, Convert.ToInt32(BytesRead));
                }
                catch (Exception ex)
                {
                    MClog.error(String.Format("SslRead {0} error: {1}", state, ex.Message));
                    status = Status.ERROR;
                }
            }

            //MClog.info(String.Format("SSlRead bytes read {0} status {1}", BytesRead, status));

            return status;
        }

        /// <summary>
        /// Ssl stream Write method
        /// </summary>
        /// <param name="SecureContext">Secure context</param>
        /// <param name="ApplicationContext">Application context</param>
        /// <param name="Buffer">Byte buffer</param>
        /// <param name="BytesToWrite">Number of bytes to write</param>
        /// <param name="BytesWritten">Number of written bytes</param>
        /// <param name="Timeout">Timeout in seconds</param>
        /// <returns></returns>
        public Status SslWrite(IntPtr SecureContext, IntPtr ApplicationContext, IntPtr Buffer, uint BytesToWrite, ref uint BytesWritten, int Timeout)
        {
            Status status = Status.NORMAL_COMPLETION;

            if (BytesToWrite == 0)
            {
                BytesWritten = 0;
                return status;
            }

            SslState state = null;
            SslStream ssl = null;
            int timeout = 0;

            lock (Sync)
            {
                state = connections[SecureContext];
                if (state == null)
                    return Status.ERROR;

                ssl = state.ssl;
                if (ssl == null)
                    return Status.ERROR;

                timeout = Timeout * 1000; // milliseconds
                if (timeout > 0)
                {
                    try { ssl.WriteTimeout = timeout; } catch { }
                }
            }

            lock (state.Write.Sync)
            {
                //MClog.info(String.Format("SSlWrite {0} bytes to write {1} timeout {2}", state, BytesToWrite, timeout));

                try
                {
                    int bytes = Convert.ToInt32(BytesToWrite);
                    byte[] buffer = new byte[bytes];
                    BytesWritten = 0;
                    Marshal.Copy(Buffer, buffer, 0, bytes);

                    DateTime start = DateTime.Now;
                    for (int i = 0; ; i++)
                    {
                        //MClog.info(String.Format("SSlWrite {0} bytes to write {1} in cycle {2} ", state, BytesToWrite, i));

                        TimeSpan ts = (DateTime.Now - start);
                        if ((i > 0) && (ts.TotalMilliseconds > timeout))
                        {
                            //MClog.info(String.Format("SSlWrite {0} exits on timeout {1}", state, timeout));
                            status = Status.TIMEOUT;
                            break;
                        }

                        if (state.Write.Op == SslState.Operation.PROGRESS)
                        {
                            Thread.Sleep(SslState.Operation.TIMEOUT);
                            continue;
                        }
                        state.Write.Op = SslState.Operation.PROGRESS;
                        state.Write.Event.Reset();

                        ssl.BeginWrite(buffer, 0, bytes, new AsyncCallback(WriteCallback), state);

                        if (!state.Write.Event.WaitOne(timeout))
                        {
                            //MClog.error(String.Format("SSlWrite {0} BeginWrite exits on timeout {1}", state, timeout));
                            status = Status.TIMEOUT;
                        }

                        //MClog.info(String.Format("SSlWrite {0} bytes written {1} in cycle {2}", state, bytes, i++));                        
                        BytesWritten = Convert.ToUInt32(bytes);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    MClog.error(String.Format("SslWrite {0} error: {1}", state, ex.Message));
                    status = Status.ERROR;
                }
            }

            //MClog.info(String.Format("SSlWrite bytes written {0} status {1}", BytesWritten, status));
            return status;
        }

        public void SslShutdown(IntPtr SecureContext, IntPtr ApplicationContext)
        {
            shutdown(SecureContext, true);
        }

        private void shutdown(IntPtr secureContext, bool remove)
        {
            lock (Sync)
            {
                if (disposed)
                    return;

                if (secureContext == IntPtr.Zero)
                    return;

                try
                {
                    SslState state = connections[secureContext];
                    if (state == null)
                        return;

                    state.Tcp = null;
                    state.Shutdown.Op = SslState.Operation.PROGRESS;
                    //MClog.info(String.Format("SslShutdown {0} in PROGRESS", state));

                    SslStream ssl = state.ssl;
                    if (ssl != null)
                    {
                        try
                        {
                            ssl.Flush();
                            ssl.Dispose();
                        }
                        catch { }
                        finally { ssl = null; }
                    }

                    try { Marshal.FreeHGlobal(secureContext); }
                    catch { }

                    state.Shutdown.Op = SslState.Operation.COMPLETE;
                    //MClog.info(String.Format("SslShutdown {0} done", state));
                }
                catch { }
                finally
                {
                    if (remove)
                        connections.Remove(secureContext);
                }
            }
        }

        private X509Certificate2 getCertificate(String name, String pass)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(pass))
                return null;

            X509Certificate2 cert = null;
            try
            {
                cert = new X509Certificate2(name, pass);
            }
            catch (Exception ex)
            {
                MClog.error(String.Format("Ssl failed to create X509 certificate, error '{0}'", ex.Message));
                cert = null;
            }
            return cert;
        }

        private IntPtr getSecureContextHandle(SecureContext secureContext)
        {
            if (secureContext == null)
                return IntPtr.Zero;

            IntPtr handle = IntPtr.Zero;
            try
            {
                handle = Marshal.AllocHGlobal(Marshal.SizeOf(secureContext));
                Marshal.StructureToPtr(secureContext, handle, false);
            }
            catch
            {
                Marshal.FreeHGlobal(handle);
                handle = IntPtr.Zero;
            }
            return handle;
        }

        bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        X509Certificate CertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return certificate;
        }

        void AuthenticateCallback(IAsyncResult ar)
        {
            SslState state = ar.AsyncState as SslState;
            if ((state == null) || (state.Authenticate.Op != SslState.Operation.PROGRESS) || (state.ssl == null))
            {
                if (state != null)
                {
                    state.Authenticate.Op = SslState.Operation.ERROR;
                    state.Authenticate.Event.Set();
                }
                return;
            }

            try
            {
                if (state.Type == MCsocket.ACCEPTOR)
                {
                    state.ssl.EndAuthenticateAsServer(ar);
                }
                else
                {
                    state.ssl.EndAuthenticateAsClient(ar);
                }
                state.Authenticate.Op = SslState.Operation.COMPLETE;
            }
            catch (Exception ex)
            {
                MClog.error(String.Format("Ssl AuthenticateCallback error: {0}", ex.Message));
                state.Authenticate.Op = SslState.Operation.ERROR;
            }
            finally
            {
                state.Authenticate.Event.Set();
            }
        }

        void ReadCallback(IAsyncResult ar)
        {
            if (ar == null)
                return;

            SslState state = ar.AsyncState as SslState;
            if ((state == null) || (state.Read.Op != SslState.Operation.PROGRESS) || (state.ssl == null))
            {
                if (state != null)
                {
                    state.Read.Op = SslState.Operation.ERROR;
                    state.Read.Event.Set();
                }
                return;
            }
            try
            {
                state.bytes = state.ssl.EndRead(ar);
                state.Read.Op = SslState.Operation.COMPLETE;
            }
            catch (Exception ex)
            {
                MClog.error(String.Format("Ssl ReadCallback error: {0}", ex.Message));
                state.Read.Op = SslState.Operation.ERROR;
            }
            finally
            {
                state.Read.Event.Set();
            }
        }

        void WriteCallback(IAsyncResult ar)
        {
            if (ar == null)
                return;

            SslState state = ar.AsyncState as SslState;
            if ((state == null) || (state.Write.Op != SslState.Operation.PROGRESS) || (state.ssl == null))
            {
                if (state != null)
                {
                    state.Write.Op = SslState.Operation.ERROR;
                    state.Write.Event.Set();
                }
                return;
            }
            try
            {
                state.ssl.EndWrite(ar);
                state.ssl.Flush();
                state.Write.Op = SslState.Operation.COMPLETE;
            }
            catch (Exception ex)
            {
                MClog.error(String.Format("Ssl WriteCallback error: {0}", ex.Message));
                state.Write.Op = SslState.Operation.ERROR;
            }
            finally
            {
                state.Write.Event.Set();
            }
        }

        static void SslInfo(SslStream sslStream)
        {
            MClog.info(String.Format("Is Authenticated:            {0}", sslStream.IsAuthenticated));
            MClog.info(String.Format("Is Encrypted:                {0}", sslStream.IsEncrypted));
            MClog.info(String.Format("Is Signed:                   {0}", sslStream.IsSigned));
            MClog.info(String.Format("Is Mutually Authenticated:   {0}\n", sslStream.IsMutuallyAuthenticated));

            MClog.info(String.Format("Hash Algorithm:              {0}", sslStream.HashAlgorithm));
            MClog.info(String.Format("Hash Strength:               {0}", sslStream.HashStrength));
            MClog.info(String.Format("Cipher Algorithm:            {0}", sslStream.CipherAlgorithm));
            MClog.info(String.Format("Cipher Strength:             {0}\n", sslStream.CipherStrength));

            MClog.info(String.Format("Key Exchange Algorithm:      {0}", sslStream.KeyExchangeAlgorithm));
            MClog.info(String.Format("Key Exchange Strength:       {0}\n", sslStream.KeyExchangeStrength));
            MClog.info(String.Format("SSL Protocol:                {0}", sslStream.SslProtocol));
        }

        static void CertificateInfo(X509Certificate2 certificate)
        {
            MClog.info(String.Format("Certificate for: {0}", certificate.Subject));
            MClog.info(String.Format("Valid From:      {0}", certificate.GetEffectiveDateString()));
            MClog.info(String.Format("Valid To:        {0}", certificate.GetExpirationDateString()));
            MClog.info(String.Format("Format:          {0}", certificate.GetFormat()));
            MClog.info(String.Format("Issuer:          {0}", certificate.Issuer));

            MClog.info(String.Format("Serial Number:   {0}", certificate.GetSerialNumberString()));
            MClog.info(String.Format("Hash:            {0}", certificate.GetCertHashString()));
            MClog.info(String.Format("Key Algorithm:   {0}", certificate.GetKeyAlgorithm()));
            MClog.info(String.Format("Key Parameters:  {0}", certificate.GetKeyAlgorithmParametersString()));
            MClog.info(String.Format("Public Key:      {0}", certificate.GetPublicKeyString()));
        }
    }
}
