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

        public void SetEncryptionStrategy(IEncryptionStrategy strategy)
        {
            this._strategy = strategy;
        }

        public void SetMessage(string message)
        {
            this._message = message;
        }

        public string Encrypt()
        {
            return _strategy.Encrypt(_message);
        }

        public void Decrypt()
        {
            _strategy.Decrypt(_message);
        }

        // for testing purposes
        public bool Test()
        {
            return true;
        }
    }
}
