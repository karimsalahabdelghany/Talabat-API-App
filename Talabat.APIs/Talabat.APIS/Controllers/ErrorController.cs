﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;

namespace Talabat.APIS.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]     //to ignore documentation in swagar
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error (int code)
         {
            return NotFound(new ApiResponse(code));
        }
    }
}
