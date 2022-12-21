using UnityEngine;

public class UnityAds : MonoBehaviour
{
/*#pragma warning disable 0649
#pragma warning disable 0649
    [Header("ID Гри")]
    [SerializeField] private string gameID;
    [Header("ID блоків")]
    [SerializeField] private string interEndLvlID;
    [SerializeField] private string interRestartLvlID;
    [SerializeField] private string rewardedMultipleCointID;
    [SerializeField] private string BannerTopID;

#pragma warning restore 0649

    public void Init()
    {
        Advertisement.Initialize(gameID);
        Advertisement.Load(interEndLvlID);
        Advertisement.Load(interRestartLvlID);
        Advertisement.Load(rewardedMultipleCointID);
        Advertisement.Load(BannerTopID);
        StartCoroutine(ShowBannerWhenReady());  
    }

    public void StartBanner()
    {
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Load(BannerTopID);
        //StartCoroutine(DelayShowBanner());
        StartCoroutine(ShowBannerWhenReady());
    }
    
    public void HideBanner()
    {
        Advertisement.Banner.Hide();
    }

    private IEnumerator ShowBannerWhenReady()
    {   
        while (!Advertisement.IsReady(BannerTopID))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(BannerTopID);
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
    }

    IEnumerator DelayShowBanner()
    {
        yield return new WaitForSeconds(2);
        Advertisement.Banner.Show(BannerTopID);
    }

    public void ShowInterstitialEndLvl()
    {
        if (Advertisement.IsReady(interEndLvlID))
            Advertisement.Show(interEndLvlID);

        Advertisement.Load(interEndLvlID);
    }

    public void ShowInterstitialRestartLvl()
    {
        if (Advertisement.IsReady(interRestartLvlID))
            Advertisement.Show(interRestartLvlID);

        Advertisement.Load(interRestartLvlID);
    }

    public void ShowAdMultipleCoins()
    {
        if (Advertisement.IsReady(rewardedMultipleCointID))
            Advertisement.Show(rewardedMultipleCointID);

        Advertisement.Load(rewardedMultipleCointID);
    }

    #region Result
    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("OnUnityAdsDidError");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("OnUnityAdsDidFinish");
        switch (showResult)
        {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Finished:
                break;
            default:
                break;
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("OnUnityAdsDidStart");
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("OnUnityAdsReady");
    }
    #endregion*/

}
