using EM_DomainAluno;
using EM_RepositorioAluno;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly RepositorioAluno _repositorio;
        public HomeController(RepositorioAluno repositorio)
        {
            _repositorio = repositorio;
        }

        public IActionResult Index()
        {
            var ListaDeAlunos = _repositorio.GetAll();
            IEnumerable<AlunoModal> ListaConvertida = ListaDeAlunos.Select(o => new AlunoModal
            {
                Matricula= o.Matricula,
                Nome= o.Nome,
                CPF= o.CPF,
                Nascimento = o.Nascimento,
                Sexo = (EnumeradorSexo)o.Sexo,
            }); 

            return View(ListaConvertida);
        }

        public IActionResult Add(AlunoModal aluno)
        {
            // _repositorio.Add((Aluno)aluno);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}