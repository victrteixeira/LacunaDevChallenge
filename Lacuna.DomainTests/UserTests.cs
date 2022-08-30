using Bogus.DataSets;
using FluentAssertions;
using Lacuna.Domain.Users;
using Lacuna.Domain.Validation;

namespace Lacuna.DomainTests;

public class UserTests
{
    [Fact]
    public void User_CreateNewUser_ResultObjectValidState()
    {
        // Arrange
        // Act
        Action action = () => new User("RandomUsername", new Internet().Email(), new Internet().Password(8));
        // Assert
        action.Should().NotThrow<DomainExceptionValidation>();
    }

    [Fact]
    public void User_CreateNewUser_EmptyUsernameThrowException()
    {
        // Arrange
        // Act
        Action user = () => new User("", new Internet().Email(), new Internet().Password(8));
        // Assert
        user.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Invalid username.");
    }
    
    [Fact]
    public void User_CreateNewUser_NameWithSpecialCharThrowException()
    {
        // Arrange
        // Act
        Action user = () => new User("vict@rteixeria", new Internet().Email(), new Internet().Password(8));
        // Assert
        user.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Invalid username.");
    }
    
    [Fact]
    public void User_CreateNewUser_NameLengthLessThan4CharThrowException()
    {
        // Arrange
        // Act
        Action user = () => new User("vic", new Internet().Email(), new Internet().Password(8));
        // Assert
        user.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Username invalid, the minimum are 3 characters and maximum are 32 characters.");
    }
    
    [Fact]
    public void User_CreateNewUser_NameLengthGreaterThan4Char()
    {
        // Arrange
        // Act
        Action user = () => new User("LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL", new Internet().Email(), new Internet().Password(8));
        // Assert
        user.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Username invalid, the minimum are 3 characters and maximum are 32 characters.");
    }
    
    [Fact]
    public void User_CreateNewUser_EmailInvalidPatternThrowException()
    {
        // Arrange
        // Act
        Action user = () => new User("RandomUsername", "something.com", new Internet().Password(8));
        // Assert
        user.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Invalid email.");
    }
    
    [Fact]
    public void User_CreateNewUser_PasswordLessThan8CharThrowException()
    {
        // Arrange
        // Act
        Action user = () => new User("RandomUsername", new Internet().Email(), new Internet().Password(7));
        // Assert
        user.Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Password must has 8 chars at minimum.");
    }

    [Fact]
    public void User_GetUserEmail_ReturnEmailValueFromAnUser()
    {
        // Arrange
        var user = new User("RandomUsername", "someemail@totest.com", new Internet().Password(8));
        // Act
        var res = user.Email;
        // Assert
        res.Should().BeEquivalentTo("someemail@totest.com");
    }

    [Fact]
    public void User_GetUserUsername_ReturnUsernameValueFromAnUser()
    {
        // Arrange
        var user = new User("RandomUsername", "someemail@totest.com", new Internet().Password(8));
        // Act
        var res = user.Username;
        // Assert
        res.Should().BeEquivalentTo("RandomUsername");
    }
}