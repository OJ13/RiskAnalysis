using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RiskAnalisys.Application.DTO.Requests;
using RiskAnalisys.Application.Services;
using System.Diagnostics;

namespace RiskAnalisys.API.Controllers
{
    [Route("api/risk-analisys")]
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

        /// <summary>
        /// Endopoint para classificar o risco de um conjunto de operações comerciais.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Retorna o resultado da classificação de risco</returns>
        [HttpPost("classify")]
        public async Task<IActionResult> Classify(ClassifyRiskRequestDTO[] request)
        {
            var sw = Stopwatch.StartNew();

            if (request is null || request.Length == 0)
                return BadRequest("Requisição Inválida - dados não encontrados");


            var response = _riskAnalisysService.ClassifyRisk(request);

            sw.Stop();

            var result = response with
            {
                ProcessingTimeMS = sw.ElapsedMilliseconds
            };

            ActivityTelemetry(request, result);

            return Ok(result);
        }

        /// <summary>
        /// Endpoint para trazer resumo de operações comerciais por distribuição de risco.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Retorna o resultado da distribuição de risco</returns>
        [HttpPost("distribution")]
        public async Task<IActionResult> Distribution(DistributionRiskRequestDTO[] request)
        {
            var sw = Stopwatch.StartNew();

            if (request is null || request.Length == 0)
                return BadRequest("Requisição Inválida - dados não encontrados");

            var response = _riskAnalisysService.DistributionRisk(request);

            sw.Stop();

            var result = response with
            {
                ProcessingTimeMS = sw.ElapsedMilliseconds
            };

            ActivityTelemetry(request, result);

            return Ok(result);
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
