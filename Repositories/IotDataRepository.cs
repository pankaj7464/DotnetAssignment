using DotnetAssignment1.Data;
using DotnetAssignment1.Dto;
using DotnetAssignment1.Entities;
using Microsoft.EntityFrameworkCore;
namespace DotnetAssignment1.Repositories
{
    public class IotDataRepository : IIotDataRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<IotDataRepository> _logger;

        public IotDataRepository(DataContext dataContext, ILogger<IotDataRepository> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        /// <summary>
        // Retrieves an IoT data record from the database based on the provided key asynchronously.
       
        public async Task<IotData> GetValueByKeyAsync(string key)
        {
            try
            {
                var keyValue = await _dataContext.IotDatas.FirstOrDefaultAsync(x => x.Key == key);
                if (keyValue != null)
                {
                    _logger.LogInformation("Successfully fetched value for key: {Key}", key);
                }
                else
                {
                    _logger.LogInformation("No value found for key: {Key}", key);
                }
                return keyValue;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching value by key: {Key}", key);
                // Re-throw the exception for higher-level error  handling
                throw;
            }
        }

        /// <summary>
        // Adds a new key-value pair to the database asynchronously.
    
        public async Task<Response> AddKeyValueAsync(AddKeyValue request)
        {
            try
            {
                var existingKey = await _dataContext.IotDatas.FindAsync(request.Key);
                if (existingKey != null)
                {
                    return new Response { Message = "Key already exists.", StatusCode = 409 };
                }

                var newKeyValue = new IotData
                {
                    Key = request.Key,
                    Value = request.Value
                };

                _dataContext.IotDatas.Add(newKeyValue);
                await _dataContext.SaveChangesAsync();

                _logger.LogInformation("Key-value pair added successfully. Key: {Key}, Value: {Value}", newKeyValue.Key, newKeyValue.Value);
                return new Response { Message = "Key-value pair added successfully.", StatusCode = 200, Data = newKeyValue };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding key-value pair");
                // Re-throw the exception for higher-level handling
                throw;
            }
        }


        /// <summary>
        // Update key-value pair to the database asynchronously.
       
        public async Task<Response> UpdateValueAsync(string key, string value)
        {
            try
            {
                var keyValue = await _dataContext.IotDatas.FindAsync(key);
                if (keyValue == null)
                {
                    return new Response { Message = "No value found corresponding to the key", StatusCode = 404 };
                }

                keyValue.Value = value;
                await _dataContext.SaveChangesAsync();

                _logger.LogInformation("Successfully updated value for key: {Key}. New value: {Value}", key, value);
                return new Response { Message = "Successfully updated", StatusCode = 200,Data = keyValue };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating value for key: {Key}", key);
                // Re-throw the exception for higher-level handling
                throw;
            }
        }

        /// <summary>
        // Delete key-value pair to the database asynchronously.
    
        public async Task<Response> DeleteKeyValueAsync(string key)
        {
            try
            {
                var keyValue = await _dataContext.IotDatas.FindAsync(key);
                if (keyValue == null)
                {
                    return new Response { Message = "Data not found", StatusCode = 404 };
                }

                _dataContext.IotDatas.Remove(keyValue);
                await _dataContext.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted key-value pair for key: {Key}", key);
                return new Response { Message = "Successfully deleted", StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting key-value pair for key: {Key}", key);
                // Re-throw the exception for higher-level handling
                throw;
            }
        }
    }
}
