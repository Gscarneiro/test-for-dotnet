using System.Reflection;
using TestForDotNet.DataAccess;
using TestForDotNet.Interfaces;
using TestForDotNet.Models;

namespace TestForDotNet.Repositories
{
    public class MonsterRepository : IMonsterRepository
    {

        private readonly DatabaseContext _context;

        public MonsterRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<MonsterModel> GetById(int id)
        {
            await EnsureDatabaseCreated();

            return _context.Monsters.FirstOrDefault(m => m.id == id);
        }

        public async Task<List<MonsterModel>> List(string? name = null)
        {
            await EnsureDatabaseCreated();

            var monsters = _context.Monsters.AsQueryable();

            if(!String.IsNullOrWhiteSpace(name))
                monsters = monsters.Where(m => m.name.Contains(name));

            return monsters.ToList();
        }

        public async Task<MonsterModel> Insert(MonsterModel model)
        {
            await EnsureDatabaseCreated();

            _context.Add(model);

            _context.SaveChanges();

            return model;
        }


        public async Task<MonsterModel> Update(int id, MonsterModel model)
        {
            await EnsureDatabaseCreated();

            var monster = await GetById(id);

            if(monster != null) {
                UpdateValues(model, monster);

                _context.SaveChanges();
            }

            return monster;
        }

        public async Task<bool> Delete(int id)
        {
            var monster = await GetById(id);
            var deleted = false;
            if(monster != null) {
                _context.Monsters.Remove(monster);
                _context.SaveChanges();

                deleted = true;
            }

            return deleted;
        }


        private MonsterModel UpdateValues(MonsterModel modelBase, MonsterModel modelDestiny)
        {
            var properties = modelBase.GetType().GetProperties().Where(p => p.Name != "id");

            foreach(PropertyInfo property in properties) {
                var value = property.GetValue(modelBase);
                PropertyInfo? destinyProperty = modelDestiny.GetType().GetProperty(property.Name);
                if(destinyProperty != null)
                    destinyProperty.SetValue(modelDestiny, value);
            }

            return modelDestiny;
        }


        private async Task EnsureDatabaseCreated()
        {
            if(!_context.Monsters.Any()) {
                var monsters = await ApiConnection.CallMonsterApi();

                if(monsters?.Any() ?? false)
                    _context.AddRange(monsters.OrderBy(m => m.id));

                _context.SaveChanges();
            }
        }
    }
}
