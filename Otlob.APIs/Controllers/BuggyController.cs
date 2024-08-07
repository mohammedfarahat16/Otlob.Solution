using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otlob.APIs.Errors;
using Otlob.Repository.Data;

namespace Otlob.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : APIBaseController
    {
        private readonly StoreContext dbcontext;

        public BuggyController(StoreContext _dbcontext)
        {
            dbcontext = _dbcontext;
        }
        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
            var Product = dbcontext.Products.Find(100);

            if(Product is null) return NotFound(new ApiResponse(404));

            return Ok(Product);
        }


        [HttpGet("ServerError")]
        public ActionResult GetServerError(int id)
        {
            var Product = dbcontext.Products.Find(100);
            var ProductToReturn = Product.ToString();

            return Ok(ProductToReturn);
        }

        [HttpGet("BasRequest")]
        public ActionResult GetError(int id) 
        {
            return BadRequest();

        }

        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest (int id)
        {
            return Ok();

        }

    }
}
