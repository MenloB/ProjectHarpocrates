using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harpokrat.EncryptionAlgorithms
{
    // A51 algorithm with custom parameters
    public class A51 : IEncryptionStrategy
    {
        private BitArray X;
        private BitArray Y;
        private BitArray Z;

        public A51()
        {
            X = new BitArray(19);
            Y = new BitArray(22);
            Z = new BitArray(23);
        }

        public string Decrypt(string message)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string message)
        {
            throw new NotImplementedException();
        }
    }
}
