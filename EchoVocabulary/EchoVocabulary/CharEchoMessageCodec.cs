using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EchoVocabulary
{
    public class CharEchoMessageCodec: ICodec
    {
        public byte[] Encode(EchoMessage message)
        {
            return Encoding.UTF8.GetBytes(message.Message + "\r\n");
        }

        public String Decode(NetworkStream stream)
        {
            byte[] bRec = new byte[256];

            String cRec, strRec = null;
            bool b = true;
            int i = 0;
            stream.Read(bRec, i, 1);
            cRec = Encoding.UTF8.GetString(bRec, i, 1);

            while (b) {
                strRec = strRec + cRec;
                i++;
                stream.Read(bRec, i, 1);
                cRec = Encoding.UTF8.GetString(bRec,i,1);
                if (cRec.Equals("\n"))
                {
                    if (strRec.EndsWith("\r"))
                    {
                        b = false;
                    }
                }
            }
            return strRec.Substring(0,strRec.Length-1);
        }
    }
}
