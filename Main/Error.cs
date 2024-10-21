using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Lexico3
{
    public class Error : Exception
    {
        public Error(string message, StreamWriter log) : base(message)
        {
            log.WriteLine("Error:" + message);
            log.Flush();
        }
        public Error(string message, StreamWriter log, int linea) : base($"{message} en la línea {linea}")
        {
            log.WriteLine($"Error: {message} en la línea {linea}");
            log.Flush();
        }

    }
}