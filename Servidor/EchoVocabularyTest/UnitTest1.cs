using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EchoVocabulary;
using System.Net;
using System.Net.Sockets;

namespace EchoVocabularyTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MessageTest()
        {
            String text="Hola mundo";
            EchoMessage msg = new EchoMessage(text);
            Assert.AreEqual(msg.Message,text + " | " + msg.Date);
        }

         [TestMethod]
        public void BinaryDecodeByte()
        {
            EchoMessage msg = new EchoMessage("Hola mundo");
            BinaryEchoMessageCodec codec=new BinaryEchoMessageCodec();
            Assert.AreEqual(msg.Message,codec.Decode(codec.Encode(msg)));
        }

         [TestMethod]
         public void CharDecodeByte()
         {
             EchoMessage msg = new EchoMessage("Hola mundo");
             CharEchoMessageCodec codec = new CharEchoMessageCodec();
             byte[] etest = codec.Encode(msg);
             String test=codec.Decode(etest);
             Assert.AreEqual(msg.Message, test);
         }

        [TestMethod]
        public void BinaryDecodeStream()
        {
            EchoMessage msg = new EchoMessage("Hola mundo");
            BinaryEchoMessageCodec codec=new BinaryEchoMessageCodec();

            TcpListener listener = new TcpListener(IPAddress.Any, 23456);
            listener.Start();
            
            TcpClient sender=new TcpClient("localhost",23456);
            NetworkStream sstream=sender.GetStream();

            TcpClient receiver=listener.AcceptTcpClient();
            NetworkStream rstream=receiver.GetStream();

            byte[] encoded = codec.Encode(msg);
            sstream.Write(encoded, 0, encoded.Length);
            String receivedMessage = codec.Decode(rstream);

            sender.Close();
            receiver.Close();
            sstream.Close();
            rstream.Close();

           Assert.AreEqual(msg.Message,receivedMessage);
        }

        [TestMethod]
        public void CharDecodeStream()
        {
            EchoMessage msg = new EchoMessage("Hola mundo");
            CharEchoMessageCodec codec = new CharEchoMessageCodec();

            TcpListener listener = new TcpListener(IPAddress.Any, 23456);
            listener.Start();

            TcpClient sender = new TcpClient("localhost", 23456);
            NetworkStream sstream = sender.GetStream();

            TcpClient receiver = listener.AcceptTcpClient();
            NetworkStream rstream = receiver.GetStream();

            byte[] encoded = codec.Encode(msg);
            sstream.Write(encoded, 0, encoded.Length);
            String receivedMessage = codec.Decode(rstream);

            sender.Close();
            receiver.Close();
            sstream.Close();
            rstream.Close();

            Assert.AreEqual(msg.Message, receivedMessage);
        }
    }
}
