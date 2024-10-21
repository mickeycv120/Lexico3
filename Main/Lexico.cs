/* 
//SECTION - Requerimientos para el proyecto

    //SECTION - Número y Cadena

        //SECTION - Casos válidos:
            * 526
            * 526.18
            * 526E-813
            * 526E+813
            * 526E813
            * 526.18E-81
            * 526.18E+81
            * " "
            * '''
            * ' '
            * "Hola mundo"
        //!SECTION

        //SECTION - Casos inválidos:
        * 526.18E81  <- +|-|D Error léxico
        * 526. <- Error lexico
        * "ITQ <- Error lexico
        * ''
        * ""
        * "
        * '
        //!SECTION

    //!SECTION

//!SECTION

//SECTION - Tokens

//!SECTION
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace Lexico3
{
    public class Lexico : Token, IDisposable
    {
        //@params
        StreamReader archivo; //* archivo - el archivo que vamos a leer
        StreamWriter log; //* log - el archivo domde vamos a escribir lo que identifiquemos
        StreamWriter asm; //* asm - el archivo donde vamos a escribir el código ensamblador
        StreamWriter error; //* error - el archivo donde vamos a escribir los errores
        int linea;
        const int F = -1;
        const int E = -2;
        int[,] TRAND ={
            {0,1,2,3},
            {F,1,1,F},
            {F,F,2,F},
            {F,F,F,F},
        };
        //!@params

        public Lexico()
        {
            linea = 1;
            log = new StreamWriter("prueba.log");
            asm = new StreamWriter("prueba.asm");
            log.AutoFlush = true;
            asm.AutoFlush = true;
            if (File.Exists("prueba.cpp"))
            {
                archivo = new StreamReader("prueba.cpp");
            }
            else
            {
                throw new Error("El archivo prueba.cpp no existe", log);
            }
        }

        public Lexico(string nombre)
        {
            /* Si nombre es = suma.cpp
            LOG = suma.log
            ASM = suma.asm
            y validar la extensión del archivo
            checar como validar y cambiar la extensión del archivo */
            linea = 1;
            error = new StreamWriter("Error.log");

            string extension = Path.GetExtension(nombre);

            if (File.Exists(nombre))
            {
                if (extension != ".cpp")
                {
                    throw new Error($"El archivo {nombre} es de extensión {extension} y no de extensión \".cpp\"", error);
                }
                else
                {
                    archivo = new StreamReader(nombre);
                    log = new StreamWriter(Path.GetFileNameWithoutExtension(nombre) + ".log");
                    asm = new StreamWriter(Path.GetFileNameWithoutExtension(nombre) + ".asm");
                    log.AutoFlush = true;
                    asm.AutoFlush = true;
                }
            }
            else
            {
                throw new Error($"El archivo {nombre} no existe", error);
            }
        }

        public void Dispose()
        {
            log.WriteLine($"El archivo tiene {linea} líneas");
            archivo.Close();
            log.Close();
            asm.Close();
            error.Close();
        }


        private int columna(char c)
        {

            int columna;
            return 0;
        }

        private void clasifica(int state)
        {
            switch (state)
            {
                case 0: setClasificacion(Tipos.Cadena); break;
                case 1: setClasificacion(Tipos.Cadena); break;
                case 2: setClasificacion(); break;

                default:
            }
        }

        public void nextToken()
        {
            char transicion; // * - es el archivo pero carácter por carácter
            string buffer = "";
            int estado = 0;

            while (estado >= 0)
            {
                transicion = (char)archivo.Peek();
                //estado = automata(transicion, estado);

                if (estado == E)
                {
                    if (getClasificacion() == Tipos.Numero)
                    {
                        throw new Error("léxico, se espera un dígito", log, linea);
                    }
                    else if (getClasificacion() == Tipos.Cadena)
                    {
                        throw new Error("lexico, cadena no cerrada", log, linea);
                    }
                    else if (getClasificacion() == Tipos.Caracter)
                    {
                        throw new Error("lexico, argumento inesperado", log, linea);
                    }
                }


                if (estado >= 0)
                {
                    archivo.Read();
                    if (transicion == '\n')
                    {
                        linea++;
                    }

                    if (transicion == '/' && (char)archivo.Peek() == '/')
                    {
                        while (transicion != '\n' && !finArchivo())
                        {
                            archivo.Read();
                            transicion = (char)archivo.Peek();
                        }

                        estado = 0;
                        buffer = "";
                    }
                    else if (transicion == '/' && (char)archivo.Peek() == '*')
                    {
                        archivo.Read();
                        transicion = (char)archivo.Peek();
                        while (transicion != '*' && archivo.Peek() != '/')
                        {
                            archivo.Read();
                            transicion = (char)archivo.Peek();
                        }
                        estado = 0;
                        buffer = "";

                    }

                    if (estado > 0)
                    {
                        buffer += transicion;
                    }
                }
            }


            if (!finArchivo())
            {
                setContenido(buffer);
                //log.WriteLine($"{linea}  {getContenido()} = {getClasificacion()}");
                log.WriteLine($"{getContenido()} = {getClasificacion()}");
            }
            //linea++;
        }

        public bool finArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}

/*
    Expresion Regular: Metodo Formal que a traves de una secuencia de caracteres que define un PATRON de busqueda

    a)Reglas BNF
    b)Reglas BNF extendidas
    c)Operaciones aplicadas al lenguaje!!!!!
    
    OAL

    1. Concatenacion Simple(·)
    2. Concatenacion Exponencial (Exponente)
    3. Cerradura de Kleene (*)
    4. Ceraddura Positiva (+)
    5. Cerradura Epsilon (?)
    6. Operador OR (|)
    7. Parentesis ()

    L = {A, B, C, D, E,...Z a, b, c, d, e, ....z}
    D = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}


    1. L·D
       LD
       >=

    2. L^3 = LLL
       L^3D^2 = LLLDD
       D^5 = DDDDD
       =2 = ==

    3. L* = Cero o mas letras
       D* = Cero o mas digitos

    4. L+ = Una o mas letras   
       D+ = Uno o mas digitos
       
    5. L? = Cero o una letra (La letra es optativa-opcional)

    6. L | D = Letra o digito
       + | - = + o menos

    7. (L D) L? = (Letra seguido de un digito y al final una Letra opcional)

    Produccion Gramatical

    Clasificacion del Token -> Expresion Regular 

    Identificador -> L (L | D )*

    Numero -> D+ (.D+)? (E(+|-)? D+ )?

    FinSentencia-> ;
    InicioBloque-> {
    FinBloque-> }

    Caracter-> 
    OperadorTernario-> ?
    Puntero-> ->
    Termino+ -> + | =
    Termino- -> - | =
    Termino-P -> 
    OperadorFactor->  * | / | % (=)?
    IncrementoFactor-> *= | /= | %=
    IncrementoTermino-> + (+ | =) | - (-|=)
    OperadorMoneda->
    OperadorLogico-> && | || | !
    NotOpRel -> ! (=)?
    Asignacion-> =
    AsOpRel -> = (=)?
    OperadorRelacional-> >(=)? | <(> | = )? | =(= | !)
    Cadena-> "c*"
    Caracter -> 'c' | #D* | lambda
    
    Automata: Modelo Matematico que representa una expresion regular a traves de un GRAFO, para una maquina de estado finito
    que consiste en un conjunto de estados bien definidos, 
    - un estado inicial
    - un alfabeto de entrada
    - una funcion de transicion.

*/