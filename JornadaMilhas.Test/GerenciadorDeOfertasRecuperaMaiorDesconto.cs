using Bogus;
using JornadaMilhasV1.Gerencidor;
using JornadaMilhasV1.Modelos;
using System;
using System.Collections.Generic;
using Xunit;

namespace JornadaMilhas.Test
{
    public class GerenciadorDeOfertasRecuperaMaiorDesconto
    {
        [Fact]
        public void RetornaOfertaNulaQuandoListaEstaVazia()
        {
            //arrange
            List<OfertaViagem> lista = new List<OfertaViagem>();
            GerenciadorDeOfertas gerenciador = new GerenciadorDeOfertas(lista);
            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");

            //act
            OfertaViagem oferta = gerenciador.RecuperaMaiorDesconto(filtro);
            
            //assert
            Assert.Null(oferta);
        }

        [Fact]
        //destino = são paulo, desconto = 40, preco = 80
        public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto40()
        {
            //arrange
            Faker<Periodo> fakerPeriodo = new Faker<Periodo>()
                .CustomInstantiator(f =>
                {
                    DateTime dataInicio = f.Date.Soon();

                    return new Periodo(dataInicio, dataInicio.AddDays(30));
                });
            Rota rota = new Rota("Curitiba", "São Paulo");
            Faker<OfertaViagem> fakerOferta = new Faker<OfertaViagem>()
                .CustomInstantiator(f => new OfertaViagem(
                    rota,
                    fakerPeriodo.Generate(),
                    100 * f.Random.Int(1, 100))
                )
                .RuleFor(o => o.Desconto, f => 40)
                .RuleFor(o => o.Ativa, f => true);

            OfertaViagem ofertaEscolhida = new OfertaViagem(rota, fakerPeriodo.Generate(), 80)
            {
                Desconto = 40,
                Ativa = true
            };

            OfertaViagem ofertaInativa = new OfertaViagem(rota, fakerPeriodo.Generate(), 70)
            {
                Desconto = 40,
                Ativa = false
            };

            List<OfertaViagem> lista = fakerOferta.Generate(200);

            lista.Add(ofertaEscolhida);
            lista.Add(ofertaInativa);

            GerenciadorDeOfertas gerenciador = new GerenciadorDeOfertas(lista);
            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
            double precoEsperado = 40;

            //act
            OfertaViagem oferta = gerenciador.RecuperaMaiorDesconto(filtro);

            //assert
            Assert.NotNull(oferta);
            Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
        }
    }
}
