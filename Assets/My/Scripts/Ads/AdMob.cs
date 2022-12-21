using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMob : MonoBehaviour
{
    [SerializeField] private string _appId = "ca-app-pub-6901847158346689~3490929628";
    [SerializeField] private string _bannerTopId = "ca-app-pub-6901847158346689/8551684618";
    [SerializeField] private string _interstitialAndLvlId = "ca-app-pub-6901847158346689/6194675081";
    [SerializeField] private string _interstitialRestartLvlId = "ca-app-pub-6901847158346689/7127950941";
    [SerializeField] private string _rewardedMultipleCoinsId = "ca-app-pub-6901847158346689/4933564300";

    private bool _testMode;
    private BannerView _bannerView;
    private InterstitialAd _interstitialAndLvl;
    private InterstitialAd _interstitialRestartLvl;
    private RewardedAd _rewardedAdMultipleCoins;

    public void Init( bool testMod)
    {
        _testMode = testMod;
        if (_testMode)
        {
            Debug.Log($"Google Ads TestMode: {_testMode}");
            _bannerTopId = "ca-app-pub-3940256099942544/6300978111";
            _interstitialAndLvlId = "ca-app-pub-3940256099942544/1033173712";
            _interstitialRestartLvlId = "ca-app-pub-3940256099942544/1033173712";
            _rewardedMultipleCoinsId = "ca-app-pub-3940256099942544/5224354917";
        }

        MobileAds.SetiOSAppPauseOnBackground(true);
        MobileAds.Initialize(appCallback =>{});

        //RequestInterstitialAdnLvl(); TODO: for future
        RequestInterstitialRestartLvl();
        CreateAndLoadRewardedMultipleCoin();
    }

    public void HideBanner()
    {
        _bannerView?.Hide();
    }

    #region CallAds
    
    public void RequestBanner()
    {
        _bannerView = new BannerView(_bannerTopId, AdSize.Banner, AdPosition.Top);
        AdRequest request = new AdRequest.Builder().Build();
        _bannerView.LoadAd(request);
    }

    /// <summary>
    /// Міжсторінкове оголошеня кінця рівня 
    /// </summary>
    public void ShowInterstitialAdnLvl()
    {
        _interstitialAndLvl.Show();
        RequestInterstitialAdnLvl();
    }
    public void ShowInterstitialRestartLvl()
    {
        _interstitialRestartLvl.Show();
        RequestInterstitialRestartLvl();
    }

    /// <summary>
    /// Відео з нагородою, примноження монеток.
    /// </summary>
    public void ShowRewardedMultipleCoins()
    {
        _rewardedAdMultipleCoins.Show();
        CreateAndLoadRewardedMultipleCoin();
    }

    #endregion

    #region Requests

    private void RequestInterstitialAdnLvl()
    {
        _interstitialAndLvl = new InterstitialAd(_interstitialAndLvlId);

        AdRequest request = new AdRequest.Builder().Build();
        _interstitialAndLvl.LoadAd(request);
    }

    private void RequestInterstitialRestartLvl()
    {
        _interstitialRestartLvl = new InterstitialAd(_interstitialRestartLvlId);

        AdRequest request = new AdRequest.Builder().Build();
        _interstitialRestartLvl.LoadAd(request);
    }

    private void CreateAndLoadRewardedMultipleCoin()
    {
        // Create new rewarded ad instance.
        _rewardedAdMultipleCoins = new RewardedAd(_rewardedMultipleCoinsId);
        // Called when an ad request has successfully loaded.
        _rewardedAdMultipleCoins.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        _rewardedAdMultipleCoins.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        _rewardedAdMultipleCoins.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        _rewardedAdMultipleCoins.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        _rewardedAdMultipleCoins.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        _rewardedAdMultipleCoins.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        _rewardedAdMultipleCoins.LoadAd(request);
    }
    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLoaded event received");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleInterstitialFailedToLoad event received with message: " + args.LoadAdError);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialOpened event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleInterstitialLeftApplication event received");
    }

    #endregion

    #region RewardedAd callback handlers

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: " + e.LoadAdError);
    }

    private void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: " + args.Message);
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    /// <summary>
    /// Умова виконана, можна зараховувати нагороду.
    /// </summary>
    private void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
        
        if (!Social.localUser.authenticated) return;
        Social.ReportProgress(GPGSIds.achievement_first_using_ads, 100.0f, success => { });
        Social.ReportProgress(GPGSIds.achievement_collect_multiply_points, 100.0f, success => { });
    }
    
    #endregion
}
