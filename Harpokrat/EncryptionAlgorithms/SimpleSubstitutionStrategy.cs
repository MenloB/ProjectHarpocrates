using System;
using System.Diagnostics;
using Harpokrat.Constants;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Harpokrat.EncryptionAlgorithms
{
    // Simple substitution cypher algorithm
    // Implemented using Dictionary<byte, byte>
    public class SimpleSubstitutionStrategy : IEncryptionStrategy
    {
        // regular UTF8 English alphabet
        private Dictionary<byte, byte> alphabet;
        // coded UTF8 English alphabet
        private Dictionary<byte, byte> codedAlphabet;

        #region CONSTRUCTOR
        public SimpleSubstitutionStrategy()
        {
            alphabet      = new Dictionary<byte, byte>(26);
            codedAlphabet = new Dictionary<byte, byte>(26);

            // Start a stopwatch
            //var sw = Stopwatch.StartNew();
            // character = a
            uint character = 97;
            foreach(byte b in Variables.EncryptionKey)
            {
                // Tries to add to alphabet dictionary if argument of Add function already exists
                // catches an error
                try
                {
                    this.alphabet.Add(Convert.ToByte(character++), b);
                }
                catch(ArgumentException e)
                {
                    MessageBox.Show("Invalid encryption key." + e.ToString(), "Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error, 
                        MessageBoxDefaultButton.Button1, 
                        MessageBoxOptions.ServiceNotification);
                }
            }

            // character = a
            character = 97;
            foreach (byte b in Variables.EncryptionKey)
            {
                try
                {
                    this.codedAlphabet.Add(b, Convert.ToByte(character++));
                }
                catch (ArgumentException e)
                {
                    MessageBox.Show("Invalid encryption key (codedAlphabet)." + e.ToString(), "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.ServiceNotification);
                }
            }
            //sw.Stop();
            //MessageBox.Show(sw.ElapsedMilliseconds.ToString(), "Time passed in (ms).");

        }

        public SimpleSubstitutionStrategy(byte[] v)
        {
            alphabet = new Dictionary<byte, byte>();
            codedAlphabet = new Dictionary<byte, byte>();

            // Start a stopwatch
            //var sw = Stopwatch.StartNew();
            // character = a
            uint character = 97;
            foreach (byte b in v)
            {
                // Tries to add to alphabet dictionary if argument of Add function already exists
                // catches an error
                try
                {
                    this.alphabet.Add(Convert.ToByte(character++), b);
                }
                catch (ArgumentException e)
                {
                    MessageBox.Show("Invalid encryption key." + e.ToString(), "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.ServiceNotification);
                }
            }

            // character = a
            character = 97;
            foreach (byte b in v)
            {
                try
                {
                    this.codedAlphabet.Add(b, Convert.ToByte(character++));
                }
                catch (ArgumentException e)
                {
                    MessageBox.Show("Invalid encryption key (codedAlphabet)." + e.ToString(), "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.ServiceNotification);
                }
            }
            //sw.Stop();
            //MessageBox.Show(sw.ElapsedMilliseconds.ToString(), "Time passed in (ms).");
        }

        #endregion

        #region DECRYPT_FUNCTION
        public string Decrypt(string message)
        {
            string result = "";
            message = message.ToLower();
            foreach (var character in message)
            {
                if(GetValue(codedAlphabet, Convert.ToByte(character)) != ' ')
                    result += Convert.ToString(Convert.ToChar(GetValue(codedAlphabet, Convert.ToByte(character))));
            }

            return result;
        }
        #endregion

        #region ENCRYPT_FUNCTION
        public string Encrypt(string message)
        {
            string result = "";
            message = message.ToLower();
            foreach (var character in message)
            {
                if (GetValue(alphabet, Convert.ToByte(character)) != ' ')
                    result += Convert.ToString(Convert.ToChar(GetValue(alphabet, Convert.ToByte(character))));
            }

            return result;
        }
        #endregion

        #region HELPER_METHOD_GETVALUE
        // Gets Value in the dictionary based on key
        // if not there returns '#'
        public byte GetValue(Dictionary<byte, byte> _dictionary, byte key)
        {
            byte value;
            if (!_dictionary.TryGetValue(key, out value))
                return Convert.ToByte(' ');
            else
                return value;
        }
        #endregion
    }
}
