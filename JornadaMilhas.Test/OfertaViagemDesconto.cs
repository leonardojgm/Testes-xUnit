using JornadaMilhasV1.Modelos;
using System;
using System.Linq;
using Xunit;

namespace JornadaMilhas.Test
{
    public class OfertaViagemDesconto
    {
        [Fact]
        public void RetornaPrecoAtualizadoQuandoAplicadoDesconto()
        {
            //arrange
            Rota rota = new Rota("OrigemA", "DestinoB");
            Periodo periodo = new Periodo(new DateTime(2024, 05, 01), new DateTime(2024, 05, 10));
            double precoOriginal = 100.0;
            double desconto = 20.00;
            double precoComDesconto = precoOriginal - desconto;
            OfertaViagem oferta = new OfertaViagem(rota, periodo, precoOriginal);

            //act
            oferta.Desconto = desconto;

            //asssert
            Assert.Equal(precoComDesconto, oferta.Preco);
        }

        [Theory]
        [InlineData(120, 30)]
        [InlineData(100, 30)]
        public void RetornaDescontoMaximoQuandoValorDescontoMaiorOuIgualQuePreco(double desconto, double precoComDesconto)
        {
            //arrange
            Rota rota = new Rota("OrigemA", "DestinoB");
            Periodo periodo = new Periodo(new DateTime(2024, 05, 01), new DateTime(2024, 05, 10));
            double precoOriginal = 100.0;
            OfertaViagem oferta = new OfertaViagem(rota, periodo, precoOriginal);

            //act
            oferta.Desconto = desconto;

            //asssert
            Assert.Equal(precoComDesconto, oferta.Preco, 0.001);
        }

        [Fact]
        public void RetornaDescontoMaximoQuandoValorDescontoMaiorQuePreco()
        {
            //arrange
            Rota rota = new Rota("OrigemA", "DestinoB");
            Periodo periodo = new Periodo(new DateTime(2024, 05, 01), new DateTime(2024, 05, 10));
            double precoOriginal = 100.0;
            double desconto = 120.00;
            double precoComDesconto = 30.00;
            OfertaViagem oferta = new OfertaViagem(rota, periodo, precoOriginal);

            //act
            oferta.Desconto = desconto;

            //asssert
            Assert.Equal(precoComDesconto, oferta.Preco, 0.001);
        }

        [Fact]
        public void RetornaPrecoOriginalQuandoValorDescontoNegativo()
        {
            //arrange
            Rota rota = new Rota("OrigemA", "DestinoB");
            Periodo periodo = new Periodo(new DateTime(2024, 05, 01), new DateTime(2024, 05, 10));
            double precoOriginal = 100.00;
            double desconto = -20.00;
            double precoComDesconto = precoOriginal;
            OfertaViagem oferta = new OfertaViagem(rota, periodo, precoOriginal);

            //act
            oferta.Desconto = desconto;

            //assert
            Assert.Equal(precoComDesconto, oferta.Preco, 0.001);
        }

        [Fact]
        public void RetornaTresErrosDeValidacaoQuandoRotaPeriodoEPrecoSaoInvalidos()
        {
            //arrange
            int quantidadeEsperada = 3;
            Rota rota = null;
            Periodo periodo = new Periodo(new DateTime(2024, 6, 1), new DateTime(2024, 5, 10));
            double preco = -100;

            //act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            //assert
            Assert.Equal(quantidadeEsperada, oferta.Erros.Count());
        }
    }
}
