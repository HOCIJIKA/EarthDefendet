using System.Collections;
using UnityEngine;

public class Shop : MonoBehaviour
{
	[SerializeField] private float _timeToRememberKnob = 10f;
	private float _startTime;
	private int _purchaseCode = -1;

	private enum PurchaseCode
	{
		Purchase1 = 1,
		Purchase5 = 5,
		Purchase10 = 10,
		Purchase20 = 20,
		Purchase50 = 50,
		Purchase100 = 100,
		
		PurchaseOffAds = 201,
		
		PurchaseDonate1 = 301,
		PurchaseDonate2 = 302,
		PurchaseDonate3 = 303
	}
	
	private static Shop instance;
	public static Shop Instance => instance;

	private void Awake()
    {
        if (instance == null)
        {
			instance = this;
        }
    }

    private FX fX;
    private void Start()
    {
		fX = FindObjectOfType<FX>();
		_startTime = Time.time;
		_timeToRememberKnob += _startTime;

		StartCoroutine(TimerToKnob());
    }

    public void SetPurchaseCode( int _code)
    {
	    _purchaseCode = _code;
    }

	/// <summary>
	///викликаєм покупкув залежності скільки гравець купляє.
	/// </summary>
	/// <param name="countShopPoints"> кількість очок задається з кнопки в едіторі</param>
	public void ByCoins()
	{
		fX.PlayButtonsShopBy();

		switch (_purchaseCode)
		{
			case (int)PurchaseCode.Purchase1:
				InAppPurchased.PurchasePoints(1000);
				_purchaseCode = -1;

				break;

			case (int)PurchaseCode.Purchase5:
				InAppPurchased.PurchasePoints(5000);
				_purchaseCode = -1;

				break;

			case (int)PurchaseCode.Purchase10:
				InAppPurchased.PurchasePoints(10000);
				_purchaseCode = -1;

				break;

			case (int)PurchaseCode.Purchase20:
				InAppPurchased.PurchasePoints(20000);
				_purchaseCode = -1;

				break;

			case (int)PurchaseCode.Purchase50:
				InAppPurchased.PurchasePoints(50000);
				_purchaseCode = -1;

				break;

			case (int)PurchaseCode.Purchase100:
				InAppPurchased.PurchasePoints(100000);
				_purchaseCode = -1;
				
				break;
		}

	}
	
	public void OffAds()
	{
		Debug.Log(_purchaseCode);

		if (_purchaseCode != (int)PurchaseCode.PurchaseOffAds)
		{
			return;
		}

		fX.PlayButtonsShopBy();
		PlayerPrefs.SetInt("AdsIsRemoved", 1);
		PlayerPrefs.Save();
		Advertising.Instance.HideBanner();
		Debug.Log($"Ads is off");
		
		_purchaseCode = -1;
		
		if (!Social.localUser.authenticated) return;
		Social.ReportProgress(GPGSIds.achievement_disable_ads, 100.0f, success =>{});
	}
	
	public void Donate()
	{
		Debug.Log(_purchaseCode);
		fX.PlayButtonsShopBy();

		//TODO: Add Achievements
		switch (_purchaseCode)
		{
			case (int)PurchaseCode.PurchaseDonate1:
				Debug.Log($"Donated1");
				_purchaseCode = -1;

				if (!Social.localUser.authenticated) break;
				Social.ReportProgress(GPGSIds.achievement_donate_level_one, 100.0f, success =>{});
				break;

			case (int)PurchaseCode.PurchaseDonate2:
				Debug.Log($"Donated2");
				_purchaseCode = -1;
				
				if (!Social.localUser.authenticated) break;
				Social.ReportProgress(GPGSIds.achievement_donate_level_two, 100.0f, success =>{});
				break;

			case (int)PurchaseCode.PurchaseDonate3:
				Debug.Log($"Donated3");
				_purchaseCode = -1;
				
				if (!Social.localUser.authenticated) break;
				Social.ReportProgress(GPGSIds.achievement_donate_level_three, 100.0f, success =>{});
				break;
		}
	}

	private IEnumerator TimerToKnob()
	{
		yield return new WaitUntil(() => (_startTime += Time.deltaTime) >= _timeToRememberKnob);
		MenuUI.Instance.ShowRememberKnob();
	}
}
