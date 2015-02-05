using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EchoVocabulary;

namespace Cliente
{
    class Client
    {
        public void Run()
        {
            byte[] bytes_cadena;

            TcpClient client = null;
            NetworkStream netStream = null;
            BinaryEchoMessageCodec encoding = new BinaryEchoMessageCodec();

            try
            {
                client = new TcpClient("localhost", 23456);

                netStream = client.GetStream();

                String cadena = "hola";
                Console.WriteLine("Cadena enviada: " + cadena);

                EchoMessage mensaje = new EchoMessage(cadena);

                bytes_cadena = encoding.Encode(mensaje);

                netStream.Write(bytes_cadena, 0, bytes_cadena.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                netStream.Close();
                client.Close();
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            const int N = 100;

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
    }
}
