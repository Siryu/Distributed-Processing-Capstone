using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActorModel.Tools
{
    internal static class ServerSecureSocket
    {        
        internal static SslStream ManageClientRequest(TcpClient client)
        {
            SslStream sslStream = null;

            try
            {
                bool leaveInnerStreamOpen = true;

                RemoteCertificateValidationCallback validationCallback =
                    new RemoteCertificateValidationCallback(ClientValidationCallback);
                LocalCertificateSelectionCallback selectionCallback =
                    new LocalCertificateSelectionCallback(ServerCertificateSelectionCallback);
                EncryptionPolicy encryptionPolicy = EncryptionPolicy.AllowNoEncryption;

                sslStream = new SslStream(client.GetStream(), leaveInnerStreamOpen, validationCallback, selectionCallback, encryptionPolicy);
                Console.WriteLine("Server starting handshake");
                ServerSideHandshake(sslStream);
                Console.WriteLine("Server finished handshake");
            }
            catch(Exception ex)
            {
                sslStream = null;
                Console.WriteLine("\nError detected in sslStream: " + ex.Message);
            }
            return sslStream;
        }

        private static void ServerSideHandshake(SslStream sslStream)
        {
            X509Certificate certificate = GetServerCertificate();

            bool requireClientCertificate = true;
            SslProtocols enabledSslProtocols = SslProtocols.Tls | SslProtocols.Ssl3;
            bool checkCertificateRevocation = true;

            sslStream.AuthenticateAsServer(certificate, requireClientCertificate, enabledSslProtocols, checkCertificateRevocation);
        }

        private static X509Certificate GetServerCertificate()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509CertificateCollection cert = store.Certificates.Find(X509FindType.FindBySubjectName, "LocalHost", true);
            return cert[0];
        }

        private static bool ClientValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            switch(sslPolicyErrors)
            {
                case SslPolicyErrors.RemoteCertificateNameMismatch:
                    Console.WriteLine("Client's name mismatch. End communication ...\n");
                    return false;
                case SslPolicyErrors.RemoteCertificateNotAvailable:
                    Console.WriteLine("Client's certificate not available. End communication ...\n");
                    return false;
                case SslPolicyErrors.RemoteCertificateChainErrors:
                    Console.WriteLine("Client's certificate validation failed. End communication ...\n");
                    return false;
            }
            Console.WriteLine("Client's authentication succeeded ...\n");
            return true;
        }

        private static X509Certificate ServerCertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            // perform checks on the certificate...
            return localCertificates[0];
        }
    }
}
