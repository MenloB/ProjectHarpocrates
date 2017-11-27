using System;
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
        private Dictionary<byte, byte> alphabet = new Dictionary<byte, byte>(26);
        // coded UTF8 English alphabet
        private Dictionary<byte, byte> codedAlphabet = new Dictionary<byte, byte>(26);

        // How??

        #region DictionaryInitialization_constructor
        public SimpleSubstitutionStrategy()
        {
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
                    MessageBox.Show("Invalid encryption key.", "Error", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error, 
                        MessageBoxDefaultButton.Button1, 
                        MessageBoxOptions.ServiceNotification);
                }
            }
        }
        #endregion

        public string Decrypt(string message)
        {
            message = message.ToLower();

        }

        public string Encrypt(string message)
        {
            throw new NotImplementedException();
        }
    }
}
