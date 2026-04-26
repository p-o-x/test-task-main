using Microsoft.EntityFrameworkCore;
using task.Database;
using task.Models;

namespace task.Services
{
    public class SearchService(DellinDictionaryDbContext db, ILogger<ImportService> logger)
    {
        public async Task<List<Office>> GetTerminalsByCityAndRegion(string city, string region,
            CancellationToken stoppingToken = default)
        {
            logger.LogInformation("Поиск терминалов по городу и региону");

            var offices = await db.Offices.Where(office => (office.AddressCity ?? "").Contains(city) 
                && (office.AddressRegion ?? "").Contains(region)).Include(office => office.Phones).ToListAsync(stoppingToken);

            logger.LogInformation($"Найдено {offices.Count} терминалов");

            return offices;
        }

        public async Task<int?> GetCityIdByCityNameAndRegion(string cityName, string region,
            CancellationToken stoppingToken = default)
        {
            logger.LogInformation("Поиск терминалов по городу и региону");

            var cityCode = (await db.Offices.FirstOrDefaultAsync(office => (office.AddressCity ?? "").Contains(cityName)
                && (office.AddressRegion ?? "").Contains(region), stoppingToken))?.CityCode;
            if (cityCode != null)
            {
                logger.LogInformation($"Найдет город с кодом {cityCode}");
            }
            else
            {
                logger.LogInformation($"Город {cityName} в регионе {region} не найден");
            }

            return cityCode;
        }
    }
}
