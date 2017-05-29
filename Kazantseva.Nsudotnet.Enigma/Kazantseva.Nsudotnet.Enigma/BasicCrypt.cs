using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kazantseva.Nsudotnet.Enigma
{
    abstract class BasicCrypt
    {
        public abstract void Crypt();

        public SymmetricAlgorithm GetAlgorithm(String algorithm)
        {
            switch (algorithm.ToUpper())
            {
                case "AES":
                    return new AesCryptoServiceProvider();
                case "DES":
                    return new DESCryptoServiceProvider();
                case "RC2":
                    return new RC2CryptoServiceProvider();
                case "RIJNDAEL":
                    return new RijndaelManaged();
                default:
                    throw new ArgumentException("Wrong algorithm name");

            }
        }

    }
}
