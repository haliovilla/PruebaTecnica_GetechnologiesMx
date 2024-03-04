using EDM.Entities;
using Microsoft.Extensions.Logging;

namespace DAC
{
    public class SeedData
    {
        private static List<string> names = new List<string> { "Juan", "María", "José", "Ana", "Carlos", "Laura" };
        private static List<string> lastnames = new List<string> { "García", "Martínez", "Hernández", "López", "Pérez", "González" };

        public static async Task InsertSeedDataAsync(AppDbContext dbContext, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<SeedData>();

            var people = GeneratePeopleList();
            dbContext.People.AddRange(people);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Se insertaron {people.Count} personas");

            var invoices = GenerateInvoicesList(people);
            dbContext.Invoices.AddRange(invoices);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"Se insertaron {invoices.Count()} facturas");
        }

        private static IEnumerable<Invoice> GenerateInvoicesList(List<Person> people)
        {
            var rand = new Random();

            foreach (var person in people)
            {
                var qty = rand.Next(1, 10);
                for (int i = 0; i < qty; i++)
                {
                    var minutes = rand.Next(1, 10080) * -1;
                    decimal amount = rand.Next(1000, 10000);

                    yield return new Invoice
                    {
                        PersonId = person.Id,
                        Date = DateTime.Now.AddMinutes(minutes),
                        Amount = amount,
                        Deleted = false
                    };
                }
            }
        }

        private static List<Person> GeneratePeopleList(int qty = 50)
        {
            var result = new List<Person>();

            var fullNames = GenerateRandomNamesList(qty);
            var identifications = GenerateRandomIdentificationsList(qty);

            for (int i = 0; i < qty; i++)
            {
                result.Add(new Person
                {
                    FirstName = GetFirstName(fullNames[i]),
                    PaternalLastname = GetPaternalLastname(fullNames[i]),
                    MaternalLastname = GetMaternalLastname(fullNames[i]),
                    Identification = identifications[i],
                    Deleted = false
                });
            }

            return result;
        }

        private static string GetMaternalLastname(string fullname)
        {
            var split = fullname.Split(" ");

            if (split.Length >= 2)
                return split[2];

            return null;
        }

        private static string GetPaternalLastname(string fullname)
        {
            var split = fullname.Split(" ");

            if (split.Length >= 2)
                return split[1];

            return "";
        }

        private static string GetFirstName(string fullname)
        {
            var split = fullname.Split(" ");

            if (split.Length > 0)
                return split[0];

            return "";
        }

        private static string GenerateRandomFullname()
        {
            var rand = new Random();
            string name = names[rand.Next(names.Count)];
            string paternalLastname = lastnames[rand.Next(lastnames.Count)];
            string maternalLastname = lastnames[rand.Next(lastnames.Count)];

            return $"{name} {paternalLastname} {maternalLastname}".Trim();
        }

        private static List<string> GenerateRandomNamesList(int qty = 25)
        {
            var result = new List<string>();

            while (result.Count < qty)
            {
                var name = GenerateRandomFullname();
                if (!result.Contains(name))
                    result.Add(name);
            }

            return result;
        }

        private static List<string> GenerateRandomIdentificationsList(int qty = 25)
        {
            var result = new List<string>();

            while (result.Count < qty)
            {
                var identification = GenerateRandomIdentification();
                if (!result.Contains(identification))
                    result.Add(identification);
            }

            return result;
        }

        private static string GenerateRandomIdentification()
        {
            var rand = new Random();
            string identification = "";
            for (int i = 0; i < 13; i++)
                identification += rand.Next(10).ToString();

            return identification;
        }
    }
}
