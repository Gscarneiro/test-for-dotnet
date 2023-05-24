using TestForDotNet.Repositories;
using TestForDotNet.Interfaces;
using TestForDotNet.Models;

namespace TestForDotNet.DataAccess
{
    public class ApiConnection
    {

        public static async Task CreateDatabase(DatabaseContext context)
        {
            try {
                var client = new HttpClient();

                var monsters = (await client.GetFromJsonAsync<MonsterResponseModel>("https://botw-compendium.herokuapp.com/api/v2/category/monsters"))?.data;

                if(monsters?.Any() ?? false)
                    context.AddRange(monsters.OrderBy(m => m.id));

                context.SaveChanges();
            } catch(HttpRequestException) {
                throw new Exception("Could not connect to Database.");
            }
        }
    }
}
