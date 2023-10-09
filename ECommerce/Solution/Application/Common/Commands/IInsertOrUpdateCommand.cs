using Domain;

namespace Application.Common.Commands
{
    public interface IInsertOrUpdateCommand<TEntity> where TEntity : Entity
    {
        bool Execute(TEntity entity);
    }
}
