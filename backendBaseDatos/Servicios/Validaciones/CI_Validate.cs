namespace backendBaseDatos.Servicios.Validaciones
{
    public static class CI_Validate
    {
        public static bool IsCIValid(string ci)
        {
            if (ci.Length != 7 && ci.Length != 8)
            {
                return false;
            }
            else
            {
                try
                {
                    int.Parse(ci);
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            int digVerificador = int.Parse(ci[ci.Length - 1].ToString());
            int[] factores;

            if (ci.Length == 7) // CI viejas
            {
                factores = new int[] { 9, 8, 7, 6, 3, 4 };
            }
            else
            {
                factores = new int[] { 2, 9, 8, 7, 6, 3, 4 };
            }

            int suma = 0;
            for (int i = 0; i < ci.Length - 1; i++)
            {
                int digito = int.Parse(ci[i].ToString());
                suma += digito * factores[i];
            }

            int resto = suma % 10;
            int checkdigit = 10 - resto;

            if (checkdigit == 10)
            {
                return digVerificador == 0;
            }
            else
            {
                return checkdigit == digVerificador;
            }
        }
    }
}
