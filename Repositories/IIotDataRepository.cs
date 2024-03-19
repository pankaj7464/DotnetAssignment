using DotnetAssignment1.Dto;
using DotnetAssignment1.Entities;

namespace DotnetAssignment1.Repositories
{
    public interface IIotDataRepository
    {
        Task<IotData> GetValueByKeyAsync(string key);
        Task<Response> AddKeyValueAsync(AddKeyValue request);
        Task<Response> UpdateValueAsync(string key, string value);
        Task<Response> DeleteKeyValueAsync(string key);
    }
}
