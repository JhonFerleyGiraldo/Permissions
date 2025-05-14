
namespace Application.Commons
{
    public class Response<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Operación exitosa";
        public T? Data { get; set; } = default;

        public Response() { }
        public Response(T? data, bool success = true,string message = "Operación exitosa") 
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }

}
