using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kazantseva.Nsudotnet.Enigma
{
    class Decryptor : BasicCrypt
    {
        //private SymmetricAlgorithm _algorithm;
        private String _algorithm;
        private String _fileFrom;
        private String _fileTo;
        private String _fileKey;

        public Decryptor(String from, String algorithm, String to, String key)
        {
            //this._algorithm = GetAlgorithm(algorithm);
            this._algorithm = algorithm;
            this._fileFrom = from;
            this._fileTo = to;
            this._fileKey = key;
            Console.WriteLine("from: " + _fileFrom);
            Console.WriteLine("To: " + _fileTo);
            Console.WriteLine("Key: " + _fileKey);
        }

        public override void Crypt()
        {
            using (SymmetricAlgorithm algorithm = GetAlgorithm(_algorithm))
            {
                using (FileStream keyStream = new FileStream(_fileKey, FileMode.Open))
                {
                    using (StreamReader keyReader = new StreamReader(keyStream))
                    {
                        algorithm.Key = Convert.FromBase64String(keyReader.ReadLine());
                        algorithm.IV = Convert.FromBase64String(keyReader.ReadLine());
                        using (FileStream fileToStream = new FileStream(_fileTo, FileMode.Create, FileAccess.Write))
                        {
                            using (FileStream fileFromStream =
                                new FileStream(_fileFrom, FileMode.Open, FileAccess.Read))
                            {
                                using (CryptoStream cryptoStream =
                                    new CryptoStream(fileToStream, algorithm.CreateDecryptor(),
                                        CryptoStreamMode.Write))
                                {
                                    fileFromStream.CopyTo(cryptoStream);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("Decryption done");
            }
        }
    }
}
