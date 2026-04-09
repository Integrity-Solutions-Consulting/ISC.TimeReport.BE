namespace isc_tmr_backend.Features.Projects.Domain;

using isc_tmr_backend.Features.Users.Domain;

public class Project
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid OwnerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public User? Owner { get; private set; }

    private Project()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    public static Project Create(string name, string description, Guid ownerId)
    {
        return new Project
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static Project Reconstitute(Guid id, string name, string description, Guid ownerId, DateTime createdAt, DateTime updatedAt)
    {
        return new Project
        {
            Id = id,
            Name = name,
            Description = description,
            OwnerId = ownerId,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
