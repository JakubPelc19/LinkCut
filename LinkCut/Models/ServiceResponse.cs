namespace LinkCut.Models
{
    public class ServiceResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; } = false;

        public T? Data { get; set; }
    }
}
