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
                    IEnumerable<AlunoModel> ListaConvertida = ListaDeAlunos.Select(o => new AlunoModel
                    {
                        Matricula = o.Matricula,
                        Nome = o.Nome,
                        CPF = o.CPF,
                        Nascimento = o.Nascimento,
                        Sexo = (EnumeradorSexo)o.Sexo,
                    });

                    return View(ListaConvertida);

                case "Matricula":
                    if (ValorDaBusca == null) { return RedirectToAction("Index"); }
                    BuscarPorMatricula(ValorDaBusca); break;

                case "Nome":
                    if (ValorDaBusca == null) { return RedirectToAction("Index"); }
                    return BuscarPorNome(ValorDaBusca);
            }
            return View();
        }

        public IActionResult BuscarPorMatricula(string valor)
        {
            if (!int.TryParse(valor, out int matricula))
            {
                return RedirectToAction("Index");
            }
            var aluno = _repositorio.GetByMatricula(matricula);
            if (aluno.Matricula == 0) 
            { 
                return RedirectToAction("Index"); 
            }

            var listaDeAlunosPorMatricula = new List<AlunoModel>
            {
                conversaoDomainparaModel(aluno)
            };

            return View(listaDeAlunosPorMatricula);
        }
        public IActionResult BuscarPorNome(string valor)
        {
            var ListaDeAlunosPorNome = conversaoListaDeDomainparaModel(_repositorio.GetByContendoNoNome(valor));
            return View(ListaDeAlunosPorNome);
        }


        public IActionResult CadastrarEditar(int id = 0)
        {
            ViewBag.Error = TempData["Error"] as string;
            if (id == 0)
            {
                return View();
            }
            var aluno = _repositorio.GetByMatricula(id);
            return View(conversaoDomainparaModel(aluno));
        }

        public IActionResult Cadastrar(AlunoModel alunoModel)
        {
            var aluno = conversaoDeModelParaDomain(alunoModel);
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Alguma informação está errada";
                TempData["Error"] = ViewBag.Error;
                return RedirectToAction("CadastrarEditar");
            }
            if (aluno.CPF != null)
            {
                if (!CpfUtils.IsCpf(aluno.CPF))
                {
                    ViewBag.Error = "CPF invalido";
                    TempData["Error"] = ViewBag.Error;
                    return RedirectToAction("CadastrarEditar");
                }
            }
            _repositorio.Add(aluno);
            return RedirectToAction("Index");
        }
        public IActionResult Editar(AlunoModel alunoModel)
        {
            var aluno = conversaoDeModelParaDomain(alunoModel);
            if (aluno.CPF != null)
            {
                if (!CpfUtils.IsCpf(aluno.CPF))
                {
                    ViewBag.Error = "CPF invalido";
                    TempData["Error"] = ViewBag.Error;
                    return RedirectToAction("CadastrarEditar", new { id = aluno.Matricula });
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Alguma informação está errada";
                TempData["Error"] = ViewBag.Error;
                return RedirectToAction("CadastrarEditar", new { id = aluno.Matricula });
            }
            _repositorio.Update(aluno);
            return RedirectToAction("Index");
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

        private static AlunoModel conversaoDomainparaModel(Aluno aluno)
        {
            AlunoModel alunoModel = new()
            {
                Matricula = aluno.Matricula,
                Nome = aluno.Nome,
                CPF = aluno.CPF,
                Nascimento = aluno.Nascimento,
                Sexo = (EnumeradorSexo)aluno.Sexo
            };
            return alunoModel;
        }
        private static Aluno conversaoDeModelParaDomain(AlunoModel alunoModel)
        {
            Aluno aluno = new Aluno
            {
                Matricula = alunoModel.Matricula,
                Nome = alunoModel.Nome,
                Nascimento = alunoModel.Nascimento,
                CPF = alunoModel.CPF,
                Sexo = (EM_DomainEnum.EnumeradorSexo)alunoModel.Sexo
            };
            return aluno;
        }
        private static IEnumerable<AlunoModel> conversaoListaDeDomainparaModel(IEnumerable<Aluno> alunosDomain)
        {
            IEnumerable<AlunoModel> ListaConvertida = alunosDomain.Select(o => new AlunoModel
            {
                Matricula = o.Matricula,
                Nome = o.Nome,
                CPF = o.CPF,
                Nascimento = o.Nascimento,
                Sexo = (EnumeradorSexo)o.Sexo,
            });
            return ListaConvertida;
        }
    }
}
