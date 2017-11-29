using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harpokrat.EncryptionAlgorithms
{
    public class Context
    {
        private string              _message;
        private IEncryptionStrategy _strategy;

        public string Message
        {
            get
            {
                return this._message;
            }

            set
            {
                this._message = value;
            }
        }

        public void SetEncryptionStrategy(IEncryptionStrategy strategy)
        {
            this._strategy = strategy;
        }

        public string Encrypt()
        {
            try
            {
                return _strategy.Encrypt(_message);
            }
            catch(NullReferenceException e)
            {
                System.Windows.Forms.MessageBox.Show("Strategy for encryption not set." + e.ToString(), "Strategy not set.", 
                    System.Windows.Forms.MessageBoxButtons.OK, 
                    System.Windows.Forms.MessageBoxIcon.Error, 
                    System.Windows.Forms.MessageBoxDefaultButton.Button1);
                return null;
            }
        }

        public string Decrypt()
        {
            try
            {
                return _strategy.Decrypt(_message);
            }
            catch(NullReferenceException e)
            {
                System.Windows.Forms.MessageBox.Show("Strategy for encryption not set." + e.ToString(), "Strategy not set.",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error,
                    System.Windows.Forms.MessageBoxDefaultButton.Button1);
                return null;
            }
        }

        // for testing purposes
        public bool Test()
        {
            return true;
        }
    }
}
