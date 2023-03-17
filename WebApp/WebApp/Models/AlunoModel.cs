using System.ComponentModel.DataAnnotations;
using System.Globalization;
using WebApp.Helpers;

namespace WebApp.Models
{
    public class AlunoModel
    {
        
        public int Matricula { get; set; }

        [Required(ErrorMessage = "Campo obrigatorio!")]
        [RegularExpression(@"^(?!\s*$)[a-zA-ZÀ-ÿ\s]*$", ErrorMessage = "O campo de nome não deve conter números ou caracteres especiais.")]
        [StringLength(100, MinimumLength = 3,ErrorMessage = "O nome tem que ter de 3 a 100 caracteres")]
        public string Nome { get; set; }

        [StringLength(14, MinimumLength = 11, ErrorMessage = "O CPF deve conter 11 números!")]
        public string? CPF { get; set; }

        [Required(ErrorMessage = "Campo obrigatorio!")]
        public DateTime Nascimento { get; set; }

        [Required(ErrorMessage = "Campo obrigatorio!")]
        public EnumeradorSexo Sexo { get; set; }

        public string NascimentoFormatado => Nascimento.ToString("dd/MM/yyyy");
        public string NomeFormatado => string.IsNullOrEmpty(Nome) ? "" : 
            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(this.Nome);
        public string CPFFormatado => 
            string.IsNullOrEmpty(CPF) ? "" : 
            string.Format("{0:000\\.000\\.000-00}", 
                long.Parse(CPF.Replace(".", "").Replace("-", "")));
    }
}
