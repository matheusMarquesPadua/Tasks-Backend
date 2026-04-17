using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using task.Src.Domain.Entities.Organization;
using task.Src.Infrastructure.Data.Context;
using Xunit;

namespace task.Tests.Application;

public class CreateOrganizationUnitCommandHandlerTest
{
    [Fact]
    public async Task Deve_Criar_Um_Novo_Setor_E_Salvar_No_Banco_De_Dados()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TaskDb_Test_CreateUnit")
            .Options;

        using var context = new AppDbContext(options);
        var setor = new OrganizationUnit
        {
            Name = "Setor de Marketing",
            ParentUnitId = null
        };

        context.OrganizationUnits.Add(setor);
        var result = await context.SaveChangesAsync();

        result.Should().BeGreaterThan(0);

        var setorSalvo = await context.OrganizationUnits.FindAsync(setor.Id);
        setorSalvo.Should().NotBeNull();
        setorSalvo!.Name.Should().Be("Setor de Marketing");
        setorSalvo.ParentUnitId.Should().BeNull();
    }
}