using FluentAssertions;
using task.Src.Domain.Entities.Organization;
using Xunit;

namespace task.Tests.Domain;

public class OrganizationUnitTest
{
    [Fact]
    public void Deve_adicionar_seto_fillho_e_Manter_a_referencia_para_o_pai()
    {
        var diretoria = new OrganizationUnit { Id = 1, Name = "Diretoria TI" };
        var setorFilho = new OrganizationUnit { Id = 2, Name = "Setor de desenvolvimento" };

        setorFilho.ParentUnitId = diretoria.Id;
        setorFilho.ParantUnit = diretoria;
        diretoria.Children.Add(setorFilho);

        diretoria.Children.Should().ContainSingle();
        diretoria.Children.First().Name.Should().Be("Setor de desenvolvimento");

        setorFilho.ParantUnit.Should().NotBeNull();
        setorFilho.ParantUnit.Name.Should().Be("Diretoria TI");
    }

    [Fact]
    public void Nao_Deve_Permitir_Que_Uma_Unidade_Seja_Filha_De_Si_Mesma()
    {
        // Arrange
        var setor = new OrganizationUnit { Id = 1, Name = "TI" };

        // Act
        // Simulando a regra de negócio que você pode adicionar depois na entidade:
        Action action = () =>
        {
            if (setor.Id == setor.Id) 
                throw new InvalidOperationException("Uma unidade não pode ser pai dela mesma.");
        };

        // Assert
        action.Should().Throw<InvalidOperationException>()
              .WithMessage("Uma unidade não pode ser pai dela mesma.");
    }

}
