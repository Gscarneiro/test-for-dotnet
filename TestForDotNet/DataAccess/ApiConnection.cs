﻿using TestForDotNet.Repositories;
using TestForDotNet.Interfaces;
using TestForDotNet.Models;

namespace TestForDotNet.DataAccess
{
    public class ApiConnection
    {

        public static async Task<List<MonsterModel>> CallMonsterApi()
        {
            try {
                var client = new HttpClient();

                return (await client.GetFromJsonAsync<MonsterResponseModel>("https://botw-compendium.herokuapp.com/api/v2/category/monsters"))?.data;
            } catch(HttpRequestException) {
                throw new Exception("Could not connect to API.");
            }
        }
    }
}
