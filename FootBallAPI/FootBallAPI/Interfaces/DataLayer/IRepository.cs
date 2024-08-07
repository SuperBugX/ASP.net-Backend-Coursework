namespace FootballAPI.Interfaces.DataLayer
{
    public interface IRepository<T> where T : class
    {
        public LinkedList<T> All();
        public T? Get(long id);
        public long Add(T entity);
        public bool DeleteAll();
        public bool Delete(long id);
        public bool Update(long id, T entity);
    }
}
