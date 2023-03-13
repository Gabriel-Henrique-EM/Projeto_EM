using EM_DomainAluno;
using EM_RepositorioAluno;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebApp.Models;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public class AlunoController : Controller
    {
        private readonly RepositorioAluno _repositorio;
        public AlunoController(RepositorioAluno repositorio)
        {
            _repositorio = repositorio;
        }
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
                    if (ValorDaBusca == null || !Regex.IsMatch(ValorDaBusca, @"^[0-9]+$"))
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

        [HttpPost]
        public IActionResult CadastrarEditar(int id = 0)
        {
            if (id == 0)
            {
                return View();
            }
            var aluno = _repositorio.GetByMatricula(id);
            return View(aluno);
        }

        public IActionResult CadastrarEditar()
        {
            ViewBag.Error = TempData["Error"] as string;
            return View();
        }

        public IActionResult Cadastrar(Aluno aluno)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Alguma informação está errada";
                TempData["Error"] = ViewBag.Error;
                return RedirectToAction("CadastrarEditar");
            }
            if (aluno.CPF != null)
            {
                if (!ValidaDigitoCPF.ValidaCPF(aluno.CPF))
                {
                    ViewBag.Error = "CPF invalido";
                    TempData["Error"] = ViewBag.Error;
                    return RedirectToAction("CadastrarEditar");
                }
            }
            _repositorio.Add(aluno);
            return RedirectToAction("Index");
        }

        
        public IActionResult Editar(Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                _repositorio.Update(aluno);
                return RedirectToAction("Index");
            }
            return BadRequest(ModelState);
        }

        public IActionResult Deletar()
        {
            return PartialView();
        }

        
        public IActionResult ConfirmarDeletar(int id)
        {
            var aluno = _repositorio.GetByMatricula(id);
            if (aluno != null)
            {
                _repositorio.Remove(aluno);
                return RedirectToAction("Index");
            }
            return BadRequest(ModelState);
        }
    }
}
