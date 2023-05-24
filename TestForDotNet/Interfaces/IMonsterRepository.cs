using TestForDotNet.Models;

namespace TestForDotNet.Interfaces
{
    public interface IMonsterRepository
    {
        Task<MonsterModel> Get(int id);
        Task<List<MonsterModel>> List(string? name = null);
        Task<MonsterModel> Insert(MonsterModel model);
        Task<MonsterModel> Update(int id, MonsterModel model);
        Task Delete(int id);
    }
}
