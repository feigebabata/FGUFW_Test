using UnityEngine;

namespace FGUFW.ECS
{
    [DisallowMultipleComponent]
    public class ConvertToEntity : MonoBehaviour
    {
        void Awake()
        {
            if (World.Current == null)
            {
                Debug.LogWarning("找不到World实例");
                return;
            }

            var converts = GetComponents<IConvertToComponent>();
            var entityUId = World.Current.CreateEntity();
            foreach (var item in converts)
            {
                item.Convert(World.Current,entityUId);
                Destroy(item as MonoBehaviour);
            }
            Destroy(this);
        }
    }

    public interface IConvertToComponent
    {
        void Convert(World world, int entityUId);
    }
}