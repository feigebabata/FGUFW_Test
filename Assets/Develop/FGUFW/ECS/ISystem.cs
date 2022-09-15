using System;

namespace FGUFW.ECS
{
    public interface ISystem:IDisposable
    {
        int Order{get;}
        void OnInit(World world);
        void OnUpdate();
    }
}