namespace FGUFW.GamePlay
{
    public interface IEnabled
    {
        bool Enable{get;}

        void OnEnable();

        void OnDisable();
    }

    public static class IEnabledExtensions
    {
        public static void SetEnable(IEnabled self,bool enable)
        {
            if(enable==self.Enable)return;
            if(enable)
            {
                self.OnEnable();
            }
            else
            {
                self.OnDisable();
            }
            if(enable!=self.Enable)
            {
                throw new System.Exception("IEnabled.Enable数值异常");
            }
        }        
        
    }
    
}