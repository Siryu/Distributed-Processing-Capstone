using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Tools
{
    internal class FileSender
    {
        private NetworkStream stream;
        private string fileName;
        private byte[] package;

        public FileSender(NetworkStream stream, string fileName)
        {
            this.stream = stream;
            this.fileName = fileName;
        }

        public void Start()
        {
            BuildPackage();
            Send();
        }

        private void BuildPackage()
        {
            byte[] data = File.ReadAllBytes(fileName);
            byte[] dataLength = BitConverter.GetBytes(data.Length);
            package = new byte[4 + data.Length];
            dataLength.CopyTo(package, 0);
            data.CopyTo(package, 4);
        }

        private void Send()
        {
            int bytesSent = 0;
            int bytesLeft = package.Length;
            int bufferSize = 1024;
            while (bytesLeft > 0)
            {
                int nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;
                stream.Write(package, bytesSent, nextPacketSize);
                bytesSent += nextPacketSize;
                bytesLeft -= nextPacketSize;
            }
        }
    }
}
