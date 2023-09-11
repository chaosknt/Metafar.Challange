using Metafar.Challange.Data;
using Metafar.Challange.Data.Service.Managers.User;
using Metafar.Challange.Data.Service.Stores.Movements;
using Metafar.Challange.Data.Service.Stores.User;

namespace Metafar.Challange.Tests.Managers
{
    public class UserManagerTest
    {
        private UserManager userManager;
        private MetafarDbContext dbContext;
        private UserStore userStore;
        private UserMovementsStore userMovement;


        private void Init(string db)
        {
            this.dbContext = DbContextMocked.New(db);
            this.userStore = new UserStore(this.dbContext);
            this.userMovement = new UserMovementsStore(dbContext, this.userStore);
            this.userManager = new UserManager(this.userStore, this.userMovement);
        }

        [Fact]
        public async Task FindUserAsync_ReturnsExpectedUser()
        {
            Init("db1");
            // Arrange
            var creditCard = DbContextMocked.Card;
            var pin = DbContextMocked.PIN;
            var user = DbContextMocked.User;
           
            // Act
            var result = await userManager.FindUserAsync(creditCard, pin);

            // Assert
            Assert.True(result.WasSuccessfullyProcceded);
            Assert.True(result.Content.Name == user.Name);
        }

        [Fact]
        public async Task FindUserAsync_ReturnsNotFound()
        {
            // Arrange
                Init("db2");
                var creditCard = DbContextMocked.Card;
                var pin ="444";

            // Act
            var result = await userManager.FindUserAsync(creditCard, pin);

            // Assert
            Assert.False(result.WasSuccessfullyProcceded);
            Assert.True(result.Message.Equals("El numero de la tarjeta de credito y/o el PIN son incorrectos"));
        }

    }
}
