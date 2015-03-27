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
        private readonly String[] SEPARATOR = { " | " };
        private String strRec = "";
        private String[] elemRec;
        private int nRec;
        private byte[] bSent, bRec = new byte[256];

        public void Run()
        {
            Console.WriteLine("Servidor en ejecución...");

            TcpListener listener = null;
            //ICodec encoding = new BinaryEchoMessageCodec();
            ICodec encoding = new CharEchoMessageCodec();

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

                    //netStream.Read(bytes_rec, 0, bytes_rec.Length);

                    strRec = encoding.Decode(netStream);

                    Console.WriteLine("Cadena recibida: " + strRec);
                        
                    elemRec = strRec.Split(SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

                    EchoMessage mensaje = new EchoMessage(elemRec[0]);

                    Console.WriteLine("Cadena reenviada al cliente: " + mensaje.Message);

                    bSent = encoding.Encode(mensaje);

                    netStream.Write(bSent, 0, bSent.Length);

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

