using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harpokrat.EncryptionAlgorithms
{
    // Simple substitution cypher algorithm
    public class SimpleSubstitutionStrategy : IEncryptionStrategy
    {
        private String message;  // message to be encrypted
        private String key;      // this will be the key (input from file or from UI)

        #region Properties
        public String Message
        {
            get
            {
                return message;
            }
            set
            {
                this.message = value;
            }
        }

        public String Key
        {
            get
            {
                return key;
            }

            set
            {
                this.key = value;
            }
        }

        #endregion

        public string Decrypt()
        {
            throw new NotImplementedException();
        }

        public string Encrypt()
        {
            throw new NotImplementedException();
        }
    }
}
