using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;

namespace FGUFW.GamePlay
{
    public static class IPartExtensions
    {
        public static async Task Creating(this IPart self,IPart parent)
        {
            var task = self.OnCreating(parent);
            if(task!=null) await task;

            if(self.SubParts!=null)
            {
                foreach (var subPart in self.SubParts)
                {
                    task = subPart.Creating(self);
                    if(task!=null) await task;
                }
            }

            tryAddUpdate(self);
            tryAddCoroutine(self);
        }

        public static async Task Destroying(this IPart self,IPart parent)
        {
            tryRemoveUpdate(self);
            tryRemoveCoroutine(self);
            Task task = null;
            if(self.SubParts!=null)
            {
                foreach (var subPart in self.SubParts)
                {
                    task = subPart.Destroying(self);
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
            var subPart = self.SubParts.Find(sp=>sp is T);
            if(subPart!=null)
            {
                return (T)subPart;
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