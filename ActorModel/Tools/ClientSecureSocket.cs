using ActorModel.Actors;
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
    internal static class ClientSecureSocket
    {
        internal static SslStream ManageServerRequest(TcpClient client, String connectToIP)
        {
            SslStream sslStream = null;
            try
            {
                bool leaveInnerStreamOpen = true;

                RemoteCertificateValidationCallback validationCallback =
                    new RemoteCertificateValidationCallback(ServerValidationCallback);

                LocalCertificateSelectionCallback selectionCallback =
                    new LocalCertificateSelectionCallback(ClientCertificateSelectionCallback);

                EncryptionPolicy encryptionPolicy = EncryptionPolicy.NoEncryption;

                sslStream = new SslStream(client.GetStream(), leaveInnerStreamOpen, validationCallback, selectionCallback, encryptionPolicy);
                Console.WriteLine("Client starting handshake");
                ClientSideHandshake(sslStream, connectToIP);
                Console.WriteLine("client finished handshake");
            }
            catch(AuthenticationException ex)
            {
                Console.WriteLine("\nAuthentication Exception: " + ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("\nError detected: " + ex.Message);
            }

            return sslStream;
        }

        private static void ClientSideHandshake(SslStream stream, String connectToIP)
        {
            X509CertificateCollection clientCertificates = GetClientCertificates("localhost");

            SslProtocols sslProtocol = SslProtocols.Tls;
            bool checkCertificateRevocation = true;
            stream.AuthenticateAsClient(connectToIP, clientCertificates, sslProtocol, checkCertificateRevocation);
        }

        private static bool ServerValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            switch (sslPolicyErrors)
            {
                case SslPolicyErrors.RemoteCertificateNameMismatch:
                    Console.WriteLine("Server name mismatch.  End communication ...\n");
                    return false;
                case SslPolicyErrors.RemoteCertificateNotAvailable:
                    Console.WriteLine("Server's certificate not available.  End communication ...\n");
                    return false;
                case SslPolicyErrors.RemoteCertificateChainErrors:
                    Console.WriteLine("Server's certificate validation failed.  End communication ...\n");
                    return false;
            }

            Console.WriteLine("Server's authentication succeeded ...\n");
            return true;
        }

        private static X509Certificate ClientCertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            // perform some check on the certificate....
            return localCertificates[0];
        }

        private static X509CertificateCollection GetClientCertificates(string certName)
        {
            X509CertificateCollection collection = new X509CertificateCollection();
            X509Certificate cer = new X509Certificate(certName, "");
            
            
            //cer.Import(certName);
            //X509Store store = new X509Store(StoreLocation.LocalMachine);
            //store.Certificates.Add(cer);
            collection.Add(cer);
            return collection;
        }
    }
}
