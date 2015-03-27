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
        private static int CODIF = 2;   //1=Binario, 2=Char
        private static String SERVER = "localhost";
        private static int SERVERPORT = 23456;
        private String cadena_recibo;
        private byte[] bytes_recibo = new byte[512];
                 
        //Run:
        public void Run()
        {
            try
            {
                //Mensaje de inicio + introducir mensaje:
                Console.WriteLine("Escribe el texto que quieres enviar: ");
                String cadena = Console.ReadLine();

                EchoMessage mensaje = new EchoMessage(cadena);

                comoBits(mensaje);
                //comoTexto(mensaje);           
            }
            catch (Exception e)
            {
                trataExcepciones(e);
            }

            //Para mantener la consola abierta hasta que se pulse una tecla:
            Console.ReadKey();
        }//Fin Run

        private void comoBits(EchoMessage mensaje)
        {
            //Cliente UDP
            UdpClient clienteUDP = new UdpClient();

            //Pasar el mensaje a binario
            BinaryEchoMessageCodec binary_encoding=new BinaryEchoMessageCodec();
            byte[] cadena_envío = binary_encoding.Encode(mensaje);

            try
            {
                // Envíar información
                clienteUDP.Send(cadena_envío, cadena_envío.Length, SERVER, SERVERPORT);

                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Recibir información
                byte[] rcvPacket = clienteUDP.Receive(ref remoteIPEndPoint);
                cadena_recibo = binary_encoding.Decode(rcvPacket);
            }
            catch (Exception e)
            {
                trataExcepciones(e);               
            }
            Console.WriteLine("Cadena reenviada por el servidor: " + cadena_recibo);
            clienteUDP.Close();
        }//Fin comoBits

        private void comoTexto(EchoMessage mensaje)
        {
            //Cliente UDP
            UdpClient clienteUDP = new UdpClient();
            CharEchoMessageCodec char_encoding = new CharEchoMessageCodec();
            byte[] cadena_envío = char_encoding.Encode(mensaje);

            try
            {
                // Envíar información
                clienteUDP.Send(cadena_envío, cadena_envío.Length, SERVER, SERVERPORT);

                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Recibir información
                byte[] rcvPacket = clienteUDP.Receive(ref remoteIPEndPoint);
                cadena_recibo = char_encoding.Decode(rcvPacket);
            }
            catch (Exception e)
            {
                trataExcepciones(e);
            }
            Console.WriteLine("Cadena reenviada por el servidor: " + cadena_recibo);
            clienteUDP.Close();
            
        }//Fin comoTexto

        private void trataExcepciones(Exception e)
        {
            if (e.InnerException != null)
            {
                if (e.InnerException is SocketException)
                {
                    SocketException se = (SocketException)e.InnerException;
                    if (se.SocketErrorCode == SocketError.TimedOut)
                    {
                        Console.WriteLine("Ha expirado el temporizador");
                    }
                    else
                    {
                        Console.WriteLine("Error: " + se.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }//Fin trataExcepciones

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
        }//Fin clase Program
    }
}
