using DotnetAssignment1.Dto;
using DotnetAssignment1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAssignment1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IotDataController : ControllerBase
    {
        private readonly IIotDataRepository _iotDataRepository;
        private readonly ILogger<IotDataController> _logger;

        public IotDataController(IIotDataRepository iotDataRepository, ILogger<IotDataController> logger)
        {
            _iotDataRepository = iotDataRepository;
            _logger = logger;
        }

        /// <summary>
        // Retrieves the value associated with the specified key from the IoT data repository.
        [HttpGet("/keys/{key}")]
        public async Task<ActionResult<Response>> GetValueByKey(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return StatusCode(400,new Response { Message = "Enter valid key", StatusCode=400 });
                }

                var keyValue = await _iotDataRepository.GetValueByKeyAsync(key);

                if (keyValue == null)
                {
                    return StatusCode(404,new Response { Message = "No value exists for the given key",StatusCode = 404 });
                }

                return StatusCode(200,new Response { Message = "Successfully fetched value for the corresponding key", StatusCode = 200,Data = keyValue });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting value by key: {Key}", key);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "Internal server error",StatusCode = 500 });
            }
        }


        /// <summary>
        // Adds a new key-value pair to the IoT data repository.
        [HttpPost("/keys")]
        public async Task<ActionResult<Response>> AddKeyValue([FromBody] AddKeyValue request)
        {
            try
            {
                if (request == null)
                {
                    return StatusCode(400,new Response { Message = "Invalid data. Please enter correct data", StatusCode = 400 });
                }

                var response = await _iotDataRepository.AddKeyValueAsync(request);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding key-value pair");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "Internal server error" });
            }
        }

        /// <summary>
        // Update Key with new value  to the IoT data repository.
        [HttpPatch("/keys/{key}/{value}")]
        public async Task<ActionResult<Response>> UpdateValue(string key, string value)
        {
            try
            {
                if (key == null || value == null)
                {
                    return StatusCode(400, new Response { StatusCode = 400, Message = "Invalid data. Please enter correct data" });
                }
                var response = await _iotDataRepository.UpdateValueAsync(key, value);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating value for key: {Key}", key);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "Internal server error" });
            }
        }




        /// <summary>
        // Delete key-value pair to the IoT data repository.
        [HttpDelete("/keys/{key}")]
        public async Task<ActionResult<Response>> DeleteKeyValue(string key)
        {
            try
            {

                if (key == null )
                {
                    return StatusCode(400, new Response { StatusCode = 400, Message = "Invalid data. Please enter correct data" });
                }
                var response = await _iotDataRepository.DeleteKeyValueAsync(key);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting key-value pair for key: {Key}", key);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "Internal server error" });
            }
        }
    }
}
