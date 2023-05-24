using Microsoft.AspNetCore.Mvc;
using TestForDotNet.Interfaces;
using TestForDotNet.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestForDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonsterController : ControllerBase
    {
        private readonly IMonsterRepository monsterRepository;

        public MonsterController(IMonsterRepository monster)
        {
            monsterRepository = monster;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var monster = await monsterRepository.GetById(id);

            if(monster != null)
                return Ok(monster);
            else
                return NotFound($"Monster with id: {id} was not found.");
        }

        [HttpGet]
        public async Task<List<MonsterModel>> List(string? name = null)
        {
            return await monsterRepository.List(name);
        }

        [HttpPost]
        public async Task<MonsterModel> Post([FromBody] MonsterModel model)
        {
            return await monsterRepository.Insert(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MonsterModel model)
        {
            var monster = await monsterRepository.Update(id, model);

            if(monster != null)
                return Ok(monster);
            else
                return NotFound($"Monster with id: {id} was not found.");
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await monsterRepository.Delete(id);
        }
    }
}
