using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoVocabulary
{
    public class EchoMessage
    {
        private String _message;
        private String _date;
        private String _EchoMessage;

        public String message
        {
            get
            {
                return _message;
            }
        }

        public String date
        {
            get
            {
                return _date;
            }
        }

        public String Message
        {
            get
            {
                return _EchoMessage;
            }
        }

        public EchoMessage(String msg)
        {
            _message = msg;
            _date = DateTime.Now.ToString("MM/dd/yyyy h:mm:ss.fff");
            _EchoMessage = msg + " " + _date;
        }
    }
}
