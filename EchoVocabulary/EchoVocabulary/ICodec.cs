using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EchoVocabulary
{
    public interface ICodec
    {
        byte[] Encode(EchoMessage message);
        String Decode(NetworkStream stream);
    }
}
