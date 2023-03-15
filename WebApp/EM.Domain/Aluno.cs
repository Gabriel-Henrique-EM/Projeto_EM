using EM_DomainEntidade;
using EM_DomainEnum;

namespace EM_DomainAluno
{
    public class Aluno : IEntidade
    {
        public int Matricula { get; set; }
        public string Nome { get; set; }
        public string? CPF { get; set; }
        public DateTime Nascimento { get; set; }
        public EnumeradorSexo Sexo { get; set; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
