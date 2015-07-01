namespace Assets.Scripts.Interfaces
{
    public interface ITwoDirectionalQueue<T>
    {
        void PushLeft(T obj);
        void PushRight(T obj);
        T PopLeft();
        T PopRight();
        T PeekLeft();
        T PeekRight();
        T Left{get;}
        T Right { get; }
    }
}