using Domain;
using System.Collections.Generic;

namespace Application.Common.Queries
{
    // SOLID Explanation: Dependency Inversion Principle
    // Abstractions should not depend upon  details. Details should depend upon  abstractions.
    // Imamo listu entiteta, tako da ova apstrakcija ne zavisi ni od jedne implementacije na primer SQL- i ni od jednog konkretnoog entiteta, dakle ni od jednog detalja.
    // Mozemo podjednako da koristimo svaku pojedinacnu implementaciju ovog interfejsa, za bilo koji entitet, i za bilo koju perzistenciju (in-memory, MS SQL Server, NoSQL...)
    public interface IGetListQuery<TEntity> where TEntity : Entity
    {
        List<TEntity> GetList();
    }
}
