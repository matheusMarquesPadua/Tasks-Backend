namespace task.Src.Application.Tasks.Dto;

public record TaskResponse(
    int Id,
    string Title,
    string Description,
    bool? Status,
    DateTime? StartDate,
    DateTime? EndDate,
    int OrganizationId,
    int? AssignedUserId,
    int CreatedByUserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
    );