using System.Text.RegularExpressions;

namespace ROP;
internal class Program
{
    static void Main(string[] args)
    {
        Result<User> userResult = User.Create("Mohamed Ehab", Email.Create("user@email.com").Value);
        if (userResult.IsFailure)
            Console.WriteLine(userResult.Error);
        else
            Console.WriteLine(userResult.Value);

        Console.ReadKey();
    }
}

public class User
{
    private const int DefaultLength = 20;

    public Guid Id { get; private set; }
    public string? Name { get; private set; }
    public Email Email { get; private set; }

    private User(Guid id, string? name, Email email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public static Result<User> Create(string name, Email email)
        => Result<(string userName, Email email)>.Create((name, email))
            .Ensure(
                user => !string.IsNullOrEmpty(user.userName),
                DomainErrors.User.Empty
            ).Ensure(
                user => name.Length <= DefaultLength,
                DomainErrors.User.DefaultLength
            ).Map(
                user =>
                    Result<User>.Success(
                        new User(Guid.NewGuid(), user.userName, user.email)));

    public override string ToString() => $"[{Id}] {Name} {Email.Address}";

}

public record Email
{
    private const int DefaultLength = 100;
    public string Address { get; }

    private Email(string address)
    {
        Address = address;
    }

    public static Result<Email> Create(string address)
        => Result<string>.Create(address)
            .Ensure(
                e => !string.IsNullOrWhiteSpace(e),
                DomainErrors.Email.Empty)
            .Ensure(
                e => e.Length <= DefaultLength,
                DomainErrors.Email.TooLong)
            .Ensure(
                IsValidEmail,
                DomainErrors.Email.InvalidFormat)
            .Map(e => Result<Email>.Success(new Email(e)));

    private static bool IsValidEmail(string email)
        => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
}

public static class DomainErrors
{
    public static class Email
    {
        public const string TooLong = "Email address is too long.";
        public const string Empty = "Email address can not be empty.";
        public const string InvalidFormat = "Invalid email format.";
    }

    public static class User
    {
        public const string Empty = "User name can not be empty";
        public const string DefaultLength = $"User name must be less than or equal to 20 characters.";
    }
}