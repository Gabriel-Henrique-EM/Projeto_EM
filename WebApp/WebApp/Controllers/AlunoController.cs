using EM_RepositorioAluno;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Helpers;
using EM_DomainAluno;

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
            ViewBag.Error = TempData["Error"] as string;
            if (TipoDeBusca != null && ValorDaBusca == null) { return RedirectToAction("Index"); }
            if (TipoDeBusca == "Matricula")
            {
                return BuscarPorMatricula(ValorDaBusca);
            }
            if (TipoDeBusca == "Nome") 
            {
                return BuscarPorNome(ValorDaBusca);
            }
            else
            {
                var ListaDeAlunos = ConversaoDeTipos.ConversaoListaDeDomainparaModel(_repositorio.GetAll());
                return View(ListaDeAlunos);
            }
            //switch (TipoDeBusca)
            //{
            //    default:
            //        var ListaDeAlunos = ConversaoDeTipos.ConversaoListaDeDomainparaModel(_repositorio.GetAll());
            //        return View(ListaDeAlunos);

            //    case "Matricula":
            //        if (ValorDaBusca == null) { return RedirectToAction("Index"); }
            //        BuscarPorMatricula(ValorDaBusca); break;

            //    case "Nome":
            //        if (ValorDaBusca == null) { return RedirectToAction("Index"); }
            //        return BuscarPorNome(ValorDaBusca);
            //}
            //return View();
        }

        public IActionResult BuscarPorMatricula(string valor)
        {
            if (!int.TryParse(valor, out int matricula))
            {
                ViewBag.Error = "Só aceita números";
                TempData["Error"] = ViewBag.Error;
                return RedirectToAction("Index");
            }
            var aluno = _repositorio.GetByMatricula(matricula);
            if (aluno == null) 
            {
                ViewBag.Error = "Aluno não encontrado";
                TempData["Error"] = ViewBag.Error;
                return RedirectToAction("Index"); 
            }
            var listaDeAlunosPorMatricula = new List<AlunoModel>
            {
                ConversaoDeTipos.ConversaoDomainparaModel(aluno)
            };

            return View(listaDeAlunosPorMatricula);
        }
        
        public IActionResult BuscarPorNome(string valor)
        {
            var ListaDeAlunosPorNome = ConversaoDeTipos.ConversaoListaDeDomainparaModel(_repositorio.GetByContendoNoNome(valor));
            
            if (ListaDeAlunosPorNome == null)
            {
                ViewBag.Error = "Aluno não encontrado";
                TempData["Error"] = ViewBag.Error;
                return RedirectToAction("Index");
            }
            return View(ListaDeAlunosPorNome);
        }

        public IActionResult CadastrarEditar(int id)
        {
            ViewBag.Error = TempData["Error"] as string;
            if (id == 0)
            {
                return View();
            }
            var aluno = _repositorio.GetByMatricula(id);
            if(aluno == null)
            {
                return View();
            }
            return View(ConversaoDeTipos.ConversaoDomainparaModel(aluno));
        }

        public IActionResult Cadastrar(AlunoModel alunoModel)
        {
            var aluno = ConversaoDeTipos.ConversaoDeModelParaDomain(alunoModel);
            
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
            var aluno = ConversaoDeTipos.ConversaoDeModelParaDomain(alunoModel);

            if (aluno.CPF != null)
            {
                if (!CpfUtils.IsCpf(aluno.CPF))
                {
                    ViewBag.Error = "CPF invalido";
                    TempData["Error"] = ViewBag.Error;
                    return RedirectToAction("CadastrarEditar", new { id = aluno.Matricula });
                }
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
            return RedirectToAction("Index");
        }
    }
}
