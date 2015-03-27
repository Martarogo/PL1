using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoVocabulary;

namespace PruebaMessage
{
    class Program
    {
        static void Main(string[] args)
        {
            EchoMessage message = new EchoMessage("hola");

            Console.WriteLine("Mensaje: " + message.message);
            Console.WriteLine("date: " + message.date);
            Console.WriteLine("EchoMessage: " + message.Message);

            Console.ReadKey();
        }
    }
}
