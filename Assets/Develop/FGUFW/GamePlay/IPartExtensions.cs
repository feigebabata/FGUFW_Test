using System.Collections.Generic;
using System.Threading.Tasks;
using FGUFW;

namespace GamePlay
{
    public static class IPartExtensions
    {
        public static async Task Creating(this IPart self,IPart parent)
        {
            await self.OnCreating(parent);

            if(self.SubParts!=null)
            {
                foreach (var subPart in self.SubParts)
                {
                    await subPart.Creating(self);
                }
            }

            tryAddUpdate(self);
        }

        public static async Task Destroying(this IPart self,IPart parent)
        {
            if(self.SubParts!=null)
            {
                foreach (var subPart in self.SubParts)
                {
                    await subPart.Destroying(self);
                }
            }

            tryRemoveUpdate(self);

            if(self.SubParts!=null)
            {
                self.SubParts.Clear();
                self.SubParts = null;
            }
            await self.OnDestroying(parent);
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
    }
}