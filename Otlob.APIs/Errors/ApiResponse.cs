

namespace Otlob.APIs.Errors
{

    public class ApiResponse 
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(StatusCode);  
        }

        private string? GetDefaultMessageForStatusCode(int? statusCode)
        {
            return statusCode switch
            {
                400=>"bad Request",
                401=>"You are not authorized",
                404=>"Resources Not Found",
                500=> "Internal Server Error",

                 _ => null
                ,
            }; 


        }
    }
}
