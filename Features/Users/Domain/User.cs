namespace isc_tmr_backend.Features.Users.Domain;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string DisplayName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private User()
    {
        Email = string.Empty;
        DisplayName = string.Empty;
    }

    public static User Create(string email, string displayName)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            DisplayName = displayName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static User Reconstitute(Guid id, string email, string displayName, DateTime createdAt, DateTime updatedAt)
    {
        return new User
        {
            Id = id,
            Email = email,
            DisplayName = displayName,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }

    public void Update(string email, string displayName)
    {
        Email = email;
        DisplayName = displayName;
        UpdatedAt = DateTime.UtcNow;
    }
}
