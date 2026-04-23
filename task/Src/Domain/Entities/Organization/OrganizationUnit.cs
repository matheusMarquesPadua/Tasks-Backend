using task.Src.Domain.Common;

namespace task.Src.Domain.Entities.Organization;

public class OrganizationUnit : BaseEntity
{
    public string Name { get; set; } = string.Empty;


    public int? ParentUnitId { get; set; }
    public OrganizationUnit? ParantUnit { get; set; }

    public ICollection<OrganizationUnit> Children { get; set; } = new List<OrganizationUnit>();
}
