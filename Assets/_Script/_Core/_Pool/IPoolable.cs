namespace Custom.Pool
{
    public interface IPoolable
    {
        /// <summary>
        /// set object state created inside pool
        /// </summary>
        void OnCreate() { }
        void OnPush() { }
        void OnPop();
    }
}
