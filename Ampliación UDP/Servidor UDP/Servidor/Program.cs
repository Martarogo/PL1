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
        private static String[] SEPARADOR = { " | " };
        private static int PORT = 23456;
        private BinaryEchoMessageCodec encoding = new BinaryEchoMessageCodec();

        public void Run()
        {
            Console.WriteLine("Servidor UDP en ejecución...");
            String cadena_rec = "";
            String[] mensaje_rec;
            byte[] bytes_cadena = new byte[256];

            //Inicialización de un socket UDP:
            UdpClient clienteUDP = null;
            try
            {
                // Enlazar el socket en un puerto
                clienteUDP = new UdpClient(PORT);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.ErrorCode + ": " + se.Message);
                return;
            }

            // Dirección desde donde recibir, se modificará tras la recepción
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // El servidor se ejecuta infinitamente
            for (; ; )
            {
                try
                {
                    // Recibir
                    byte[] bytes_rec = clienteUDP.Receive(ref remoteIPEndPoint);
                    
                    cadena_rec = encoding.Decode(bytes_rec);

                    mensaje_rec = cadena_rec.Split(SEPARADOR, StringSplitOptions.RemoveEmptyEntries);

                    EchoMessage mensaje = new EchoMessage(mensaje_rec[0]);
                    Console.WriteLine("Cadena reenviada al cliente: " + mensaje.Message);

                    bytes_cadena = encoding.Encode(mensaje);

                    // Enviar
                    clienteUDP.Send(bytes_rec, bytes_rec.Length, remoteIPEndPoint);
                }

                catch (SocketException se)
                {
                    Console.WriteLine(se.ErrorCode + ": " + se.Message);
                    return;
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

