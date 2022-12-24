using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork1
{
    public class EmotionRecognitionException : Exception
    {
        public EmotionRecognitionException(string message) : base(message)
        {
        }
    }
}
