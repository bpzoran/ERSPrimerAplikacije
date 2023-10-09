using Domain;

namespace Application.Common.Commands
{
    public interface IDeleteCommand<TEntity> where TEntity : Entity
    {
        bool Execute(object id);
    }
}
