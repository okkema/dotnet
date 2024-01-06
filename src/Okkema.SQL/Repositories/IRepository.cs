using Okkema.SQL.Entities;
namespace Okkema.SQL.Repositories;
public interface IRepository<T> where T : EntityBase
{
    /// <summary>
    /// Create a new entity in database
    /// </summary>
    /// <param name="entity">Entity to create</param>
    /// <returns>Number of rows affected</returns>
    public int Create(T entity);
    /// <summary>
    /// Read an entity from the database, returning null if it does not exist
    /// </summary>
    /// <param name="key">Entity system key</param>
    /// <returns>Entity</returns>
    public T? Read(Guid key);
    /// <summary>
    /// Update an entity in the database
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <returns>Number of rows affected</returns>
    public int Update(T entity);
    /// <summary>
    /// Delete an entity from the database
    /// </summary>
    /// <param name="key">Entity system key</param>
    /// <returns>Number of rows affected</returns>
    public int Delete(Guid key);
    /// <summary>
    /// List entities from the database
    /// </summary>
    /// <returns>Entities</returns>
    // public ICollection<T> List();
}