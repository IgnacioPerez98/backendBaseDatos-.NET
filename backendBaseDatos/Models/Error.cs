namespace backendBaseDatos.Models;

public class Error:Exception
{
    public string Mensaje { get; set; }
    public int ErrorCode { get; set; }

    public Error()
    {
        
    }
    public Error(int code, string msg)
    {
        ErrorCode = code;
        Mensaje = msg;
    }
    
}