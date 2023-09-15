namespace FGUFW
{
    /// <summary>
    /// 可回收
    /// </summary>
    public interface IRecyclable<DATA>
    {
        void Init(DATA data);
        void Recycle();
    }
}