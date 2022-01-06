using System.IO;
using Microsoft.AspNetCore.Http;

namespace Alura.ListaLeitura.Modelos
{
    public static class LivrosExtensions
    {
        public static byte[] ConvertToBytes(this IFormFile image)
        {
            if (image == null)
            {
                return null;
            }

            using (var inputStream = image.OpenReadStream())
            using (var stream = new MemoryStream())
            {
                inputStream.CopyTo(stream);
                return stream.ToArray();
            }
        }

        public static Livro ToLivro(this LivroUpload livroUploadModel)
        {
            return new Livro
            {
                Id = livroUploadModel.Id,
                Titulo = livroUploadModel.Titulo,
                Subtitulo = livroUploadModel.Subtitulo,
                Resumo = livroUploadModel.Resumo,
                Autor = livroUploadModel.Autor,
                ImagemCapa = livroUploadModel.Capa.ConvertToBytes(),
                Lista = livroUploadModel.Lista
            };
        }

        public static LivroApi ToLivroApi(this Livro livroModel)
        {
            return new LivroApi
            {
                Id = livroModel.Id,
                Titulo = livroModel.Titulo,
                Subtitulo = livroModel.Subtitulo,
                Resumo = livroModel.Resumo,
                Autor = livroModel.Autor,
                Capa = $"/api/Livros/capa/{livroModel.Id}",
                Lista = livroModel.Lista.ParaString()
            };
        }

        public static LivroUpload ToLivroUpload(this Livro livroModel)
        {
            return new LivroUpload
            {
                Id = livroModel.Id,
                Titulo = livroModel.Titulo,
                Subtitulo = livroModel.Subtitulo,
                Resumo = livroModel.Resumo,
                Autor = livroModel.Autor,
                Lista = livroModel.Lista
            };
        }

        public static LivroUpload ToLivroUpload(this LivroApi livroApModel)
        {
            return new LivroUpload
            {
                Id = livroApModel.Id,
                Titulo = livroApModel.Titulo,
                Subtitulo = livroApModel.Subtitulo,
                Resumo = livroApModel.Resumo,
                Autor = livroApModel.Autor,
                Lista = livroApModel.Lista.ParaTipo()
            };
        }
    }
}
