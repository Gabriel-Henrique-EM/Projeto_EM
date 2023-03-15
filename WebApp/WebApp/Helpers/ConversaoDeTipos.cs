using EM_DomainAluno;
using WebApp.Models;

namespace WebApp.Helpers
{
    public static class  ConversaoDeTipos
    {
        public static AlunoModel conversaoDomainparaModel(Aluno aluno)
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
        public static Aluno conversaoDeModelParaDomain(AlunoModel alunoModel)
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
        public static IEnumerable<AlunoModel> conversaoListaDeDomainparaModel(IEnumerable<Aluno> alunosDomain)
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
