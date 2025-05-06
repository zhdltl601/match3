public interface IEntityComponentStart<T> : IEntityComponent<T> where T : Entity<T>
{
    public void EntityComponentStart(T entity);
}
