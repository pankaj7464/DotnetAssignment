using DotnetAssignment1.Entities;

namespace DotnetAssignment1.Dto
{
    public class Response
    {
        public int StatusCode { get; set; } 
        public string? Message { get; set; } = null;
        public IotData? Data { get; set; } = null;
    }
}
