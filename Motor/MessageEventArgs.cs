using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motor
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }
        public bool AddExtraNewLine { get; set; }

        public MessageEventArgs(string message, bool addExtraNewLine)
        {
            Message = message;
            AddExtraNewLine = addExtraNewLine;
        }
    }
}
