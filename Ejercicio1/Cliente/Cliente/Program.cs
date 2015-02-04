using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cliente
{
    class Client
    {
        public void Run()
        {
            String cadena = "hola";
            Console.WriteLine("Cadena enviada: " + cadena);
            byte[] bytes_cadena;

            TcpClient client = null;
            NetworkStream netStream = null;
            try
            {
                client = new TcpClient("localhost", 23456);

                netStream = client.GetStream();

                //Codificacion, se convierte la cadena de String a byte[]
                using (MemoryStream ms = new MemoryStream())
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write(cadena);

                    writer.Flush();
                    bytes_cadena = ms.ToArray();
                }

                Console.WriteLine("Lognitud: " + bytes_cadena.Length);

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
