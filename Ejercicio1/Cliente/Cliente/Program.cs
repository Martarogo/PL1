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
            byte[] bytes_rec = new byte[512];
            String cadena_rec;
            TcpClient client = null;
            NetworkStream netStream = null;
            BinaryEchoMessageCodec encoding = new BinaryEchoMessageCodec();

            try
            {
                client = new TcpClient("localhost", 23456);

                netStream = client.GetStream();

                String cadena = "jeje";

                EchoMessage mensaje = new EchoMessage(cadena);
                Console.WriteLine("Cadena enviada: " + mensaje.Message);

                bytes_cadena = encoding.Encode(mensaje);

                netStream.Write(bytes_cadena, 0, bytes_cadena.Length);

                netStream.Read(bytes_rec, 0, bytes_rec.Length);

                cadena_rec = encoding.Decode(bytes_rec);

                Console.WriteLine("Cadena reenviada por el servidor: " + cadena_rec);
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
            Console.ReadKey();
        }
    }


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
    }
}
