using System;

namespace FGUFW.ECS
{
    public interface ISystem:IDisposable
    {
        int Type{get;}
        int Order{get;}
        bool Enabled{get;set;}
        void OnInit(World world);
        void OnUpdate();
    }
}