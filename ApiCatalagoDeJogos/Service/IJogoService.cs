using ApiCatalagoDeJogos.InputModel;
using ApiCatalagoDeJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalagoDeJogos.Service
{
    public interface IJogoService
    {
        Task<List<JogoViewModel>> Obter(int pagina, int quantidade);
        Task<JogoViewModel> Obter(int id);
        Task<JogoViewModel> Inserir(JogoInputModel jogo);
        Task Atualizar(int id, JogoInputModel jogo);
        Task Atualizar(int id, double preco);
        Task Remover(int id);
    }
}
