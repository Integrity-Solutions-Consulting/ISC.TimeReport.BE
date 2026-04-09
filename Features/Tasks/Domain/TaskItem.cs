namespace isc_tmr_backend.Features.Tasks.Domain;

using isc_tmr_backend.Features.Users.Domain;

public class TaskItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid? AssigneeId { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Projects.Domain.Project? Project { get; private set; }
    public User? Assignee { get; private set; }
    public User? Creator { get; private set; }

    private TaskItem()
    {
        Title = string.Empty;
        Description = string.Empty;
    }

    public static TaskItem Create(string title, string description, Guid projectId, Guid createdBy, Guid? assigneeId = null)
    {
        return new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            IsCompleted = false,
            ProjectId = projectId,
            AssigneeId = assigneeId,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static TaskItem Reconstitute(
        Guid id, string title, string description, bool isCompleted,
        Guid projectId, Guid? assigneeId, Guid createdBy,
        DateTime createdAt, DateTime updatedAt)
    {
        return new TaskItem
        {
            Id = id,
            Title = title,
            Description = description,
            IsCompleted = isCompleted,
            ProjectId = projectId,
            AssigneeId = assigneeId,
            CreatedBy = createdBy,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };
    }

    public void Update(string title, string description, Guid? assigneeId)
    {
        Title = title;
        Description = description;
        AssigneeId = assigneeId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsPending()
    {
        IsCompleted = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
