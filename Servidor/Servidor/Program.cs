using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EchoVocabulary;

namespace Servidor
{
    class Server
    {
        private readonly String[] SEPARATOR = { " | " };
        private byte[] sentBytes;
        //ICodec encoding = new BinaryEchoMessageCodec();
        ICodec encoding = new BinaryEchoMessageCodec();

        public void Run()
        {
            Console.WriteLine("Servidor en ejecución...");

            TcpListener listener = null;
            
            try
            {
                listener = new TcpListener(IPAddress.Any, 23456);
                listener.Start();
            }
            catch (SocketException se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
                return;
            }

            // El servidor se ejecuta infinitamente
            for (; ; )
            {
                TcpClient client = null;
                NetworkStream netStream = null;

                try
                {
                    client = listener.AcceptTcpClient();

                    netStream = client.GetStream();

                    String receivedMessage = Decode(netStream);

                    ProcessMessage(receivedMessage);

                    netStream.Write(sentBytes, 0, sentBytes.Length);

                    netStream.Close();
                    client.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                    netStream.Close();
                }
            }
        }

        private void ProcessMessage(String msg)
        {
            Console.WriteLine("Cadena recibida: " + msg);

            EchoMessage message = CreateMessage(msg);

            Console.WriteLine("Cadena reenviada al cliente: " + message.Message);

            sentBytes = Encode(message);
        }

        private String Decode(NetworkStream netStream)
        {
            return encoding.Decode(netStream);
        }

        private byte[] Encode(EchoMessage message)
        {
            return encoding.Encode(message); ;
        }

        private EchoMessage CreateMessage(String msg)
        {
            String[] receivedElements = msg.Split(SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

            return new EchoMessage(receivedElements[0]);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Server serv = new Server();
            Thread hilo = new Thread(new ThreadStart(serv.Run));
            hilo.Start();
            hilo.Join();
        }
    }
}

