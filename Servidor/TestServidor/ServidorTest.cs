using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using EchoVocabulary;


//Crea un programa cliente de test que haga uso del servicio. 
//Este programa deberá usar la biblioteca EchoVocabulary. 
//El programa cliente lanzará 100 hilos que invocarán al servicio y verificarán su funcionamiento comprobando que el mensaje de retorno coincide con el de ida. 
//Si el servidor tarda más de un segundo en responder se generará un mensaje de error.

namespace TestServidor
{
    [TestClass]
    public class ServidorTest
    {
        
        [TestMethod]
        public void TestReenvio()
        {
            //Crear conexiones e inicializar:
            TcpClient cliente = new TcpClient("localhost", 23456);
            NetworkStream stream = null;
            stream=cliente.GetStream();


            EchoMessage mensaje = new EchoMessage("Hola mundo");

            Assert.AreEqual(mensaje,recibido);
        }

        [TestMethod]
        public void TestTiempo()
        {
        }
    }
}
