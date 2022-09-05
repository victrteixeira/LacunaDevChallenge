using Bogus;
using Bogus.DataSets;
using Lacuna.Domain.Validation;
using Microsoft.EntityFrameworkCore;

namespace Lacuna.InfrastructureTests;

public class RepositoryTests
{
    private readonly IUserRepository _sut;
    private readonly Mock<LacunaContext> _lacunaContextMock;
    private readonly Mock<DbSet<User>> dbSetMock;
    private List<User> Users { get; set; }
    public RepositoryTests()
    {
        _lacunaContextMock = new Mock<LacunaContext>();
        _sut = new UserRepository(_lacunaContextMock.Object);
        Users = DbFixture.UserPopulate();

        dbSetMock = Users.AsQueryable().BuildMockDbSet();
        _lacunaContextMock.Setup(x => x.LacunaUsers)
            .Returns(dbSetMock.Object);
    }

    [Fact]
    public async Task Repository_GetEmail_ShouldReturnAnEntity()
    {
        // Arrange
        var user = new User(new Randomizer().ClampString(new Name().FirstName(), 5, 31), "sampleemail@email.com", new Internet().Password(12));
        Users.Add(user);
        
        // Act
        var res = await _sut.GetEmailAsync("sampleemail@email.com");
        
        // Assert
        res.Should().Be(user);
    }

    [Fact]
    public async Task Repository_CreateValidUser_ShouldAddAnEntityInDb()
    {
        // Arrange
        var newUser = DbFixture.DbValidEntity();

        // Act
        var res = await _sut.CreateUserAsync(newUser);

        // Assert
        dbSetMock.Verify(x => x.Add(newUser), Times.Exactly(1));
        _lacunaContextMock.Verify(x => x.SaveChangesAsync(default), Times.Exactly(1));
        res.Should().BeEquivalentTo(newUser);
    }

    [Fact]
    public async Task Repository_DeleteAnUser_ShouldRemoveAnEntityFromDb()
    {
        // Arrange
        
        // Act
        await _sut.DeleteUserAsync(Users[0]);

        // Assert
        dbSetMock.Verify(x => x.Remove(Users[0]), Times.Exactly(1));
        _lacunaContextMock.Verify(x => x.SaveChangesAsync(default), Times.Exactly(1));
    }

    [Fact]
    public async Task Repository_GetUsername_ShouldReturnAnEntityByUsername()
    {
        // Arrange
        var user = new User("sampleusername", new Internet().Email(), new Internet().Password(12));
        Users.Add(user);

        // Act
        var res = await _sut.GetUsernameAsync("sampleusername");
        
        // Assert
        res.Should().Be(user);
    }

    [Fact]
    public async Task Repository_TryGetAnNonExistentEntityByUsername_ShouldReturnNull()
    {
        // Arrange
        // Act
        var res = await _sut.GetUsernameAsync("nonexistententity");
        // Assert
        res.Should().BeNull();
    }

    [Fact]
    public async Task Repository_TryGetAnNonExistentEntityByEmail_ShouldReturnNull()
    {
        // arrange
        // act
        var res = await _sut.GetEmailAsync("nonexistentemail@email.com");
        // assert
        res.Should().BeNull();
    }
}