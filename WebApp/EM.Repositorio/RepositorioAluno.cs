using EM_DomainAluno;
using EM_DomainEnum;
using EM_RepositorioAbstrato;
using FirebirdSql.Data.FirebirdClient;
using System.Linq.Expressions;

namespace EM_RepositorioAluno
{
    public class RepositorioAluno : RepositorioAbstrato<Aluno>
    {
        private readonly FbConnectionStringBuilder _connectionString;
        public RepositorioAluno()
        {
            FbConnectionStringBuilder connectionString = new FbConnectionStringBuilder()
            {
                UserID = "SYSDBA",
                Password = "masterkey",
                Database = "C:\\Users\\Escolar Manager\\Desktop\\Projeto_EM\\DBALUNOS.FB4",
                DataSource = "192.168.1.143",
                Port = 3054
            };
            _connectionString = connectionString;
        }

        public override IEnumerable<Aluno> GetAll()
        {
            using (var connection = new FbConnection(_connectionString.ToString()))
            {
                var alunos = new List<Aluno>();
                try
                {
                    connection.Open();
                    string stringCommand = "SELECT * FROM TBALUNO;";
                    var command = new FbCommand(stringCommand, connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var aluno = new Aluno
                            {
                                Matricula = reader.GetInt32(reader.GetOrdinal("ALUMATRICULA")),
                                Nome = reader.GetString(reader.GetOrdinal("ALUNOME")),
                                CPF = reader.GetString(reader.GetOrdinal("ALUCPF")),
                                Sexo = (EnumeradorSexo)reader.GetInt32(reader.GetOrdinal("ALUSEXO")),
                                Nascimento = reader.GetDateTime(reader.GetOrdinal("ALUNASCIMENTO"))
                            };
                            alunos.Add(aluno);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Algo deu errado: " + ex);
                }
                return alunos;
            }
        }

        public Aluno GetByMatricula(int matricula)
        {
            using (var connection = new FbConnection(_connectionString.ToString()))
            {
                var alunos = new Aluno();
                try
                {
                    connection.Open();
                    string stringCommand = @"SELECT * FROM TBALUNO WHERE ALUMATRICULA = @Matricula;";
                    var command = new FbCommand(stringCommand, connection);
                    command.Parameters.Add("@Matricula", matricula);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var aluno = new Aluno
                            {
                                Matricula = reader.GetInt32(reader.GetOrdinal("ALUMATRICULA")),
                                Nome = reader.GetString(reader.GetOrdinal("ALUNOME")),
                                CPF = reader.GetString(reader.GetOrdinal("ALUCPF")),
                                Sexo = (EnumeradorSexo)reader.GetInt32(reader.GetOrdinal("ALUSEXO")),
                                Nascimento = reader.GetDateTime(reader.GetOrdinal("ALUNASCIMENTO"))
                            };
                            alunos = aluno;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Algo deu errado: " + ex);
                }
                return alunos;
            }
        }
        public IEnumerable<Aluno> GetByContendoNoNome(string nome)
        {
            using (var connection = new FbConnection(_connectionString.ToString()))
            {
                var alunos = new List<Aluno>();
                try
                {
                    connection.Open();
                    string stringCommand = $"SELECT * FROM TBALUNO WHERE ALUNOME LIKE '%{nome.ToLower()}%';";
                    var command = new FbCommand(stringCommand, connection);

                    using (var reader = command.ExecuteReader())
                         alunos = ReadAlunos(reader);
                }
                catch (Exception ex)
                {
                    throw new Exception("Algo deu errado: " + ex);
                }
                return alunos;
            }
        }

        public override IEnumerable<Aluno> Get(Expression<Func<Aluno, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override void Add(Aluno aluno)
        {
            string? CPF = aluno.CPF?.Replace(".", "").Replace("-", "");
            using (var connection = new FbConnection(_connectionString.ToString()))
            {
                connection.Open();
                try
                {
                    string stringCommand = "INSERT INTO TBALUNO (ALUMATRICULA, ALUNOME, ALUCPF, ALUNASCIMENTO, ALUSEXO) VALUES((GEN_ID(GEN_TBALUNO, 1)), @Nome, @CPF, @Data, @Sexo);";
                    var command = new FbCommand(stringCommand, connection);

                    command.Parameters.Add("@Nome", aluno.Nome.ToLower());
                    command.Parameters.Add("@CPF", CPF);
                    command.Parameters.Add("@Data", aluno.Nascimento.ToString("yyyy/MM/dd"));
                    command.Parameters.Add("@Sexo", (int)aluno.Sexo);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Algo deu errado: " + ex);
                }
            }
        }

        public override void Remove(Aluno aluno)
        {
            using (var connection = new FbConnection(_connectionString.ToString()))
            {
                connection.Open();
                try
                {
                    string stringCommand = @"DELETE FROM TBALUNO  WHERE ALUMATRICULA = @Matricula;";
                    var command = new FbCommand(stringCommand, connection);
                    command.Parameters.Add("@Matricula", aluno.Matricula);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Algo deu errado: " + ex);
                }
            }
        }

        public override void Update(Aluno aluno)
        {
            string? CPF = aluno.CPF?.Replace(".", "").Replace("-", "");
            using (var connection = new FbConnection(_connectionString.ToString()))
            {
                connection.Open();
                try
                {
                    string Data = aluno.Nascimento.ToString("yyyy-MM-dd");
                    string stringCommand = @"UPDATE TBALUNO SET ALUMATRICULA = @Matricula ,ALUNOME = @Nome,ALUCPF = @CPF, ALUNASCIMENTO = @Data, ALUSEXO = @Sexo
                                                WHERE ALUMATRICULA = @Matricula;";
                    var command = new FbCommand(stringCommand, connection);

                    command.Parameters.Add("@Matricula", aluno.Matricula);
                    command.Parameters.Add("@Nome", aluno.Nome.ToLower());
                    command.Parameters.Add("@CPF", CPF);
                    command.Parameters.Add("@Data", aluno.Nascimento.ToString("yyyy-MM-dd"));
                    command.Parameters.Add("@Sexo", (int)aluno.Sexo);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Algo deu errado: " + ex);
                }
            }
        }

        private static List<Aluno> ReadAlunos(FbDataReader reader)
        {
            var alunos = new List<Aluno>();
            while (reader.Read())
            {
                var aluno = new Aluno
                {
                    Matricula = reader.GetInt32(reader.GetOrdinal("ALUMATRICULA")),
                    Nome = reader.GetString(reader.GetOrdinal("ALUNOME")),
                    CPF = reader.GetString(reader.GetOrdinal("ALUCPF")),
                    Sexo = (EnumeradorSexo)reader.GetInt32(reader.GetOrdinal("ALUSEXO")),
                    Nascimento = reader.GetDateTime(reader.GetOrdinal("ALUNASCIMENTO"))
                };
                alunos.Add(aluno);
            }
            return alunos;
        }
    }
}