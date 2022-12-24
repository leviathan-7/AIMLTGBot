using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMLTGBot
{
    public class EmotionRecognitionException : Exception
    {
        public EmotionRecognitionException(string message) : base(message)
        {
        }
    }
}
