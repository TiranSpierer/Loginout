// Created by Tiran Spierer
// Created at 04/01/2023
// Class propose: Base class for entities. Method CopyValuesTo() is for the Repository to be able to update an entity. 

namespace DataAccess.DataModels;

public interface IEntity<in T> where T : class
{
    /// <summary>
    /// Don't copy over ID
    /// </summary>
    /// <param name="entity"></param>
    void CopyValuesTo(T entity);
}
