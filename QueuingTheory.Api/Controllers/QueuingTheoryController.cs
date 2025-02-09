using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QueuingTheory.Api.Models;
using QueuingTheory.Api.Services;

namespace QueuingTheory.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueuingTheoryController : ControllerBase
    {
        private readonly QueuingTheoryService _service;

        public QueuingTheoryController()
        {
            _service = new QueuingTheoryService();
        }

        [HttpPost("mm1")]
        public ActionResult<QueueingResults> CalculateMM1([FromBody] QueueingModel model)
        {
            try
            {
                return Ok(_service.CalculateMM1(model.ArrivalRate, model.ServiceRate));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("mmc")]
        public ActionResult<QueueingResults> CalculateMMC([FromBody] QueueingModel model)
        {
            try
            {
                return Ok(_service.CalculateMMC(model.ArrivalRate, model.ServiceRate, model.Servers));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
