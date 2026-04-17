using FluentAssertions;
using task.Src.Domain.Entities.Identity;
using task.Src.Domain.Entities.Organization;
using Xunit;

namespace task.Tests.Domain;

public class UserUnitRoleTests
{
    [Fact]
    public void Deve_Criar_Vinculo_De_Diretor_Com_Permissao_De_Delegar_Tarefas()
    {
        // Arrange
        var usuario = new ApplicationUser { Id = 1, UserName = "matheus@teste.com" };
        var diretoria = new OrganizationUnit { Id = 1, Name = "Diretoria de Inovação" };

        // Act
        var vinculo = new UserUnitRole
        {
            UserId = usuario.Id,
            User = usuario,
            OrganizationUnitId = diretoria.Id,
            organizationUnit = diretoria,
            RoleName = "Diretor",
            CanAssignTasks = true // Regra chave!
        };

        // Assert
        vinculo.User.UserName.Should().Be("matheus@teste.com");
        vinculo.organizationUnit.Name.Should().Be("Diretoria de Inovação");
        vinculo.RoleName.Should().Be("Diretor");
        vinculo.CanAssignTasks.Should().BeTrue();
    }

    [Fact]
    public void Deve_Criar_Vinculo_De_Colaborador_Sem_Permissao_De_Delegar_Tarefas()
    {
        // Arrange
        var usuario = new ApplicationUser { Id = 2, UserName = "joao@teste.com" };
        var setor = new OrganizationUnit { Id = 2, Name = "Desenvolvimento" };

        // Act
        var vinculo = new UserUnitRole
        {
            UserId = usuario.Id,
            OrganizationUnitId = setor.Id,
            RoleName = "Colaborador",
            CanAssignTasks = false
        };

        // Assert
        vinculo.CanAssignTasks.Should().BeFalse();
        vinculo.RoleName.Should().Be("Colaborador");
    }
}