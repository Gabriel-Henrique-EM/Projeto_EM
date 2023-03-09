using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class AlunoModal
    {
        
        public int Matricula { get; set; }
        [StringLength(100, MinimumLength = 3,ErrorMessage = "O nome tem que ter de 3 a 100 caracteres")]
        public string Nome { get; set; }
        [Range(11, 14)]
        public string CPF { get; set; }
        public DateTime Nascimento { get; set; }
        public string NascimentoFormatado
        {
            get { return Nascimento.ToString("dd/MM/yyyy"); }
        }
        public EnumeradorSexo Sexo { get; set; }
    }
}
