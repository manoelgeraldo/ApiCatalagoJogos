using ApiCatalagoDeJogos.Exceptions;
using ApiCatalagoDeJogos.InputModel;
using ApiCatalagoDeJogos.Service;
using ApiCatalagoDeJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalagoDeJogos.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = await _jogoService.Obter(pagina, quantidade);

            if (jogos.Count() == 0)
                return NoContent();

            return Ok(jogos);
        }
        
        [HttpGet("{idJogo:int}")]
        public async Task<ActionResult<List<JogoViewModel>>> Obter([FromRoute] int idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);
            
            if(jogo == null)
                return NoContent();

            return Ok(jogo);
        }
        
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);

                return Ok(jogo);
            }
            catch (JogoJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora!");
            }
        }
        
        [HttpPut("{idJogo:int}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] int idJogo, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);

                return Ok();
            }
            catch(JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo!");
            }
        }
        
        [HttpPatch("{idJogo:int}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] int idJogo, [FromBody] double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);
                return Ok();
            }
            catch(JogoJaCadastradoException ex)
            {
                return NotFound("Não existe este jogo!");
            }
        }
        
        [HttpDelete("{idJogo:int}")]
        public async Task<ActionResult> ApagarJogo([FromRoute] int idJogo)
        {
            try
            {
                await _jogoService.Remover(idJogo);
                
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo!");
            }
        }
    }
}
