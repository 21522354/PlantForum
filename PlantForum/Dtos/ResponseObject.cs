using System.Net;

namespace PlantForum.Dtos
{
    public class ResponseObject
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }        
    }
}
