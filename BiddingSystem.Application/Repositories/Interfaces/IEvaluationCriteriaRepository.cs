using BiddingSystem.Application.Repositories.Interfaces;

public interface IEvaluationCriteriaRepository : IBaseRepository<EvaluationCriteria>
{
    Task<IReadOnlyList<EvaluationCriteria>> GetCriteriaByTenderAsync(Guid tenderId);
    Task<bool> AddCriteriaRangeAsync(IEnumerable<EvaluationCriteria> criteria);
    Task<bool> DeleteCriteriaByTenderAsync(Guid tenderId);
    Task<decimal> GetTotalWeightByTenderAsync(Guid tenderId);
    Task<IReadOnlyList<EvaluationCriteria>> GetStandardCriteriaTemplatesAsync();
    Task<bool> ApplyCriteriaTemplateAsync(Guid tenderId, Guid templateId);
}
