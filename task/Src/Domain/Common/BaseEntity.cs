namespace task.Src.Domain.Common;

public class BaseEntity <TId> : BaseId <TId>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeleteAt { get; set; }
}
public abstract class BaseEntity : BaseEntity<int>
{
}