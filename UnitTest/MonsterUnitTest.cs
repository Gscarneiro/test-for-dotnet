using Microsoft.EntityFrameworkCore;
using TestForDotNet.DataAccess;
using TestForDotNet.Models;
using TestForDotNet.Repositories;

namespace UnitTest
{
    public class MonsterUnitTest
    {

        private readonly DbContextOptions<DatabaseContext> _contextOptions;

        public MonsterUnitTest()
        {
            _contextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase("BloggingControllerTest")
                .Options;

            using var context = new DatabaseContext(_contextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Monsters.AddRange(ApiConnection.CallMonsterApi().Result);

            context.SaveChanges();
        }

        [Fact]
        public async Task GetById_WithId100_ReturnsMonster()
        {
            //Arrange
            using var context = CreateContext();
            MonsterRepository monsterRepository = new(context);
            var id = 100;

            //Act
            var monster = (await monsterRepository.GetById(100));

            //Assert
            Assert.IsType<MonsterModel>(monster);
            Assert.Equal(id, monster.id);
        }

        [Fact]
        public async Task GetById_WithId997_ReturnsNull()
        {
            //Arrange
            using var context = CreateContext();
            MonsterRepository monsterRepository = new(context);
            var id = 997;

            //Act
            var monster = (await monsterRepository.GetById(id));

            //Assert
            Assert.IsNotType<MonsterModel>(monster);
        }

        [Fact]
        public async Task List_WhenCalled_ReturnsSameNameResults()
        {
            //Arrange
            using var context = CreateContext();
            MonsterRepository monsterRepository = new(context);

            //Act
            var monsterList = (await monsterRepository.List("chuchu"));

            //Assert
            Assert.Equal(4, monsterList.Count);
        }

        [Fact]
        public async Task Insert_WhenCalled_ReturnsMonster()
        {
            //Arrange
            using var context = CreateContext();
            MonsterRepository monsterRepository = new(context);
            var monster = new MonsterModel() { name = "Daniel Craig", description = "007", image = "https://i.imgur.com/c31gIaH.png" };

            //Act
            var result = await monsterRepository.Insert(monster);

            //Assert

            Assert.Equal(monster, context.Monsters.Find(result.id));
        }

        [Fact]
        public async Task Update_WithId121_ReturnsMonster()
        {
            //Arrange
            using var context = CreateContext();
            MonsterRepository monsterRepository = new(context);
            var monster = new MonsterModel() { id = 121, name = "lynel", description = "123987898989", image = "https://i.imgur.com/U0DSd8K.jpeg" };

            //Act
            var result = await monsterRepository.Update(monster.id, monster);

            //Assert

            Assert.Equal(monster.description, context.Monsters.Find(result.id)?.description);
        }

        [Fact]
        public async Task Update_WithId9998_ReturnsNull()
        {
            //Arrange
            using var context = CreateContext();
            MonsterRepository monsterRepository = new(context);
            var monster = new MonsterModel() { id = 9998, name = "lynel", description = "123987898989", image = "https://i.imgur.com/U0DSd8K.jpeg" };

            //Act
            var result = await monsterRepository.Update(monster.id, monster);

            //Assert

            Assert.IsNotType<MonsterModel>(result);
        }

        [Fact]
        public async Task Delete_WithId101_ReturnsTrue()
        {
            //Arrange
            using var context = CreateContext();
            MonsterRepository monsterRepository = new(context);
            var id = 101;

            //Act
            var deleted = (await monsterRepository.Delete(id));

            //Assert
            Assert.True(deleted);
        }

        [Fact]
        public async Task Delete_WithId999_ReturnsFalse()
        {
            //Arrange
            using var context = CreateContext();
            MonsterRepository monsterRepository = new(context);
            var id = 999;

            //Act
            var deleted = (await monsterRepository.Delete(id));

            //Assert
            Assert.False(deleted);
        }

        private DatabaseContext CreateContext()
        {
            return new DatabaseContext(_contextOptions, (context, modelBuilder) => {
                modelBuilder.Entity<MonsterModel>()
                    .ToInMemoryQuery(() => context.Monsters.Select(b => b));
            });
        }
    }
}