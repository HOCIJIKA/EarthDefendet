using UnityEngine;

public static class InAppPurchased
{
    /// <summary>
    /// Purchase Points/ PointsCollector.Instance.AddPoints()
    /// </summary>
    /// <param name="countPoints">how much count of points well be added to player</param>
    public static void PurchasePoints(int countPoints)
    {
        Debug.Log($"Purchased:{countPoints} points!");
        PointsCollector.Instance.AddPoints(countPoints, null);
        
        if (!Social.localUser.authenticated) return;
        Social.ReportProgress(GPGSIds.achievement_first_buy_points, 100.0f, success =>{});
    }
}
