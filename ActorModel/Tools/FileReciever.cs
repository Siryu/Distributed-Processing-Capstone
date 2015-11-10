using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ActorModel.Tools
{
    internal class FileReciever
    {
        private NetworkStream stream;
        private string fileName;
        private int bytesRead = 0;
        private int dataLength = 0;

        public FileReciever(NetworkStream stream, string fileName)
        {
            this.stream = stream;
            this.fileName = fileName;
        }

        public void Start()
        {
            dataLength = GetLengthOfIncomingFile();
            CreateDirectories();
            WriteFile(ReadData());
        }

        private void CreateDirectories()
        {
            string[] directoryStructure = fileName.Split('\\');
            string directory = "";
            for (int i = 0; i < directoryStructure.Length - 1; i++)
            {
                directory += directoryStructure[i] + "\\";
            }
            Directory.CreateDirectory(@directory);
        }

        private int GetLengthOfIncomingFile()
        {
            byte[] length = new byte[4];
            bytesRead = stream.Read(length, 0, 4);
            int dataLength = BitConverter.ToInt32(length, 0);
            return dataLength;
        }

        private byte[] ReadData()
        {
            int bytesLeft = dataLength - 4;
            int allBytesRead = 0;
            int bufferSize = 1024;
            byte[] data = new byte[dataLength];

            while (bytesLeft > 0)
            {
                int nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;
                bytesRead = stream.Read(data, allBytesRead, nextPacketSize);
                allBytesRead += bytesRead;
                bytesLeft -= bytesRead;
            }
            return data;
        }

        private void WriteFile(byte[] data)
        {
            File.WriteAllBytes(fileName, data);
        }
    }
}
