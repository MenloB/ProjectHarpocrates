using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Harpokrat.EncryptionAlgorithms
{
    // RSA algorithm in CBC mode
    public class RSA : IEncryptionStrategy
    {
        int p;
        int q;

        //Public key
        int N;
        int euler;
        int e;

        //private key
        int d;

        public int PublicKeyN
        {
            get
            {
                return this.N;
            }
            set
            {
                this.N = value;
            }
        }

        public int PublicKeyE
        {
            get
            {
                return this.e;
            }
            set
            {
                this.e = value;
            }
        }

        public int PrivateKeyD
        {
            get
            {
                return this.d;
            }
            set
            {
                this.d = value;
            }
        }

        public int Euler
        {
            get
            {
                return this.euler;
            }
            set
            {
                this.euler = (this.p - 1) * (this.q - 1);
            }
        }

        public RSA()
        {
            p = 61;
            q = 53;

            this.N = p * q;

            this.e = 17;
            this.d = 413;
        }

        public string Decrypt(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);

            int temp = 0;

            int[] plainText = new int[message.Length];
            for(int i = 0; i < message.Length; i++)
            {
                temp = data[i];
                plainText[i] = this.Calculate(temp, this.d);
            }

            string plainTextString = "";

            foreach(int i in plainText)
            {
                plainTextString += (char)i;
            }

            return plainTextString;
        }

        private int Calculate(int startValue, int eksponent)
        {
            int retValue = startValue;

            for (int i = 1; i < eksponent; i++)
            {
                retValue = (retValue * startValue) % N;
            }

            return retValue;
        }

        public string Encrypt(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);

            int temp = 0;

            int[] cipherText = new int[message.Length];

            for(int i = 0; i < message.Length; i++)
            {
                temp = data[i];

                cipherText[i] = this.Calculate(temp, this.e);
            }

            string cipherTextString = "";

            foreach (int i in cipherText)
                cipherTextString += i + " ";

            return cipherTextString;
        }
    }
}
