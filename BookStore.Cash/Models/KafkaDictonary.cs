namespace BookStore.Cash.Models
{
    public static class KafkaDictonary<TKey, TValue>
    {
        public static Dictionary<TKey, TValue> dataDictonary = new Dictionary<TKey, TValue>();
    }
}
