using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AgendaUtils
{
    public class Criptografia
    {
        public static string RetornarMD5(string valor)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return RetonarHash(md5Hash, valor);
            }
        }

        public static bool ComparaMD5(string valor, string valorCriptografado)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var senha = RetornarMD5(valor);
                if (VerificarHash(md5Hash, valorCriptografado, senha))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static string RetonarHash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static bool VerificarHash(MD5 md5Hash, string input, string hash)
        {
            StringComparer compara = StringComparer.OrdinalIgnoreCase;

            if (0 == compara.Compare(input, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
