using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgendaUtils
{
    public class Validacoes
    {
        public static bool ValidaCpf(string cpf)
        {
            try
            {
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                string tempCpf;

                string digito;

                int soma;

                int resto;

                cpf = cpf.Trim();

                cpf = cpf.Replace(".", "").Replace("-", "");

                if (cpf.Length != 11)

                    return false;

                tempCpf = cpf.Substring(0, 9);

                soma = 0;

                for (int i = 0; i < 9; i++)

                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

                resto = soma % 11;

                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();

                tempCpf = tempCpf + digito;

                soma = 0;

                for (int i = 0; i < 10; i++)

                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

                resto = soma % 11;

                if (resto < 2)

                    resto = 0;

                else

                    resto = 11 - resto;

                if (string.IsNullOrWhiteSpace(digito))
                    digito = "";
                
                return cpf.EndsWith(digito + resto);
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidaCep(string cep)
        {
            Regex Rgx = new Regex(@"^\d{5}-\d{3}\$");

            return !Rgx.IsMatch(cep);
        }

        public static bool ApenasNumeros(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return false;

            foreach (char c in valor)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        public static bool ValidaCelular(string valor)
        {
            var Rgx = new Regex(@"[1-9]{2}[2-9][0-9]{8}$");
            return Rgx.IsMatch(valor);
        }

        public static bool ValidaTelefone(string valor)
        {
            var Rgx = new Regex(@"[1-9]{2}[2-9][0-9]{7}$");
            return Rgx.IsMatch(valor);
        }

        public static bool ValidaDataNascimento(DateTime? data)
        {
            if (data == null)
                return false;

            if (data > new DateTime(1900, 1, 1) && data < DateTime.Now.AddMonths(-6))
                return true;
            else
                return false;
        }

        public static bool ValidaTimeSpan24Hrs(TimeSpan? time)
        {
            if (time == null)
                return false;

            if (time <= new TimeSpan(24, 00, 00) && time > new TimeSpan(00, 00, 00))
                return true;
            else
                return false;
        }
    }
}
