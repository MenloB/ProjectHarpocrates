using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harpokrat.EncryptionAlgorithms
{
    // Simple substitution cypher algorithm
    // Currently implemented with ArrayList
    // Working on implementing this using BitArray class
    public class SimpleSubstitutionStrategy : IEncryptionStrategy
    {
        private string alphabet;  // message to be encrypted
        private string coded;      // this will be the key (input from file or from UI)

        private ArrayList AlphabetBackUp = new ArrayList();
        private ArrayList CodedBackUp    = new ArrayList();

        #region Properties
        public string Alphabet
        {
            get
            {
                return this.alphabet;
            }
            set
            {
                this.alphabet = value;
                foreach (char c in this.alphabet.ToCharArray())
                {
                    this.AlphabetBackUp.Add(c);
                }
            }
        }

        public string Coded
        {
            get
            {
                return this.coded;
            }

            set
            {
                this.coded = "yqmnnsgwatkgetwtawuiqwemsg";
                foreach (char c in this.coded.ToCharArray())
                {
                    this.CodedBackUp.Add(c);
                }
            }
        }

        #endregion

        public string Decrypt(string message)
        {
            message = message.ToLower();
            string result = "";
            for (int i = 0; i < message.Length; i++)
            {
                int indexOfSourceChar = CodedBackUp.IndexOf(message[i]);
                if (indexOfSourceChar < 0 || (indexOfSourceChar > alphabet.Length - 1))
                {
                    result += "#";
                }
                else
                {
                    result += alphabet[indexOfSourceChar].ToString();
                }
            }
            return result;
        }

        public string Encrypt(string message)
        {
            message = message.ToLower();
            string result = "";
            for(int i = 0; i < message.Length; i++)
            {
                int indexOfSourceChar = AlphabetBackUp.IndexOf(message[i]);
                if (indexOfSourceChar < 0 || (indexOfSourceChar > coded.Length - 1))
                {
                    result += "#";
                }
                else
                {
                    result += coded[indexOfSourceChar].ToString();
                }
            }

            return result;
        }
    }
}
