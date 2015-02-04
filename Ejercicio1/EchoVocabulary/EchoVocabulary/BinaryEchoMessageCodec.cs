using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ICodec
{

}


namespace EchoVocabulary
{
    class BinaryEchoMessageCodec
    {
        public byte[] codificacion(String msg)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                writer.Write(msg);

                writer.Flush();
                return ms.ToArray();
            }
        }

        public String decodificacion(byte[] array)
        {
            using (MemoryStream ms = new MemoryStream(array))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                return reader.ReadString();
            }
        }
    }
}
