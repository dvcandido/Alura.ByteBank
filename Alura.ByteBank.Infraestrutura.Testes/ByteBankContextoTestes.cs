using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Alura.ByteBank.Dados.Contexto;

namespace Alura.ByteBank.Infraestrutura.Testes
{
    public class ByteBankContextoTestes
    {
        [Fact]
        public void TestaConexaoContextoComBDMySQL()
        {
            //Arrange
            var contexto = new ByteBankContexto();
            bool conectado;
            //Act
            try
            {
                conectado = contexto.Database.CanConnect();

            }
            catch (Exception)
            {

                throw new Exception("Não foi possivel conectar a base de dados.");
            }
            //Assert
            Assert.True(conectado);
        }
    }
}
