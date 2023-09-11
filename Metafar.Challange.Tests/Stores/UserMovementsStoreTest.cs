using Metafar.Challange.Data;
using Metafar.Challange.Data.Service.Stores.Movements;
using Metafar.Challange.Data.Service.Stores.User;
using Moq;

namespace Metafar.Challange.Tests.Stores
{
    public class UserMovementsStoreTest
    {
        private  Mock<UserStore> userStore;
        private  MetafarDbContext dbContext;
        private  UserMovementsStore userMovement;

        [Fact]
        public async Task GetAll_ReturnExpected()
        {
            // arrange
            this.Init("test");
            var expected = DbContextMocked.Movements;

            // act

                var actual = (await this.userMovement.GetAllAsync()).ToList();

            // assert

            Assert.NotNull(actual);
            Assert.Equal(expected.FirstOrDefault().AccountMovementId, actual.FirstOrDefault().AccountMovementId);
        }

        [Fact]
        public async Task Withdrawal_ValidId_ReturnEntity()
        {
            // arrange
            this.Init("test2");
            var expected = DbContextMocked.Userid;
            // act

            var actual = await this.userMovement.Withdrawal(DbContextMocked.Userid, 10m);

            // assert

            Assert.NotNull(actual);
            Assert.Equal(expected, actual.UserId);
        }

        [Fact]
        public async Task Withdrawal_NonValidId_ReturnNull()
        {
            // arrange
            this.Init("test3");
            Guid? expected = null;
            // act

            var actual = await this.userMovement.Withdrawal(Guid.NewGuid(), 10m);

            // assert

            Assert.Null(actual);
        }

        private void Init(string db)
        {
            this.dbContext = DbContextMocked.New(db);
            this.userStore = new Mock<UserStore>(this.dbContext);
            this.userMovement = new UserMovementsStore(this.dbContext, this.userStore.Object);
        }
    }
}
