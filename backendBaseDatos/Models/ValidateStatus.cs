namespace backendBaseDatos.Models
{
    public class ValidateStatus
    {
        public ValidateStatus()
        {
        }

        public ValidateStatus(bool isOK, string message)
        {
            IsOK = isOK;
            Message = message;
        }

        public bool IsOK { get; set; } = true;
        public string Message { get; set; } = "Correcto";


    }
}
