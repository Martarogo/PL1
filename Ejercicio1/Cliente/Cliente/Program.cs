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

                Console.WriteLine("Escribe el texto que quieres enviar: ");
                String cadena = Console.ReadLine();

                Console.WriteLine("¿Que codificacion quieres utilizar? Escoge 1 o 2:\n1. Codificacion binaria\n2. Codificacion como texto");
                String codif = Console.ReadLine();

                while (!(codif.Equals("1")) && !(codif.Equals("2")))
                {
                    Console.WriteLine("Por favor, introduce '1' o '2'");
                    codif = Console.ReadLine();
                }

                cadena = cadena.Insert(0, codif);

                //char asdf = cadena[0];

                //cadena = cadena.Remove(0,1);

                EchoMessage mensaje = new EchoMessage(cadena);
                Console.WriteLine("\nCadena enviada: " + mensaje.Message + "\n");   

                if (codif.Equals("1"))
                {
                    bytes_cadena = encoding.Encode(mensaje);

                    byte[] bytes_codif = new byte[bytes_cadena.Length + 1];
                    
                    bytes_cadena.CopyTo(bytes_codif, 1);
                    bytes_codif[0] = 1;

                    netStream.Write(bytes_codif, 0, bytes_codif.Length);

                    netStream.Read(bytes_rec, 0, bytes_rec.Length);

                    cadena_rec = encoding.Decode(bytes_rec);

                    Console.WriteLine("\nCadena reenviada por el servidor: " + cadena_rec);
                }
                else if (codif.Equals("2"))
                {

                }
                
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
