using EM_DomainAluno;
using EM_RepositorioAluno;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;
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

        //public IActionResult Index()
        //{
        //    var ListaDeAlunos = _repositorio.GetAll();
        //    IEnumerable<AlunoModal> ListaConvertida = ListaDeAlunos.Select(o => new AlunoModal
        //    {
        //        Matricula = o.Matricula,
        //        Nome = o.Nome,
        //        CPF = o.CPF,
        //        Nascimento = o.Nascimento,
        //        Sexo = (EnumeradorSexo)o.Sexo,
        //    });

        //    return View(ListaConvertida);
        //}




        public IActionResult Index(string ValorDaBusca, string TipoDeBusca)
        {
            switch (TipoDeBusca)
            {
                default:
                    var ListaDeAlunos = _repositorio.GetAll();
                    IEnumerable<AlunoModal> ListaConvertida = ListaDeAlunos.Select(o => new AlunoModal
                    {
                        Matricula = o.Matricula,
                        Nome = o.Nome,
                        CPF = o.CPF,
                        Nascimento = o.Nascimento,
                        Sexo = (EnumeradorSexo)o.Sexo,
                    });

                    return View(ListaConvertida);

                case "Matricula":
                    if (ValorDaBusca == null || !Regex.IsMatch(ValorDaBusca, @"^[0-9]+$") )
                    {
                        return RedirectToAction("Index");
                    }
                    var ListaDeAlunosPorMatricula = _repositorio.GetByMatricula(Convert.ToInt32(ValorDaBusca));
                    List<AlunoModal> criandoLista = new() {
                        new AlunoModal {
                            Matricula = ListaDeAlunosPorMatricula.Matricula,
                            Nome = ListaDeAlunosPorMatricula.Nome,
                            CPF = ListaDeAlunosPorMatricula.CPF,
                            Nascimento= ListaDeAlunosPorMatricula.Nascimento,
                            Sexo = (EnumeradorSexo)ListaDeAlunosPorMatricula.Sexo}
                    };
                    IEnumerable<AlunoModal> AlunoPorMatricula = criandoLista;
                    return View(AlunoPorMatricula);

                case "Nome":
                    var ListaDeAlunosPorNome = _repositorio.GetByContendoNoNome(ValorDaBusca);
                    IEnumerable<AlunoModal> ListaConvertida2 = ListaDeAlunosPorNome.Select(o => new AlunoModal
                    {
                        Matricula = o.Matricula,
                        Nome = o.Nome,
                        CPF = o.CPF,
                        Nascimento = o.Nascimento,
                        Sexo = (EnumeradorSexo)o.Sexo,
                    });
                    return View(ListaConvertida2);
            }
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