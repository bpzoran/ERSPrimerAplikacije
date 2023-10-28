using Domain;

namespace Application.Common.Queries
{
    // SOLID Explanation
    // Interface segregation principle
    // Za svaki Query ili Command imamo poseban interfejs. 
    // Tako mozemo da koristimo klase samo onih metoda koji su nam zaista potrebni.
    // Mogli smo da imamo na prvi pogled nesto intuitivniji pristup, i da na primer u istoj klasi imamo FindById i GetList.
    // U tom slucaju bismo na primer u klasi CommonOrderCreator morali da importujemo tu klasu, a od metoda bismo koristili samo FindById, sto ne bi bilo u skladu sa
    // interface segregation principom
    public interface IFindByIdQuery<TEntity> where TEntity : Entity
    {
        TEntity FindById(object id);
    }
}
