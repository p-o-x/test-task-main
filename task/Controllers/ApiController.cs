using Microsoft.AspNetCore.Mvc;
using task.Models;
using task.Services;

namespace task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController(ILogger<ApiController> logger, SearchService searchService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            
            return Ok();
        }

        /// <summary>
        /// Поиск терминалов города по названию города и области.
        /// </summary>
        /// <param name="city">Город.</param>
        /// <param name="region">Регион.</param>
        /// <param name="stoppingToken">Токен отмены.</param>
        /// <returns>Список терминалов.</returns>
        [HttpGet("GetTerminalsByCityAndRegion")]
        [ProducesResponseType<Office>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> GetTerminalsByCityAndRegion([FromQuery] string city, [FromQuery] string region,
            CancellationToken stoppingToken = default)
        {
            logger.LogInformation($"Получен запрос на поиск терминалов с параметрами ${nameof(city)} = {city}, ${nameof(region)} = {region}");
            List<Office>? offices;
            try
            {
                offices = await searchService.GetTerminalsByCityAndRegion(city, region, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return StatusCode(499);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(504, "Timeout");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(offices);
        }

        /// <summary>
        /// Поиск идентификатора города по названию города и области.
        /// </summary>
        /// <param name="cityName">Название города.</param>
        /// <param name="region">Регион.</param>
        /// <param name="stoppingToken">Токен отмены.</param>
        /// <returns>Id города</returns>
        [HttpGet("GetCityIdByCityNameAndRegion")]
        [ProducesResponseType<Office>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> GetCityIdByCityNameAndRegion([FromQuery] string cityName, [FromQuery] string region, 
            CancellationToken stoppingToken = default)
        {
            logger.LogInformation($"Получен запрос на поиск Id города с параметрами ${nameof(cityName)} = {cityName}, ${nameof(region)} = {region}");
            int? id;
            try
            {
                id = await searchService.GetCityIdByCityNameAndRegion(cityName, region, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return StatusCode(499);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(504, "Timeout");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return id == null ? NotFound() : Ok(id);
        }
    }
}
