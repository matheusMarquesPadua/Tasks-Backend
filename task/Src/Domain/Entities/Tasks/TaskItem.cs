using task.Src.Domain.Common;

namespace task.Src.Domain.Entities.Tasks;

public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }

    public bool? Status { get; set; };

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int OrganizationId { get; set; }

    public int? AssignedUserId { get; set; }
    public int CreatedByUserId { get; set; }
}
