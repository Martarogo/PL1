using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using EchoVocabulary;

namespace Cliente
{
    class Client
    {
        //Variables:
        private static String SERVER = "localhost";
        private static int SERVERPORT = 23456;
        private BinaryEchoMessageCodec codec = new BinaryEchoMessageCodec();
                 
        //Run:
        public void Run()
        {
            //Mensaje de inicio + introducir mensaje:
            Console.WriteLine("Escribe el texto que quieres enviar: ");
            String str = Console.ReadLine();
            EchoMessage msg = new EchoMessage(str);
            byte[] bytesToSend = codec.Encode(msg);
            send(bytesToSend);           

            //Para mantener la consola abierta hasta que se pulse una tecla:
            Console.ReadKey();
        }//Fin Run

        private void send(byte[] strToSend)
        {
            UdpClient udpClient = new UdpClient();
            try
            {
                // Envíar información
                udpClient.Send(strToSend, strToSend.Length, SERVER, SERVERPORT);

                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Recibir información
                byte[] rcvPacket = udpClient.Receive(ref remoteIPEndPoint);
                Console.WriteLine("Cadena reenviada por el servidor: " + codec.Decode(rcvPacket));
                udpClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: "+e.Message);
                return;
            }
        }//Fin Send

        class Program
        {
            static void Main(string[] args)
            {
                const int N = 1;

                // Se crean los hilos
                Thread[] threads = new Thread[N];
                for (int i = 0; i < N; i++)
                {
                    Client client = new Client();
                    threads[i] = new Thread(new ThreadStart(client.Run));
                    threads[i].Start();
                }
                // Se espera a que los hilos terminen
                for (int i = 0; i < N; i++)
                {
                    threads[i].Join();
                }
            }
        }//Fin  Program
    }
}
