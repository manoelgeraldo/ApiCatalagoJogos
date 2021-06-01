using ApiCatalagoDeJogos.Entities;
using ApiCatalagoDeJogos.Exceptions;
using ApiCatalagoDeJogos.InputModel;
using ApiCatalagoDeJogos.Repositories;
using ApiCatalagoDeJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalagoDeJogos.Service
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var jogos = await _jogoRepository.Obter(pagina, quantidade);

            return jogos.Select(jogos => new JogoViewModel 
            { 
                Id = jogos.Id, 
                Nome = jogos.Nome,
                Produtora = jogos.Produtora, 
                Preco = jogos.Preco
            }).ToList();
        }

        public async Task<JogoViewModel> Obter(int id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
                return null;

            return new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }
        public async Task<JogoViewModel> Inserir(JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(jogo.Nome, jogo.Produtora);

            if (entidadeJogo.Count > 0)
                throw new JogoJaCadastradoException();

            var jogoInsert = new Jogo
            {
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };

            await _jogoRepository.Inserir(jogoInsert);

            return new JogoViewModel
            {
                Id = jogoInsert.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task Atualizar(int id, JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepository.Obter(id);

            if (entidadeJogo == null)
                throw new JogoNaoCadastradoException();

            entidadeJogo.Nome = jogo.Nome;
            entidadeJogo.Produtora = jogo.Produtora;
            entidadeJogo.Preco = jogo.Preco;

            await _jogoRepository.Atualizar(entidadeJogo);
        }

        public async Task Atualizar(int id, double preco)
        {
            var entidadeJogo = await _jogoRepository.Obter(id);

            if (entidadeJogo == null)
                throw new JogoNaoCadastradoException();

            entidadeJogo.Preco = preco;

            await _jogoRepository.Atualizar(entidadeJogo);
        }

        public async Task Remover(int id)
        {
            var jogo = await _jogoRepository.Obter(id);

            if (jogo == null)
                throw new JogoNaoCadastradoException();

            await _jogoRepository.Remover(id);
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }
    }
}
