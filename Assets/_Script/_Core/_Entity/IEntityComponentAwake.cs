public interface IEntityComponentAwake<T> : IEntityComponent<T> where T : Entity<T>
{
    public void EntityComponentAwake(T entity);
}
