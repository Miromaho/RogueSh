using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeCL.System
{
    public class MessLogs
    {
        private static readonly int maxLines = 9;

        private readonly Queue<string> _lines;

        public MessLogs()
        {
            _lines = new Queue<string>();
        }


        public void AddLine(string message)
        {
            _lines.Enqueue(message);

            if (_lines.Count > maxLines)
            {
                _lines.Dequeue();
            }
        }


        public void Draw(RLConsole console)
        {
            string[] lines = _lines.ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                console.Print(1, i + 1, lines[i], RLColor.White);
            }
        }
    }
}
    

    
