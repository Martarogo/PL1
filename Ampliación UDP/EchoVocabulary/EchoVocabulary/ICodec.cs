using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoVocabulary
{
    interface ICodec
    {
        byte[] Encode(EchoMessage message);
        String Decode(byte[] message);
    }
}
