using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using task.Database;
using task.Models;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace task.Services
{
    public class ImportService(
        DellinDictionaryDbContext db, 
        ILogger<ImportService> logger)
    {
        private static readonly string[] FederailCities =
        [
            "Москва",
            "Санкт-Петербург",
            "Севастополь"
        ];

        private static readonly JsonSerializerOptions jsonSerializerOptions = new() 
        {
            PropertyNameCaseInsensitive = true 
        };

        public async Task ImportDataToDb(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Удаление старых записей");
            await db.Offices.ExecuteDeleteAsync(cancellationToken);
            logger.LogInformation("Начато получение данных");

            var cities = await FetchResults(cancellationToken);

            logger.LogInformation($"Получена информация по {cities.Count} городам");

            var offices = ConvertFetchedData(cities);

            logger.LogInformation($"Информация приведена в нужный формат, получено {offices.Count} офисов");

            await db.Offices.AddRangeAsync(offices, cancellationToken);

            var count = await db.SaveChangesAsync(cancellationToken);

            logger.LogInformation($"В бд добавлено {count} сущностей");
        }

        private async Task<List<City>> FetchResults(CancellationToken cancellationToken = default)
        {
            var json = await File.ReadAllTextAsync("files/terminals.json", cancellationToken);
            var data = JsonSerializer.Deserialize<Rootobject>(json, jsonSerializerOptions);

            if (data == null)
            {
                return [];
            }

            return [.. data.city];
        }

        private List<Office> ConvertFetchedData(List<City> cities)
        {
            var offices = new List<Office>();

            foreach (var city in cities)
            {
                offices.AddRange(city.terminals.terminal.Select(terminal =>
                {
                    var id = int.Parse(terminal.id);

                    var office = new Office
                    {
                        Id = id,
                        Code = terminal.addressCode?.place_code,
                        CityCode = int.Parse(city.id),
                        Uuid = Guid.NewGuid().ToString(),
                        Type = terminal.storage ? OfficeType.WAREHOUSE : terminal.isPVZ ? OfficeType.PVZ : null,
                        CountryCode = "ru",
                        Coordinates = new Coordinates
                        {
                            Latitude = double.Parse(terminal.latitude, CultureInfo.InvariantCulture),
                            Longitude = double.Parse(terminal.longitude, CultureInfo.InvariantCulture),
                        },
                        AddressApartment = 0,
                        WorkTime = terminal.worktables.worktable.Aggregate(new StringBuilder(), 
                            (stringBuilder, worktable) => stringBuilder.Append(worktable.timetable +";"),
                            stringBuilder => (stringBuilder.Length > 0) ? stringBuilder.ToString(0, stringBuilder.Length - 1): ""),
                        Phones = terminal.phones.Select(phone => new Phone
                        { 
                            OfficeId = id,
                            PhoneNumber = phone.number,
                            Additional = phone.primary ? "Нет" : "Да",
                        }).FirstOrDefault()
                    };

                    SetAddress(office, terminal.fullAddress);

                    return office;
                }));
            }

            return offices;
        }

        private void SetAddress(Office office, string address)
        {
            var cityRegex = new Regex(@"[а-яА-я]+ (г|д|с)");
            var city = cityRegex.Match(address).Value;
            var streetRegex = new Regex(@"[0-9а-яА-я-]+ [0-9а-яА-я- \.\)\(]*(ул|тракт|пр-кт|проезд|ш|пер)");
            var street = streetRegex.Match(address).Value;
            var houseRegex = new Regex(@"(дом №.+|стр .+)");
            var house = houseRegex.Match(address).Value;

            office.AddressCity = city;
            office.AddressStreet = street;
            office.AddressHouseNumber = house;

            if (FederailCities.Any(city => address.Contains(city)))
            {
                office.AddressRegion = city;
                return;
            }

            var regionRegex = new Regex(@"[а-яА-я -]+(обл|АО|край|Респ)(?=,)");

            office.AddressRegion = regionRegex.Match(address).Value;
        }
    }
}
