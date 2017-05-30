using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kazantseva.Nsudotnet.Enigma
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                BasicCrypt cryptor;
                switch (args[0].ToUpper())
                {
                    case "ENCRYPT":
                        if (args.Length != 4)
                        {
                            throw new ArgumentException(
                                "Wrong arguments\nUsage: {0} encrypt <input file> <algorithm> <output file>", Environment.GetCommandLineArgs()[0]);
                        }
                        cryptor = new Encryptor(args[1], args[2], args[3]);
                        break;
                    case "DECRYPT":
                        if (args.Length != 5)
                        {
                            throw new ArgumentException(
                                "Wrong arguments\nUsage: {0} decrypt <input file> <algorithm> <output file> <key file>", Environment.GetCommandLineArgs()[0]);
                        }
                        cryptor = new Decryptor(args[1], args[2], args[3], args[4]);
                        break;
                    default:
                        throw new ArgumentException(
                            "Wrong mode\nUsage: {0} encrypt/decrypt <input file> <algorithm> -/<key file> <output file>", Environment.GetCommandLineArgs()[0]);
                }

                cryptor.Crypt();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
