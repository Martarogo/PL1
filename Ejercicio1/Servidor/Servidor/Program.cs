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
        public void Run()
        {
            Console.WriteLine("Servidor en ejecución...");
            String cadena_rec = "";
            byte[] bytes_cadena;
            byte[] bytes_rec = new byte[512];
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
                BinaryEchoMessageCodec encoding = new BinaryEchoMessageCodec();

                try
                {
                    client = listener.AcceptTcpClient();

                    netStream = client.GetStream();

                    netStream.Read(bytes_rec, 0, bytes_rec.Length);

                    if (bytes_rec[0] == 1)
                    {
                        cadena_rec = encoding.Decode(bytes_rec);

                        bytes_rec.

                        Console.WriteLine("Cadena recibida: " + cadena_rec);

                        String[] separador = { " | " };
                        String[] mensaje_rec = cadena_rec.Split(separador, StringSplitOptions.RemoveEmptyEntries);

                        EchoMessage mensaje = new EchoMessage(mensaje_rec[0]);
                        Console.WriteLine("Cadena reenviada: " + mensaje.Message);

                        bytes_cadena = encoding.Encode(mensaje);

                        netStream.Write(bytes_cadena, 0, bytes_cadena.Length);
                    }
                    

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

