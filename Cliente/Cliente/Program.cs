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
        private readonly String HOST = "localhost";
        private readonly int PORT = 23456;
        private readonly int MAXTIME = 1000;
        private TcpClient tcpClient;
        private NetworkStream netStream;
        private String serverEcho;
        private byte[] bSent;
        private byte[] bRec = new byte[512];
        ICodec encoding = new BinaryEchoMessageCodec();
        //ICodec encoding = new CharEchoMessageCodec();

        public void Run()
        {
            try
            {
                //Crear conexión:
                tcpClient = new TcpClient(HOST, PORT);
                netStream = tcpClient.GetStream();

                String text = WriteText();

                EchoMessage message = CreateMessage(text);

                ProcessMessage(message); 
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            finally
            {
                netStream.Close();
                tcpClient.Close();
            }

            //Para mantener la consola abierta hasta que se pulse una tecla:
            Console.ReadKey();
        }

        private EchoMessage CreateMessage(String text)
        {
            return new EchoMessage(text);
        }

        private String WriteText()
        {
            Console.WriteLine("Escribe el texto que quieres enviar: ");
            return Console.ReadLine();
        }

        private void ProcessMessage(EchoMessage msg)
        {
            byte[] bSent = Encode(msg);
            
            Console.WriteLine("Cadena enviada al servidor: " + msg.Message);

            //Envío de los datos codificados:
            netStream.Write(bSent, 0, bSent.Length);

            //Esperar la respuesta:
            try
            {
                tcpClient.ReceiveTimeout = MAXTIME;

                serverEcho = Decode(netStream);
            }
            catch (Exception e)
            {
                processException(e);               
            }
            Console.WriteLine("Cadena reenviada por el servidor: " + serverEcho);
        }

        private byte[] Encode(EchoMessage msg) 
        {
            return encoding.Encode(msg);
        }

        private String Decode(NetworkStream netStream)
        {
            return encoding.Decode(netStream);
        }

        private void processException(Exception e)
        {
            if (e.InnerException != null)
            {
                if (e.InnerException is SocketException)
                {
                    SocketException se = (SocketException)e.InnerException;
                    if (se.SocketErrorCode == SocketError.TimedOut)
                    {
                        Console.WriteLine("Ha expirado el temporizador");
                    }
                    else
                    {
                        Console.WriteLine("Error: " + se.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }//Fin processException
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
