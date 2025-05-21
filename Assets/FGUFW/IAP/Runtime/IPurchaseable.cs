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
    public interface IPurchaseable
    {
        /// <summary>
        /// 所有回调支付成功的回调
        /// </summary>
        /// <param name="purchaseEvent"></param>
        /// <param name="restore"></param>
        void ProcessPurchase(PurchaseEventArgs purchaseEvent, bool restore);
        
        void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription);
    }
}