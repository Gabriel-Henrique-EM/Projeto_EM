using EM_DomainAluno;
using EM_RepositorioAluno;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AlunoController : Controller
    {
        private readonly RepositorioAluno _repositorio;
        public AlunoController(RepositorioAluno repositorio)
        {
            _repositorio = repositorio;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cadastrar()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                _repositorio.Add(aluno);
                return RedirectToActionPermanentPreserveMethod("Index", "Home");
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var aluno = _repositorio.GetByMatricula(id);
            return View(aluno);
        }

        [HttpPost]
        public IActionResult Editar(Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                _repositorio.Update(aluno);
                return RedirectToActionPermanentPreserveMethod("Index", "Home");
            }
            return BadRequest(ModelState);
        }

        public IActionResult Deletar(int id)
        {
            var aluno = _repositorio.GetByMatricula(id);
            if (aluno != null)
            {
                _repositorio.Remove(aluno);
                return RedirectToActionPermanentPreserveMethod("Index", "Home");
            }
            return BadRequest(ModelState);
        }

    }
}
