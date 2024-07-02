using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;
using Talabat.Repositiory.Data;

namespace Talabat.APIS.Controllers
{
    
    public class BuggyController : APIBaseController
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        [HttpGet("NotFound")]   
        public ActionResult GetNotFoundRequest()
        {
           var product = _dbcontext.Products.Find(100);
            if(product is null) return NotFound(new ApiResponse(404));
            else
                return Ok(product);
        }
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var Product = _dbcontext.Products.Find(100);    
            var producttoreturn =Product.ToString();  //will throw Null Reference Exception
            return Ok(producttoreturn);
        }
        [HttpGet("BadRequest")]
        
        public ActionResult GetBadRequest() 
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("BadRequest/{id}")]     //validation error
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }
    }
}
