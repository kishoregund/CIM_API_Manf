namespace Application.Features.Masters
{
    public interface IConfigTypeValuesService
    {
        Task<ConfigTypeValues> GetConfigTypeValueAsync(Guid id);
        Task<List<ConfigTypeValues>> GetConfigTypeValuesByTypeIdAsync(Guid configTypeId);
        Task<Guid> CreateConfigTypeValuesAsync(ConfigTypeValues configTypeValues);
        Task<Guid> UpdateConfigTypeValuesAsync(ConfigTypeValues configTypeValues);
        Task<bool> DeleteConfigTypeValuesAsync(Guid id);
    }
}
