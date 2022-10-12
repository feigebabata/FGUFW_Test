using FGUFW;
using FGUFW.ECS;

namespace GUNNAC
{
    public interface ICmd2Comp
    {
        void Convert(World world, UInts8 cmds);
    }
}