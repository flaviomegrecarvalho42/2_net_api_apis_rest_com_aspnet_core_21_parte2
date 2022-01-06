using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Alura.ListaLeitura.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly IRepository<Livro> _repository;

        public LivrosController(IRepository<Livro> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult ListarLivros()
        {
            var listaLivros = _repository.All
                                         .Select(l => l.ToLivroApi())
                                         .ToList();

            return Ok(listaLivros);
        }

        [HttpGet("{id}")]
        public IActionResult Recuperar(int id)
        {
            var livroModel = _repository.Find(id);

            if (livroModel == null)
            {
                return NotFound(); //Error 404
            }

            return Ok(livroModel.ToLivroApi());
        }

        [HttpPost]
        public IActionResult Incluir([FromForm] LivroUpload livroUploadModel)
        {
            if (ModelState.IsValid)
            {
                var livro = livroUploadModel.ToLivro();
                _repository.Incluir(livro);

                var uri = Url.Action("Recuperar", new { id = livro.Id });
                return Created(uri, livro);
            }

            return BadRequest(); //Código 400
        }

        [HttpPut]
        public IActionResult Atualizar([FromForm] LivroUpload livroUploadModel)
        {
            if (ModelState.IsValid)
            {
                var livro = livroUploadModel.ToLivro();

                if (livroUploadModel.Capa == null)
                {
                    livro.ImagemCapa = _repository.All
                                                  .Where(l => l.Id == livro.Id)
                                                  .Select(l => l.ImagemCapa)
                                                  .FirstOrDefault();
                }

                _repository.Alterar(livro);
                return Ok(); //Código 200
            }

            return BadRequest(); //Código 400
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var livroModel = _repository.Find(id);

            if (livroModel == null)
            {
                return NotFound(); //Error 404
            }

            _repository.Excluir(livroModel);
            return NoContent(); //Código 204
        }

        [HttpGet("capa/{id}")]
        public IActionResult ImagemCapa(int id)
        {
            byte[] img = _repository.All
                                    .Where(l => l.Id == id)
                                    .Select(l => l.ImagemCapa)
                                    .FirstOrDefault();

            if (img != null)
            {
                return File(img, "image/png");
            }

            return File("~/images/capas/capa-vazia.png", "image/png");
        }
    }
}