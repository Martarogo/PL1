using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoVocabulary
{
    class EchoMessage
    {
        private String _mensaje;

        public String mensaje
        {
            get
            {
                return _mensaje;
            }
            set
            {
                _mensaje = value;
            }
        }

        public EchoMessage(String msg)
        {
            _mensaje = msg;
        }


    }
}
