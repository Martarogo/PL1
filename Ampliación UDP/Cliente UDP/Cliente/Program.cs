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
        private String cadena_recibo;
        private byte[] bytes_recibo, bytes_envio = new byte[512];
        private BinaryEchoMessageCodec codec = new BinaryEchoMessageCodec();
                 
        //Run:
        public void Run()
        {
            //Mensaje de inicio + introducir mensaje:
            Console.WriteLine("Escribe el texto que quieres enviar: ");
            String cadena = Console.ReadLine();
            EchoMessage mensaje = new EchoMessage(cadena);
            bytes_envio = codec.Encode(mensaje);
            send(bytes_envio);
           

            //Para mantener la consola abierta hasta que se pulse una tecla:
            Console.ReadKey();
        }//Fin Run

        private void send(byte[] cadena_envío)
        {
            UdpClient clienteUDP = new UdpClient();
            try
            {
                // Envíar información
                clienteUDP.Send(cadena_envío, cadena_envío.Length, SERVER, SERVERPORT);

                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Recibir información
                byte[] rcvPacket = clienteUDP.Receive(ref remoteIPEndPoint);
                cadena_recibo = codec.Decode(rcvPacket);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: "+e.Message);
                return;
            }
            Console.WriteLine("Cadena reenviada por el servidor: " + cadena_recibo);
            clienteUDP.Close();

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
