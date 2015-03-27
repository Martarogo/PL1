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
        //Variables:
        private readonly String HOST = "localhost";
        private readonly int PORT = 23456;
        private readonly int TIEMPOMAX = 1000;
        private TcpClient clienteTCP;
        private NetworkStream netStream;
        private String strRec;
        private byte[] bSent;
        private byte[] bRec = new byte[512];

        //Run:
        public void Run()
        {
            try
            {
                //Crear conexión:
                clienteTCP = new TcpClient(HOST, PORT);
                netStream = clienteTCP.GetStream();

                //Mensaje de inicio + introducir mensaje:
                Console.WriteLine("Escribe el texto que quieres enviar: ");
                String str = Console.ReadLine();

                EchoMessage msg = new EchoMessage(str);

                processMessage(msg); 
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }

            finally
            {
                netStream.Close();
                clienteTCP.Close();
            }

            //Para mantener la consola abierta hasta que se pulse una tecla:
            Console.ReadKey();
        }//Fin Run

        private void processMessage(EchoMessage msg)
        {
            //Pasar el mensaje a binario
            //ICodec coding = new BinaryEchoMessageCodec();
            ICodec coding = new CharEchoMessageCodec();

            //Enconde incluye la fecha de envio en el mensaje, y devuelve el array de bytes
            byte[] bSent = coding.Encode(msg);

            Console.WriteLine("Cadena enviada al servidor: " + msg.Message);

            //Envío de los datos codificados:
            netStream.Write(bSent, 0, bSent.Length);

            //Esperar la respuesta:
            try
            {
                //Arrancar temporizador:
                clienteTCP.ReceiveTimeout = TIEMPOMAX;

                //Leer datos y decodificar:
                strRec = coding.Decode(netStream);
            }
            catch (Exception e)
            {
                processException(e);               
            }
            Console.WriteLine("Cadena reenviada por el servidor: " + strRec);
        }//Fin comoBits


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
        }//Fin clase Program
    }
}
