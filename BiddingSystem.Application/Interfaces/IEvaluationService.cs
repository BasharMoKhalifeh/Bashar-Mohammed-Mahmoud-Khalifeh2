using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Application.Interfaces
{
    public interface IEvaluationService
    {
        Task<bool> AddEvaluationCriteriaAsync(AddCriteriaDto dto, int userId);
        Task<bool> EvaluateBidAsync(EvaluateBidDto dto, int evaluatorId);
        Task<bool> AwardTenderAsync(AwardTenderDto dto, int userId);
        Task<EvaluationSummaryDto> GetEvaluationSummaryAsync(int tenderId);
    }
}
