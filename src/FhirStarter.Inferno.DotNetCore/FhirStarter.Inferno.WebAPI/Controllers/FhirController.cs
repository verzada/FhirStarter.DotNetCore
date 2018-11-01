using FhirStarter.Inferno.WebAPI.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FhirStarter.Inferno.WebAPI.Controllers
{
    [Route("fhir"), EnableCors]
    public class FhirController : Controller
    {
        public FhirController()
        {}

        [HttpGet, Route("{type}")]
        public void Read(string type)
        {
            var t = 2;
            var searchParams = Request.GetSearchParams();

            var test = searchParams;

            //var service = _handler.FindServiceFromList(_fhirServices, _fhirMockupServices, type);
            //var parameters = Request.GetSearchParams();
            //if (!(parameters.Parameters.Count > 0)) return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            //var results = service.Read(parameters);
            //return SendResponse(results);
        }

        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
