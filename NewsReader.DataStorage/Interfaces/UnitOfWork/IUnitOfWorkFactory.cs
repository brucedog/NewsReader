namespace NewsReader.DataStorage.Interfaces.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}