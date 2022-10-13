namespace FGUFW.ECS
{
    public interface IWorldUpdateControl
    {
        void OnPreUpdate(World world);
        void OnPreWorldUpdate(World world);
        bool CanUpdate(World world);
    }
}