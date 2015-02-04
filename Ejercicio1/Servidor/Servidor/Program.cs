using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servidor
{
    class Server
    {
        public void Run()
        {
            Console.WriteLine("A ver ");
            String cadena_rec = "";
            byte[] bytes_rec = new byte[1024];
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
                    Console.WriteLine("A ver 2");
                    client = listener.AcceptTcpClient();
                    Console.WriteLine("A ver 3");
                    netStream = client.GetStream();

                    netStream.Read(bytes_rec, 0, bytes_rec.Length);

                    Console.WriteLine("Lognitud: " + bytes_rec.Length);

                    using (MemoryStream ms = new MemoryStream(bytes_rec))
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        cadena_rec = reader.ReadString();
                    }

                    netStream.Close();
                    client.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                    netStream.Close();
                }
                Console.WriteLine("Cadena recibida: " + cadena_rec);
            }
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

