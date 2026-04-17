using task.Src.Domain.Entities.Identity;

namespace task.Src.Domain.Entities.Organization
{
    public class UserUnitRole
    {
        public int id { get; set; }

        public int UserId { get; set; }

        public ApplicationUser User { get; set; }
        public int OrganizationUnitId { get; set; }
        public OrganizationUnit organizationUnit { get; set; }

        public string RoleName { get; set; } = string.Empty;

        public bool CanAssignTasks { get; set; } = false;

    }
}
