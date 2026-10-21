using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;

namespace Test.Domain.Servicos;

[TestClass]
public class VeiculoServicoTest
{
    private static DbContexto CriarContexto()
    {
        var options = new DbContextOptionsBuilder<DbContexto>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DbContexto(options);
    }

    [TestMethod]
    public void DeveFiltrarVeiculosPorNomeEMarca()
    {
        using var contexto = CriarContexto();
        contexto.Veiculos.AddRange(
            new Veiculo { Nome = "Civic", Marca = "Honda", Ano = 2020 },
            new Veiculo { Nome = "Corolla", Marca = "Toyota", Ano = 2021 },
            new Veiculo { Nome = "City", Marca = "Honda", Ano = 2022 }
        );
        contexto.SaveChanges();

        var servico = new VeiculoServico(contexto);

        var veiculos = servico.Todos(null, "city", "honda");

        Assert.AreEqual(1, veiculos.Count);
        Assert.AreEqual("City", veiculos[0].Nome);
    }

    [TestMethod]
    public void DevePaginarVeiculosOrdenadosPorId()
    {
        using var contexto = CriarContexto();
        for (var indice = 1; indice <= 12; indice++)
        {
            contexto.Veiculos.Add(new Veiculo
            {
                Nome = $"Veículo {indice}",
                Marca = "Marca",
                Ano = 2020
            });
        }
        contexto.SaveChanges();

        var servico = new VeiculoServico(contexto);

        var primeiraPagina = servico.Todos(1);
        var segundaPagina = servico.Todos(2);

        Assert.AreEqual(10, primeiraPagina.Count);
        Assert.AreEqual(2, segundaPagina.Count);
        Assert.AreEqual(1, primeiraPagina[0].Id);
        Assert.AreEqual(11, segundaPagina[0].Id);
    }
}
