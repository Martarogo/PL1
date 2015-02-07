using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoVocabulary
{
    public class CharEchoMessageCodec: ICodec
    {
        public byte[] Encode(EchoMessage message)
        {
            return Encoding.UTF8.GetBytes(message.Message);
        }

        public String Decode(byte[] array)
        {
            return Encoding.UTF8.GetString(array, 0, array.Length);
        }
    }
}
