using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace FGUFW.IAPs
{
/*
1.在内购初始化前初始化GameServer不确定
2.初始化GameServer必须链接到你的ProjectID(Unity账号关联的项目)
3.内购支持谷歌,亚马逊应用商店,三星,苹果,微软商店
*/
    public class IAP : MonoSingleton<IAP>,IDetailedStoreListener
    {
        private IStoreController _storeController;
        private IExtensionProvider _extensionProvider;
        private Func<PurchaseEventArgs, PurchaseProcessingResult> _onPurchaseSuccess;
        private Action<Product, PurchaseFailureDescription> _onPurchaseFailed;
        private Action _onPurchasingInitSuccess;
        private Action<InitializationFailureReason, string> _onPurchasingInitFailed;

        protected override bool IsDontDestroyOnLoad()=>true;

        public override void Dispose()
        {
            base.Dispose();

            _onPurchaseSuccess = default;
            _onPurchaseFailed = default;
            _onPurchasingInitSuccess = default;
            _onPurchasingInitFailed = default;

            _storeController = default;
            _extensionProvider = default;
        }

        /// <summary>
        /// 在内购初始化前是否初始化Server
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public async Task<string> InitializeProductionServer()
        {
            if(UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                var options = new InitializationOptions().SetEnvironmentName("production");

                try
                {
                    await UnityServices.InitializeAsync(options);
                }
                catch (System.Exception ex)
                {
                    return ex.Message;
                }
            }
            
            return string.Empty;
        }

        public void InitializePurchasing
        (
            Dictionary<string,ProductType> products,
            Func<PurchaseEventArgs,PurchaseProcessingResult> onPurchaseSuccess, 
            Action<Product,PurchaseFailureDescription> onPurchaseFailed,
            Action onPurchasingInitSuccess, 
            Action<InitializationFailureReason,string> onPurchasingInitFailed
        )
        {
            _onPurchaseSuccess = onPurchaseSuccess;
            _onPurchaseFailed = onPurchaseFailed;
            _onPurchasingInitSuccess = onPurchasingInitSuccess;
            _onPurchasingInitFailed = onPurchasingInitFailed;

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var (k,v) in products)
            {
                builder.AddProduct(k,v);
            }
            UnityPurchasing.Initialize(this, builder);
        }

        public void Purchase(string productId)
        {
            _storeController?.InitiatePurchase(productId);
        }

#region  IDetailedStoreListener

        //初始化成功
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _extensionProvider = extensions;
            
            _onPurchasingInitSuccess?.Invoke();
        }

        //购买失败
        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            _onPurchaseFailed?.Invoke(product,failureDescription);
        }

        //购买失败 弃用
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            //_onPurchaseFailed?.Invoke(product,failureReason);
        }

        //初始化失败
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            _onPurchasingInitFailed?.Invoke(error,string.Empty);
        }

        //初始化失败
        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            _onPurchasingInitFailed?.Invoke(error,message);
        }

        //购买成功
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            if(_onPurchaseSuccess!=default)
            {
                return _onPurchaseSuccess(purchaseEvent);
            }
            return PurchaseProcessingResult.Complete;
        }
#endregion

    }
}


