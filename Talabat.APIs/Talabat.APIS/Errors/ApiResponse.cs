
namespace Talabat.APIS.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statuscode,string? message = null)
        {
            StatusCode = statuscode;
            Message = message??GetDefualtMessageForStatusCode(statuscode);
        }

        private string? GetDefualtMessageForStatusCode(int statuscode)
        {
            return statuscode switch
            {
                400 => "BadRequest",
                401 => "you are not authorized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
