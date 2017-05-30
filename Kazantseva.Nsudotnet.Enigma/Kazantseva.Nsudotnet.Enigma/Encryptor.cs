using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kazantseva.Nsudotnet.Enigma
{
    class Encryptor : BasicCrypt
    {
        //private SymmetricAlgorithm _algorithm;
        private String _algorithm;
        private String _fileFrom;
        private String _fileTo;

        public Encryptor(String from, String algorithm, String to)
        {
            //this._algorithm = GetAlgorithm(algorithm);
            this._algorithm = algorithm;
            this._fileFrom = from;
            this._fileTo = to;
            
        }

        public override void Crypt()
        {
            using (SymmetricAlgorithm algorithm = GetAlgorithm(_algorithm))
            {
                algorithm.GenerateKey();
                algorithm.GenerateIV();

                String fileKey = Path.GetFileNameWithoutExtension(_fileFrom) + ".key.txt";

                using (FileStream keyStream = new FileStream(fileKey, FileMode.Create))
                {
                    using (StreamWriter keyWriter = new StreamWriter(keyStream))
                    {
                        keyWriter.WriteLine(Convert.ToBase64String(algorithm.Key));
                        keyWriter.WriteLine(Convert.ToBase64String(algorithm.IV));
                    }

                    using (FileStream fileToStream = new FileStream(_fileTo, FileMode.Create, FileAccess.Write))
                    {
                        using (FileStream fileFromStream = new FileStream(_fileFrom, FileMode.Open, FileAccess.Read))
                        {
                            using (CryptoStream cryptoStream =
                                new CryptoStream(fileToStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                fileFromStream.CopyTo(cryptoStream);
                            }
                        }
                    }
                    Console.WriteLine("Encyption done");
                }
            }
        }
    }
}
