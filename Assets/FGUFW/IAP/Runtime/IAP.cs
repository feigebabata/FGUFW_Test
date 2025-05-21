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
    1.在内购初始化前初始化GameServer 不确定
    2.初始化GameServer必须链接到你的ProjectID(Unity账号关联的项目)
    3.内购支持谷歌,亚马逊应用商店,三星,苹果,微软商店
    */
    public class IAP : MonoSingleton<IAP>, IDetailedStoreListener
    {
        private IStoreController _storeController;
        private IExtensionProvider _extensionProvider;

        private Action _onPurchasingInitSuccess;
        private Action<InitializationFailureReason, string> _onPurchasingInitFailed;
        private Action<bool, string> _onRestorePurchases;

        private HashSet<IPurchaseable> _purchaseables = new HashSet<IPurchaseable>();
        private bool _restore;

        protected override bool IsDontDestroyOnLoad() => true;

        public override void Dispose()
        {
            base.Dispose();

            _purchaseables.Clear();

            _onPurchasingInitSuccess = default;
            _onPurchasingInitFailed = default;
            _onRestorePurchases = default;


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
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
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
            Dictionary<string, ProductType> products,
            Action onPurchasingInitSuccess,
            Action<InitializationFailureReason, string> onPurchasingInitFailed,
            Action<bool, string> onRestorePurchases
        )
        {
            _onPurchasingInitSuccess = onPurchasingInitSuccess;
            _onPurchasingInitFailed = onPurchasingInitFailed;
            _onRestorePurchases = onRestorePurchases;

            var module = StandardPurchasingModule.Instance();
            module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
            var builder = ConfigurationBuilder.Instance(module);
            builder.useCatalogProvider = true;

            foreach (var (k, v) in products)
            {
                builder.AddProduct(k, v);
            }
            UnityPurchasing.Initialize(this, builder);
        }

        public bool Purchase(string productId)
        {
            if (_storeController == default) return false;

            Product product = _storeController.products.WithID(productId);
            if (product == default || !product.availableToPurchase)return false;

            _storeController.InitiatePurchase(productId);

            return true;
        }

        public void AddListener(IPurchaseable purchaseable)
        {
            _purchaseables.Add(purchaseable);
        }

        public void RemoveListener(IPurchaseable purchaseable)
        {
            _purchaseables.Remove(purchaseable);
        }

        /// <summary>
        /// 获取已购买的非消耗品和订阅
        /// 在安装后调用一次
        /// </summary>
        public void RestorePurchases()
        {
            if (_storeController == default) return;
            _restore = true;
            var apple = _extensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((succ, msg) =>
            {
                _restore = false;
                _onRestorePurchases?.Invoke(succ, msg);

            });
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
            foreach (var item in _purchaseables)
            {
                item.OnPurchaseFailed(product, failureDescription);
            }
        }

        //购买失败 弃用
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            //_onPurchaseFailed?.Invoke(product,failureReason);
        }

        //初始化失败
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            _onPurchasingInitFailed?.Invoke(error, string.Empty);
        }

        //初始化失败
        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            _onPurchasingInitFailed?.Invoke(error, message);
        }

        //购买成功
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            foreach (var item in _purchaseables)
            {
                item.ProcessPurchase(purchaseEvent, _restore);
            }
            return PurchaseProcessingResult.Complete;
        }
#endregion

    }
}


