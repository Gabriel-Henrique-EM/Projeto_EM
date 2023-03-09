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


        public IActionResult adicionar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult adicionar([FromBody]AlunoModal aluno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string cpf = aluno.CPF.Replace(".", "").Replace("-", "");
            Aluno newAluno = new Aluno()
            {
                Matricula = aluno.Matricula,
                Nome = aluno.Nome,
                CPF = cpf,
                Nascimento = aluno.Nascimento,
                Sexo = (EM_DomainEnum.EnumeradorSexo)aluno.Sexo
            };
            _repositorio.Add(newAluno);
            return RedirectToActionPermanentPreserveMethod("Index", "Home");
        }
    }
}
