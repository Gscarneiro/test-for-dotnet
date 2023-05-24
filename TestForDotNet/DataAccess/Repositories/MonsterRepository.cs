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

        public async Task<MonsterModel> Get(int id)
        {
            await EnsureDatabaseCreated();

            var model = _context.Monsters.FirstOrDefault(m => m.id == id);

            if(model == null)
                throw new Exception($"Monster with id: {id} was not found.");

            return model;
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

            var dbMonster = await Get(id);

            UpdateValues(model, dbMonster);

            _context.SaveChanges();

            return dbMonster;
        }

        public async Task Delete(int id)
        {
            var monster = await Get(id);
            _context.Monsters.Remove(monster);
            _context.SaveChanges();
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
            if(!_context.Monsters.Any())
                await ApiConnection.CreateDatabase(_context);
        }
    }
}
