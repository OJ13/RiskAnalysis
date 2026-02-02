using RiskAnalisys.Application.DTO.Requests;
using RiskAnalisys.Application.DTO.Responses;

namespace RiskAnalisys.Application.Services;

public interface IRiskAnalisysService
{
    ClassifyRiskDTO ClassifyRisk(ClassifyRiskRequestDTO[] request);

    DistribuitionCalculatedDTO DistributionRisk(DistributionRiskRequestDTO[] request);
}
