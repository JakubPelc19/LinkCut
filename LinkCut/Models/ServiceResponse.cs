namespace LinkCut.Models
{
    public class ServiceResponse<T>
    {
        public string Message { get; set; } = string.Empty;

        public int StatusCode { get; set; }
        public bool IsSuccessful { get; set; } = false;

        public T? Data { get; set; }
    }
}
