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
        private static String[] SEPARATOR = { " | " };
        private static int PORT = 23456;
        private BinaryEchoMessageCodec codec = new BinaryEchoMessageCodec();

        public void Run()
        {
            Console.WriteLine("Servidor UDP en ejecución...");
            String received_str = "";
            String[] split_msg;
            byte[] bytesToSend = new byte[256];

            //Inicialización de un socket UDP:
            UdpClient udpClient = null;
            try
            {
                // Enlazar el socket en un puerto
                udpClient = new UdpClient(PORT);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
                return;
            }

            // Dirección desde donde recibir, se modificará tras la recepción
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // El servidor se ejecuta infinitamente
            for (; ; )
            {
                try
                {
                    // Recibir
                    byte[] bytes_rec = udpClient.Receive(ref remoteIPEndPoint);
                    received_str = codec.Decode(bytes_rec);

                    split_msg = received_str.Split(SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

                    EchoMessage msg = new EchoMessage(split_msg[0]);
                    Console.WriteLine("Cadena reenviada al cliente: " + msg.Message);
                    bytesToSend = codec.Encode(msg);

                    // Enviar
                    udpClient.Send(bytesToSend, bytesToSend.Length, remoteIPEndPoint);
                }

                catch (SocketException se)
                {
                    Console.WriteLine(se.ErrorCode + ": " + se.Message);
                    return;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Server echoServer = new Server();
            Thread thread = new Thread(new ThreadStart(echoServer.Run));
            thread.Start();
            thread.Join();
        }
    }
}

