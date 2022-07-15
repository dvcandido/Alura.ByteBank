using Alura.ByteBank.Dados.Repositorio;
using Alura.ByteBank.Dominio.Entidades;
using Alura.ByteBank.Dominio.Interfaces.Repositorios;
using Alura.ByteBank.Infraestrutura.Testes.DTO;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace Alura.ByteBank.Infraestrutura.Testes
{
    public class ContaCorrenteRepositorioTestes
    {
        private readonly IContaCorrenteRepositorio _repositorio;

        public ContaCorrenteRepositorioTestes()
        {
            var servico = new ServiceCollection();
            servico.AddTransient<IContaCorrenteRepositorio, ContaCorrenteRepositorio>();

            var provedor = servico.BuildServiceProvider();

            _repositorio = provedor.GetService<IContaCorrenteRepositorio>();
        }

        [Fact]
        public void TestaObterTodosClientes()
        {
            //Arrange
            //Act
            List<ContaCorrente> lista = _repositorio.ObterTodos();

            //Assert
            Assert.NotNull(lista);
            Assert.Equal(4, lista.Count);
        }

        [Fact]
        public void TestaObterClientePorId()
        {
            //Arrange
            //Act
            var cliente = _repositorio.ObterPorId(1);
            //Assert
            Assert.NotNull(cliente);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void TestaObterClientePorVariosId(int id)
        {
            //Arrange
            //Act
            var cliente = _repositorio.ObterPorId(id);
            //Assert
            Assert.NotNull(cliente);
        }

        [Fact]
        public void TestaAtualizaSaldoDeterminadaConta()
        {
            //Arrange
            var conta = _repositorio.ObterPorId(1);
            double saldoNovo = 15;
            conta.Saldo = saldoNovo;

            //Act
            var atualizado = _repositorio.Atualizar(1, conta);

            //Assert
            Assert.True(atualizado);
        }

        [Fact]
        public void TesteInsereUmaNovaContaCorrenteNoBancoDeDados()
        {
            //Arrange
            var conta = new ContaCorrente()
            {
                Saldo = 10,
                Identificador = Guid.NewGuid(),
                Cliente = new Cliente()
                {
                    Nome = "Kent Nelson",
                    CPF = "486.074.980-45",
                    Identificador = Guid.NewGuid(),
                    Profissao = "Bancário",
                    Id = 1
                },
                Agencia = new Agencia()
                {
                    Nome = "Agencia Central Coast City",
                    Identificador = Guid.NewGuid(),
                    Id = 1,
                    Endereco = "Rua das Flores, 25",
                    Numero = 147
                }
            };
            
            //Act
            var retorno = _repositorio.Adicionar(conta);

            //Assert
            Assert.True(retorno);
        }

        [Fact]
        public void TestaObterAgenciaMock()
        {
            //Arrange
            var byteBankRepositorioMock = new Mock<IByteBankRepositorio>();
            var mock = byteBankRepositorioMock.Object;

            //Act
            var lista = mock.BuscarAgencias();

            //Assert
            byteBankRepositorioMock.Verify(b => b.BuscarAgencias());
        }

        [Fact]
        public void TestaConsultaPix()
        {
            //Arange
            var guid = new Guid("a0b80d53-c0dd-4897-ab90-c0615ad80d5a");
            var pix = new PixDTO() { Chave = guid, Saldo = 10 };

            var pixRepositorioMock = new Mock<IPixRepositorio>();
            pixRepositorioMock.Setup(x => x.consultaPix(It.IsAny<Guid>())).Returns(pix);

            var mock = pixRepositorioMock.Object;

            //Act
            var saldo = mock.consultaPix(guid).Saldo;

            //Assert
            Assert.Equal(10, saldo);
        }
    }
}
