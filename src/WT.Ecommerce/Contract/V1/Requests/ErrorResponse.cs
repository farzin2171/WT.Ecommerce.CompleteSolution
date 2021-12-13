using System.Collections.Generic;

namespace WT.Ecommerce.Contract.V1.Requests
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
