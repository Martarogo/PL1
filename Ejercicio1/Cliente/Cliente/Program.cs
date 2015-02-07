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
            byte[] bytes_cadena, bytes_cadena_codif;
            byte[] bytes_rec = new byte[512];
            String cadena_rec = "";
            TcpClient client = null;
            NetworkStream netStream = null;
            BinaryEchoMessageCodec encoding = new BinaryEchoMessageCodec();
            CharEchoMessageCodec char_encoding = new CharEchoMessageCodec();

            try
            {
                client = new TcpClient("localhost", 23456);

                netStream = client.GetStream();

                Console.WriteLine("Escribe el texto que quieres enviar: ");
                String cadena = Console.ReadLine();

                Console.WriteLine("\n¿Que codificacion quieres utilizar? Escoge 1 o 2:\n1. Codificacion binaria\n2. Codificacion como texto");
                String codif = Console.ReadLine();

                while (!(codif.Equals("1")) && !(codif.Equals("2")))
                {
                    Console.WriteLine("Por favor, introduce '1' o '2'");
                    codif = Console.ReadLine();
                }

                EchoMessage mensaje = new EchoMessage(cadena);

                if (codif.Equals("1"))
                {
                    Console.WriteLine("\nSe ha escogido la codificacion binaria.\n\nCadena enviada: " + mensaje.Message);  

                    bytes_cadena = encoding.Encode(mensaje);

                    bytes_cadena_codif = new byte[bytes_cadena.Length + 1];
                    
                    bytes_cadena.CopyTo(bytes_cadena_codif, 1);
                    bytes_cadena_codif[0] = 1;

                    netStream.Write(bytes_cadena_codif, 0, bytes_cadena_codif.Length);

                    netStream.Read(bytes_rec, 0, bytes_rec.Length);

                    cadena_rec = encoding.Decode(bytes_rec);
                }
                else if (codif.Equals("2"))
                {
                    Console.WriteLine("\nSe ha escogido la codificacion como texto.\n\nCadena enviada: " + mensaje.Message);  

                    bytes_cadena = char_encoding.Encode(mensaje);

                    bytes_cadena_codif = new byte[bytes_cadena.Length + 1];

                    bytes_cadena.CopyTo(bytes_cadena_codif, 1);
                    bytes_cadena_codif[0] = 2;

                    netStream.Write(bytes_cadena_codif, 0, bytes_cadena_codif.Length);

                    netStream.Read(bytes_rec, 0, bytes_rec.Length);

                    cadena_rec = char_encoding.Decode(bytes_rec);
                }
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
