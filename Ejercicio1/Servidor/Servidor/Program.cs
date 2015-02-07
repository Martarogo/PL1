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
            String[] separador = { " | " }, mensaje_rec;
            byte[] bytes_cadena, bytes_rec = new byte[256];
            byte codif;
            TcpListener listener = null;
            BinaryEchoMessageCodec encoding = new BinaryEchoMessageCodec();
            CharEchoMessageCodec char_encoding = new CharEchoMessageCodec();

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

                    netStream.Read(bytes_rec, 0, bytes_rec.Length);

                    /* El primer byte del array de bytes recibido indica la codificacion escogida por el cliente. Por lo tanto,
                     * la cadena de texto se encuentra a partir del elemento 1 */
                    codif = bytes_rec[0];
                    for (int i = 0; i < bytes_rec.Length-1; i++)
                    {
                        bytes_rec[i] = bytes_rec[i + 1];
                    }

                    if (codif == 1)
                    {
                        cadena_rec = encoding.Decode(bytes_rec);

                        Console.WriteLine("\n\nEl cliente ha escogido codificacion binaria.\n\nCadena recibida: " + cadena_rec);
                        
                        mensaje_rec = cadena_rec.Split(separador, StringSplitOptions.RemoveEmptyEntries);

                        EchoMessage mensaje = new EchoMessage(mensaje_rec[0]);
                        Console.WriteLine("Cadena reenviada al cliente: " + mensaje.Message);

                        bytes_cadena = encoding.Encode(mensaje);

                        netStream.Write(bytes_cadena, 0, bytes_cadena.Length);
                    }
                    else if (codif == 2)
                    {                    
                        cadena_rec = char_encoding.Decode(bytes_rec);
                        
                        Console.WriteLine("\n\nEl cliente ha escogido codificacion como texto.\n\nCadena recibida: " + cadena_rec);
                        
                        mensaje_rec = cadena_rec.Split(separador, StringSplitOptions.RemoveEmptyEntries);

                        EchoMessage mensaje = new EchoMessage(mensaje_rec[0]);

                        Console.WriteLine("Cadena reenviada al cliente: " + mensaje.Message);
                        Console.WriteLine("Bb?");
                        bytes_cadena = char_encoding.Encode(mensaje);

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

