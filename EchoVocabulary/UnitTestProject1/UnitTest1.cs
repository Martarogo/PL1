using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EchoVocabulary;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            EchoMessage msg = new EchoMessage("Hola");

            ICodec coding = new CharEchoMessageCodec();

            byte[] bSent = coding.Encode(msg);


        }
    }
}
