using Cysharp.Threading.Tasks;

namespace FGUFW.MonoGameplay
{
    public interface IPartPreload
    {
        UniTask OnPreload();
    }
}