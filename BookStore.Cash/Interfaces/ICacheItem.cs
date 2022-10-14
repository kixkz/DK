namespace BookStore.Cash.Interfaces
{
    public interface ICacheItem<out T>
    {
        T GetKey();
    }
}
