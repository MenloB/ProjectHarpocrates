using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harpokrat.EncryptionAlgorithms
{
    public interface IEncryptionStrategy
    {
        String Encrypt();
        String Decrypt();
    }
}
