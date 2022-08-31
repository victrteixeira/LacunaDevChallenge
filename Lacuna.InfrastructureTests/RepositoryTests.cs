using FluentAssertions;
using Lacuna.Domain.Interfaces;
using Lacuna.Domain.Users;
using Lacuna.Infrastructure.Context;
using Lacuna.Infrastructure.Repository;
using Lacuna.InfrastructureTests.Fixtures;
using MockQueryable.Moq;
using Moq;

namespace Lacuna.InfrastructureTests;

public class RepositoryTests
{
    private readonly IUserRepository _sut;
    private readonly Mock<LacunaContext> _lacunaContextMock;
    private List<User> _users;
    public List<User> Users
    {
        get => _users;
        set => _users = value;
    }

    public RepositoryTests()
    {
        _lacunaContextMock = new Mock<LacunaContext>();
        _sut = new UserRepository(_lacunaContextMock.Object);
        Users = DbFixture.UserPopulate();
    }

    [Fact]
    public async Task Repository_GetEmailAsync_ShouldReturnAnEntity()
    {
        // Arrange
        var dbSetMock = Users.AsQueryable().BuildMockDbSet();
        _lacunaContextMock.Setup(x => x.LacunaUsers)
            .Returns(dbSetMock.Object);
        // Act

        var res = await _sut.GetEmailAsync(Users[0].Email);
        // Assert
        res.Should().NotBeNull();
        res.Should().BeOfType<User>();
        res.Should().BeEquivalentTo(Users[0]);
        res.Should().NotBeNull();
    }


}