using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoVocabulary;
using System.Net.Sockets;

namespace EchoVocabulary
{
    public class BinaryEchoMessageCodec: ICodec
    {
        public byte[] Encode(EchoMessage message)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(message.Message);

                writer.Flush();
                return ms.ToArray();
            }
        }

        public String Decode(NetworkStream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            return reader.ReadString();
        }
    }
}
