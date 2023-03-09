using EM_DomainEntidade;
using System.Linq.Expressions;

namespace EM_RepositorioAbstrato
{
    public abstract class RepositorioAbstrato<T> where T : IEntidade
    {
        public abstract void Add(T item);
        public abstract void Remove(T item);
        public abstract void Update(T item);
        public abstract IEnumerable<T> GetAll();
        public abstract IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
    }
}
