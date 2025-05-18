using OpenGost.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeContCoreLib
{
    public static class OpenGostConfigurator
    {
        public static void ConfigureGostCryptographicServices()
        {
            OpenGostCryptoConfig.ConfigureCryptographicServices();
        }
    }
}
