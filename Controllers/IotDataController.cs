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

        [HttpGet("/keys/{key}")]
        public async Task<ActionResult<Response>> GetValueByKey(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return BadRequest(new Response { Message = "Enter valid key" });
                }

                var keyValue = await _iotDataRepository.GetValueByKeyAsync(key);

                if (keyValue == null)
                {
                    return NotFound(new Response { Message = "No value exists for the given key" });
                }

                return Ok(new Response { Message = "Successfully fetched value for the corresponding key", Data = keyValue });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting value by key: {Key}", key);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "Internal server error" });
            }
        }

        [HttpPost("/keys")]
        public async Task<ActionResult<Response>> AddKeyValue([FromBody] AddKeyValue request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new Response { Message = "Invalid data. Please enter correct data", StatusCode = 400 });
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

        [HttpPatch("/keys/{key}/{value}")]
        public async Task<ActionResult<Response>> UpdateValue(string key, string value)
        {
            try
            {
                var response = await _iotDataRepository.UpdateValueAsync(key, value);

                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating value for key: {Key}", key);
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Message = "Internal server error" });
            }
        }

        [HttpDelete("/keys/{key}")]
        public async Task<ActionResult<Response>> DeleteKeyValue(string key)
        {
            try
            {
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
