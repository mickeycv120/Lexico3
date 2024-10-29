using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lexico3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                using (Lexico T = new(@"prueba.cpp"))
                {
                    while (!T.finArchivo())
                    {
                        T.NextToken();
                    }
                    /* T.SetContenido("HOLA");
                    T.SetClasificacion(Token.Tipos.Identificador);
                    System.Console.WriteLine(T.GetContenido() + "=" + T.GetClasificacion());
                    T.SetContenido("123");
                    T.SetClasificacion(Token.Tipos.Numero);

                    System.Console.WriteLine(T.GetContenido() + "=" + T.GetClasificacion()); */

                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error: " + e.Message);

            }

        }
    }
}