namespace webShop2.midlleware
{
    public class AppException : Exception
    {
        public int StatusCode { get; }

        public AppException(int statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
