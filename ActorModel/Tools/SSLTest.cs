using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Tools
{
    /// <summary>
    /// Network Security layer
    /// </summary>
    public class SSLTest
    {
        /// <summary>
        /// Takes in a TCPClient and converts it to an SSL Stream.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>SslStream from the recieved TcpClient</returns>
        public static SslStream ManageClient(TcpClient client)
        {
            SslStream sslStream = null;
            try
            {
                string certName = "localhost";
                sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(CertificateValidationCallback));
                sslStream.AuthenticateAsClient(certName);
                Console.WriteLine("Client authenticated.......");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return sslStream;
        }

        /// <summary>
        /// Authenticates as a Server.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>SSL Stream after authentication.</returns>
        public static SslStream ManageServer(TcpClient client)
        {
            try
            {
                X509Certificate cert = getServerCert();
                SslStream sslStream = new SslStream(client.GetStream());
                sslStream.AuthenticateAsServer(cert, false, SslProtocols.Ssl3, false);
                Console.WriteLine("Server Authenticated.........");
                return sslStream;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private static X509Certificate getServerCert()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509CertificateCollection cert = store.Certificates.Find(X509FindType.FindBySubjectName, "LocalHost", true);
            return cert[0];
        }

        private static bool CertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            bool ok = true;
            if(sslPolicyErrors != SslPolicyErrors.None)
            {
                Console.WriteLine("SSL Certificate Validation Error!........");
                Console.WriteLine(sslPolicyErrors.ToString());
                ok = false;
            }
            return ok;
        }
    }
}
