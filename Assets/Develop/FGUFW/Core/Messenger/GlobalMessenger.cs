
namespace FGUFW
{
    static public class GlobalMessenger
    {
        static public IOrderedMessenger<string,object> M = new OrderedMessenger<string,object>();
    }
}