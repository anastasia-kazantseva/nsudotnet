using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kazantseva.Nsudotnet.LinesCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Counter counter = new Counter(args[0]);
                Console.WriteLine("There are {0} lines in *.{1} files in current directory", counter.CountLines(),
                    args[0]);
                Console.ReadKey();
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Too few arguments. Extention required.");
                Console.ReadKey();
            }
        }
    }
}
