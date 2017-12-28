using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harpokrat.EncryptionAlgorithms
{
    //TEA/XTEA algorithm in CFB mode
    public class TEA : IEncryptionStrategy
    {
        string key;

        //magic delta constant
        uint delta = 0x9E3779B9;

        UInt32[] K = new UInt32[4];

        public string Key
        {
            get
            {
                return this.key;
            }
            
            set
            {
                this.key = value;

                //converts string to ascii string
                string asciiKey = FromCharArray(GetCharsFromString(this.key));

                int point = 0;
                for(int i = 0; i < K.Length; i++)
                {
                    uint output;
                    output = ((uint)asciiKey[point]);
                    output += ((uint)asciiKey[point + 1] << 8);
                    output += ((uint)asciiKey[point + 2] << 16);
                    output += ((uint)asciiKey[point + 3] << 24);
                    point += 4;
                    K[i] = output;
                }
            }
        }

        public TEA()
        {
            this.Key = "secret key that is not shareable";
        }

        public TEA(string key)
        {
            this.Key = key;
        }


        #region encryption
        /// <summary>
        /// Encrypts the message parameter that is entered from user interface
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string Encrypt(string message)
        {
            string cipherText = "";
            byte[] data = Encoding.ASCII.GetBytes(message);

            // to avoid additional indexing through the loop
            uint a = K[0]; uint b = K[1];
            uint c = K[2]; uint d = K[3];

            uint L = 0;
            uint R = 0;
            
            for(int i = 0; i < data.Length; i +=2)
            {
                L = data[i];
                if (i+1 >= data.Length)
                    R = data[0];
                else
                    R = data[i + 1];                

                UInt32 sum = 0;

                // Feistel network for encryption in 32 rounds
                for (int j = 0; j < 32; j++)
                {
                    sum += delta;
                    L += ((R << 4) + a) ^ (R + sum) ^ ((R >> 5) + b);
                    R += ((L << 4) + c) ^ (L + sum) ^ ((L >> 5) + d);
                }

                cipherText += UIntToString(L) + UIntToString(R);
            }
            return cipherText;
        }
        #endregion encryption

        string UIntToString(uint r)
        {
            StringBuilder result = new StringBuilder();
            // Logical AND to preserve 1s on initial places
            result.Append((char)((r & 0xFF)));
            result.Append((char)(((r >> 8) & 0xFF)));
            result.Append((char)(((r >> 16) & 0xFF)));
            result.Append((char)(((r >> 24) & 0xFF)));
            return result.ToString();
        }

        /// <summary>
        /// Decrypts message parameter that is entered from the user interface
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string Decrypt(string message)
        {
            string result = "";
            byte[] data = new byte[message.Length / 8 * 2];
            int x = 0;

            uint L;
            uint R;

            uint a = K[0]; uint b = K[1];
            uint c = K[2]; uint d = K[3];

            for (int i = 0; i < message.Length; i+=8)
            {
                L = FromStringToUInt(message.Substring(i, 4));
                R = FromStringToUInt(message.Substring(i + 4, 4));

                UInt32 sum = delta << 5;

                // Again Feistel network for decryption in 32 rounds (the same amount we had while encrypting)
                for(int j = 0; j < 32; j++)
                {
                    R -= (((L << 4) + K[2]) ^ (L + sum) ^ ((L >> 5) + K[3]));
                    L -= (((R << 4) + K[0]) ^ (R + sum) ^ ((R >> 5) + K[1]));
                    sum -= delta;
                }

                data[x++] = (byte)L;
                data[x++] = (byte)R;
            }

            result = ASCIIEncoding.ASCII.GetString(data, 0, data.Length);
            if (result[result.Length - 1] == '\0')
                result = result.Substring(0, result.Length - 1);
            return result;
        }

        private uint FromStringToUInt(string v)
        {
            uint output;
            output = ((uint)v[0]);
            output += ((uint)v[1] << 8);
            output += ((uint)v[2] << 16);
            output += ((uint)v[3] << 24);
            return output;
        }

        #region helperfunctions
        string FromCharArray(char[] v)
        {
            string result = "";

            for (int i = 0; i < v.Length; i++)
                result += v[i];

            return result;
        }

        char[] GetCharsFromString(string key)
        {
            byte[] asciiBytes = Encoding.Convert(Encoding.Default, Encoding.ASCII, Encoding.Default.GetBytes(key));
            char[] data = new char[Encoding.ASCII.GetCharCount(asciiBytes, 0, asciiBytes.Length)];

            Encoding.ASCII.GetChars(asciiBytes, 0, asciiBytes.Length, data, 0);
            return data;
        }
        #endregion helperfunctions
    }
}
