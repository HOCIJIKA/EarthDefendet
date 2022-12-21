using UnityEngine;

public class Advertising : MonoBehaviour
{
	private enum AdsStore { UnityAds, AdMob};
	public static Advertising Instance => _instance;

	[SerializeField]private bool _onAds = true;
	[SerializeField]private bool _showBanner = true;

	[SerializeField] private AdsStore _adsStore;
	[SerializeField] private bool _testMod = false;

	private AdMob _adMob;
	private UnityAds _unityAds;

	private static Advertising _instance;

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);

		_adMob = gameObject.GetComponent<AdMob>();
		_unityAds = gameObject.GetComponent<UnityAds>();
	}

    public void Init()
    {
	    if (_onAds)
        {
	        switch (_adsStore)
	        {
		        case AdsStore.AdMob:
			        _adMob.Init(_testMod);
				
			        if (_showBanner && !IsAdsOff())
			        {
				        _adMob.RequestBanner();
			        }
			        break;
		        
		        case AdsStore.UnityAds:
			        //_unityAds.Init(_testMode, _showBanner);
			        break;
	        }
        }
    }

    /// <summary>
	/// Міжсторінкове оголошеня кінця рівня 
	/// </summary>
	public void ShowInterstitialAdnLvl()
    {
	    switch (_adsStore)
		{
			case AdsStore.AdMob:
				_adMob.ShowInterstitialAdnLvl();
				break;
		        
			case AdsStore.UnityAds:
				//_unityAds.ShowInterstitialEndLvl();
				break;
		}
    }

	/// <summary>
	/// Міжсторінкове оголошеня при рестарта гри
	/// </summary>
	public void ShowInterstitialRestartLvl()
	{
		if (IsAdsOff())
		{
			switch (_adsStore)
			{
				case AdsStore.AdMob:
					_adMob.ShowInterstitialRestartLvl();
					break;

				case AdsStore.UnityAds:
					//_unityAds.ShowInterstitialRestartLvl();
					break;
			}
		}
	}

	/// <summary>
	/// Міжсторінкове оголошеня при відео реклама множення монеток в кінці рівня.
	/// </summary>
	public void ShowRewardedMultipleCoins()
	{
		switch (_adsStore)
		{
			case AdsStore.AdMob:
				_adMob.ShowRewardedMultipleCoins();
				break;
		        
			case AdsStore.UnityAds:
				//_unityAds.ShowAdMultipleCoins();
				break;
		}
    }

	// переобити на "якщо удачно показали рекламу". Бо заспамлять.

	public void ShowAdsAddPoints(int coins)
    {
	    PointsCollector.Instance.AddPoints(1000, transform);
		FX.Instance.PlayButtonsShopBy();

		switch (_adsStore)
		{
			case AdsStore.AdMob:
				_adMob.ShowRewardedMultipleCoins();
				break;
		        
			case AdsStore.UnityAds:
				//_unityAds.ShowAdMultipleCoins();
				break;
		}
		
		MenuUI.Instance.HideRememberAdsKnob();
    }

	public void HideBanner()
	{
		switch (_adsStore)
		{
			case AdsStore.AdMob:
				_adMob.HideBanner();
				break;
		        
			case AdsStore.UnityAds:
				//_unityAds.HideBanner();
				break;
		}
	}

	private bool IsAdsOff()
	{
		return PlayerPrefs.GetInt("AdsIsRemoved") == 1;
	}
}
