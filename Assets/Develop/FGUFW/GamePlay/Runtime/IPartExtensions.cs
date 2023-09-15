using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;

namespace FGUFW.GamePlay
{
    public static class IPartExtensions
    {
        public static async Task Create(this IPart self,IPart parent)
        {
            var task = self.OnCreating(parent);
            if(task!=null) await task;

            if(self.SubParts!=null)
            {
                foreach (var subPart in self.SubParts)
                {
                    task = subPart.Create(self);
                    if(task!=null) await task;
                }
            }

            tryAddUpdate(self);
            tryAddCoroutine(self);
        }

        public static async Task Destroy(this IPart self,IPart parent)
        {
            tryRemoveUpdate(self);
            tryRemoveCoroutine(self);
            Task task = null;
            if(self.SubParts!=null)
            {
                foreach (var subPart in self.SubParts)
                {
                    task = subPart.Destroy(self);
                    if(task!=null) await task;
                }
            }

            if(self.SubParts!=null)
            {
                self.SubParts.Clear();
                self.SubParts = null;
            }
            task = self.OnDestroying(parent);
            if(task!=null) await task;
        }

        public static T GetPart<T>(this IPart self) where T : IPart
        {
            int length = self.SubParts.Count;
            for (int i = 0; i < length; i++)
            {
                if(self.SubParts[i] is T)
                {
                    return (T)self.SubParts[i];
                }
            }
            return default(T);
        }

        private static void tryAddUpdate(IPart self)
        {
            if(self is IUpdate)
            {
                var iUpdate = self as IUpdate;
                iUpdate.AddUpdateToPlayerLoop();
            }
        }

        private static void tryRemoveUpdate(IPart self)
        {
            if(self is IUpdate)
            {
                var iUpdate = self as IUpdate;
                iUpdate.RemoveUpdateToPlayerLoop();
            }
        }

        private static void tryAddCoroutine(IPart self)
        {
            if(self is ICoroutineable)
            {
                var coroutineable = self as ICoroutineable;
                coroutineable.AddCoroutineBehaviour();
            }
        }

        private static void tryRemoveCoroutine(IPart self)
        {
            if(self is ICoroutineable)
            {
                var coroutineable = self as ICoroutineable;
                coroutineable.AddCoroutineBehaviour();
            }
        }
    }
}