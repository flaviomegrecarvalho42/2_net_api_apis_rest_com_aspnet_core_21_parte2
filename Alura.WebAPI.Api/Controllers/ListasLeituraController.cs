using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.ListaLeitura.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ListasLeituraController : ControllerBase
    {
        private readonly IRepository<Livro> _repository;

        public ListasLeituraController(IRepository<Livro> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult ListarListasLeitura()
        {
            Lista paraLer = CriarLista(TipoListaLeitura.ParaLer);
            Lista lendo = CriarLista(TipoListaLeitura.Lendo);
            Lista lidos = CriarLista(TipoListaLeitura.Lidos);

            var colecaoListaLeitura = new List<Lista> { paraLer, lendo, lidos };
            return Ok(colecaoListaLeitura);
        }

        [HttpGet("{tipoLista}")]
        public IActionResult ListarListaLeitura(TipoListaLeitura tipoLista)
        {
            var listaLeitura = CriarLista(tipoLista);

            if (listaLeitura == null)
            {
                return NotFound(); //Error 404
            }

            return Ok(listaLeitura);
        }

        private Lista CriarLista(TipoListaLeitura tipoListaLeitura)
        {
            return new Lista
            {
                Tipo = tipoListaLeitura.ParaString(),
                Livros = _repository.All
                                    .Where(l => l.Lista == tipoListaLeitura)
                                    .Select(l => l.ToLivroApi())
                                    .ToList()
            };
        }
    }
}