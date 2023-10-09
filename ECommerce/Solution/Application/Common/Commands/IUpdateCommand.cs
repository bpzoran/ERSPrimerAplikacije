using Domain;

namespace Application.Common.Commands
{
    public interface IUpdateCommand<TEntity> where TEntity : Entity
    {
        bool Execute(TEntity entityToUpdate);
    }
}
