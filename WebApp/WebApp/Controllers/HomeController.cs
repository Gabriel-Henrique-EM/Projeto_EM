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

        public IActionResult Index()
        {
            return View();

        }

        public IActionResult Infos()
        {
            return View();
        }

        public IActionResult PageNotFound()
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