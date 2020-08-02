namespace RicAuthServer.ViewModels
{
    public class BaseRestApiModel
    {
        public object Payload { get; set; }
        public object Errors { get; set; }
        public int StatusCode { get; set; }
    }
}
