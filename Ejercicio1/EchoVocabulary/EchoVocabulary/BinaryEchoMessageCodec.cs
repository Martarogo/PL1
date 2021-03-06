﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoVocabulary;

public interface ICodec
{
    byte[] Encode(EchoMessage message);
    String Decode(byte[] array);
}

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

        public String Decode(byte[] array)
        {
            using (MemoryStream ms = new MemoryStream(array))
            using (BinaryReader reader = new BinaryReader(ms))
            {
                return reader.ReadString();
            }
        }
    }
}
