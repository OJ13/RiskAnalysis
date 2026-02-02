using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RiskAnalisys.Application.DTO.Requests;
using RiskAnalisys.Application.Services;
using System.Diagnostics;

namespace RiskAnalisys.API.Controllers
{
    [Route("risk-analisys")]
    [ApiController]
    public class RiskAnalisysController : ControllerBase
    {
        private readonly IRiskAnalisysService _riskAnalisysService;
        private readonly ILogger<RiskAnalisysController> _logger;
        public RiskAnalisysController(IRiskAnalisysService riskAnalisysService, ILogger<RiskAnalisysController> logger)
        {
            _riskAnalisysService = riskAnalisysService;
            _logger = logger;
        }

        [HttpPost("classify")]
        public async Task<IActionResult> Classify(ClassifyRiskRequestDTO[] request)
        {
            if (request is null || request.Length == 0)
                return BadRequest("Requisição Inválida - dados não encontrados");


            var response = _riskAnalisysService.ClassifyRisk(request);

            ActivityTelemetry(request, response);

            return Ok(response);
        }

        [HttpPost("distribution")]
        public async Task<IActionResult> Distribution(DistributionRiskRequestDTO[] request)
        {
            if (request is null || request.Length == 0)
                return BadRequest("Requisição Inválida - dados não encontrados");

            var response = _riskAnalisysService.DistributionRisk(request);

            ActivityTelemetry(request, response);

            return Ok(response);
        }

        private void ActivityTelemetry<T>(T payload, object response)
        {
            var activity = Activity.Current;
            if (activity != null)
            {
                activity.SetTag("app.request.headers", GetHeaders());
                if (payload is Array arr)
                    activity.SetTag("app.request.count", arr.Length);

                //activity.SetTag("app.request", payload);
                activity.SetTag("app.response", response);
            }
        }

        private string GetHeaders()
        {
            return JsonConvert.SerializeObject(HttpContext.Request.Headers, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
