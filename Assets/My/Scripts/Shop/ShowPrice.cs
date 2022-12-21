using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class ShowPrice : MonoBehaviour, IStoreListener
{
    [SerializeField] private Text _byMonets1kButtonCostText;
    [SerializeField] private Text _byMonets5kButtonCostText;
    [SerializeField] private Text _byMonets10kButtonCostText;
    [SerializeField] private Text _byMonets20kButtonCostText;
    [SerializeField] private Text _byMonets50kButtonCostText;
    [SerializeField] private Text _byMonets100kButtonCostText;
    [Space]
    [SerializeField] private Text _offAdsButtonCostText;
    [Space]
    [SerializeField] private Text _donate1ButtonCostText;
    [SerializeField] private Text _donate5ButtonCostText;
    [SerializeField] private Text _donate10ButtonCostText;

    private void OnEnable()
    {
        // var asd = FindObjectOfType<SubscriptionInfo>().getSkuDetails("asd");
        // _priceText.text = m_StoreController.products.WithID("pack_gold1").metadata.localizedDescription;
        // _priceText.text = SubscriptionInfo.
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"OnInitializeFailed: {error.ToString()}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        return PurchaseProcessingResult.Complete;
    }
    
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _byMonets1kButtonCostText.text = controller.products.WithID("removeads").metadata.localizedPriceString;
        _byMonets5kButtonCostText.text = controller.products.WithID("removeads").metadata.localizedPriceString;
        _byMonets10kButtonCostText.text = controller.products.WithID("removeads").metadata.localizedPriceString;
        _byMonets20kButtonCostText.text = controller.products.WithID("removeads").metadata.localizedPriceString;
        _byMonets50kButtonCostText.text = controller.products.WithID("removeads").metadata.localizedPriceString;
        _byMonets100kButtonCostText.text = controller.products.WithID("removeads").metadata.localizedPriceString;
        
        _offAdsButtonCostText.text = controller.products.WithID("removeads").metadata.localizedPriceString;
        
        _donate1ButtonCostText.text = controller.products.WithID("removeads").metadata.localizedPriceString;
        _donate5ButtonCostText.text = controller.products.WithID("removeads").metadata.localizedPriceString;
        _donate10ButtonCostText.text = controller.products.WithID("removeads").metadata.localizedPriceString;
    }
}
