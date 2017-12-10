using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harpokrat.EncryptionAlgorithms
{
    // A51 algorithm with custom parameters
    // default key is: GBCHAFED
    public class A51 : IEncryptionStrategy
    {
        private BitArray X;
        private BitArray Y;
        private BitArray Z;

        //bits that are defined for majority function
        private int xmaj;
        private int ymaj;
        private int zmaj;

        private byte[] key;

        public A51()
        {
            X = new BitArray(19);
            Y = new BitArray(22);
            Z = new BitArray(23);

            // By default without modification these are bits that
            // are used for majority vote
            xmaj = 8;
            ymaj = 10;
            zmaj = 10;

            key = new byte[8];
            UTF8Encoding encoding = new UTF8Encoding();
            key = encoding.GetBytes("GBCHAFED");

            BitArray bitKey = new BitArray(key);

            initialize(X, 0, 19, bitKey);  // first 19 bits from the key are stored in X
            initialize(Y, 19, 22, bitKey); // first 22 bits from the key are stored in Y
            initialize(Z, 41, 23, bitKey); // first 23 bits from the key are stored in Z
        }

        public A51(string key)
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
            //message = message.ToLower();
            //UTF8Encoding encoding = new UTF8Encoding();
            //byte[] messageBytes = encoding.GetBytes(message);

            throw new NotImplementedException();
        }

        public bool majority()
        {
            if ((X[xmaj] == true) && (Y[ymaj] == true))
                return true;
            else if ((X[xmaj] == true) && (Z[zmaj] == true))
                return true;
            else if ((Y[ymaj] == true) && (Z[zmaj] == true))
                return true;
            else
                return false;
        }

        public void initialize(BitArray register, int offset, int size, BitArray key)
        {
            for(int i = 0; i < size; i++)
            {
                register[i] = key[offset + i];
            }
        }
    }
}
