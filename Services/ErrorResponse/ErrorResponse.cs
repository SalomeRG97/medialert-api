namespace Services.ErrorResponse
{
    public class ErrorResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public string? details { get; set; }
    }
}
