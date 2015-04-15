using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EchoVocabulary;

namespace UdpEchoVocabularyTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MessageTest()
        {
            String text = "Hola mundo";
            EchoMessage msg = new EchoMessage(text);
            Assert.AreEqual(msg.Message, text + " | " + msg.Date);
        }

        [TestMethod]
        public void BinaryDecodeByte()
        {
            EchoMessage msg = new EchoMessage("Hola mundo");
            BinaryEchoMessageCodec codec = new BinaryEchoMessageCodec();
            Assert.AreEqual(msg.Message, codec.Decode(codec.Encode(msg)));
        }

        [TestMethod]
        public void CharDecodeByte()
        {
            EchoMessage msg = new EchoMessage("Hola mundo");
            CharEchoMessageCodec codec = new CharEchoMessageCodec();
            Assert.AreEqual(msg.Message, codec.Decode(codec.Encode(msg)));
        }
    }
}