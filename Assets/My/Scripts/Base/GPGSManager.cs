using System.Collections;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GPGSManager : MonoBehaviour
{
    public static GPGSManager Instance => _instance;

    private bool _isAuthenticationSuccess;
    private static GPGSManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    _isAuthenticationSuccess = true;
                }
                else
                {
                    _isAuthenticationSuccess = false;
                }
            });
        }
        
        StartCoroutine(tryConnection());
    }

    private IEnumerator tryConnection()
    {
        yield return new WaitForSeconds(5);
        
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    _isAuthenticationSuccess = true;
                }
                else
                {
                    _isAuthenticationSuccess = false;

                    StartCoroutine(tryConnection());
                }
            });
        }
    }

    private void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            _isAuthenticationSuccess = true; 
            //MenuUI.Instance.LeaderboardButon.interactable = _isAuthenticationSuccess;
        }
        else
        {
            _isAuthenticationSuccess = false;
            //MenuUI.Instance.LeaderboardButon.interactable = _isAuthenticationSuccess;
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }

    #region Leaderboards
    
    public void ShowLevelLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }
    
    public void PostScoreToLeaderboard(int currentLevelPoints)
    {
        if (!Social.localUser.authenticated) return;

        Social.ReportScore(currentLevelPoints, GPGSIds.leaderboard_levels_leaderboard,
            (bool success) =>
            {
                Debug.Log(
                    $"posted level vale: {currentLevelPoints} to leaderboard^ {GPGSIds.leaderboard_points_leaderboard}");
            });
    }
    
    public void PostLevelToLeaderboard(int currentLevel)
    {
        if (!Social.localUser.authenticated) return;

        Social.ReportScore(currentLevel, GPGSIds.leaderboard_levels_leaderboard,
            (bool success) =>
            {
                Debug.Log(
                    $"posted level vale: {currentLevel} to leaderboard^ {GPGSIds.leaderboard_levels_leaderboard}");
            });
    }
    #endregion

    #region Achievement

    public void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }

    public void CollAchievementOfPoints(int points)
    {
        if (!Social.localUser.authenticated) return;
        
        switch (points)
        {
            case > 1000 and < 5000:
                Social.ReportProgress(GPGSIds.achievement_collect_1000_points, 100.0f, success =>{});
                break;
            
            case > 5000 and < 20000:
                Social.ReportProgress(GPGSIds.achievement_collect_5000_points, 100.0f, success =>{});
                break;
            
            case > 20000 and < 50000:
                Social.ReportProgress(GPGSIds.achievement_collect_20000_points, 100.0f, success =>{});
                break;
            
            case > 50000 and < 100000:
                Social.ReportProgress(GPGSIds.achievement_collect_100000_points, 100.0f, success =>{});
                break;
        }
    }

    public void ReportAchievement(string achievement)
    {
        if (!Social.localUser.authenticated) return;

        Social.ReportProgress(achievement, 100.0f, (bool success) =>
        {
            if (success)
            {
                Debug.Log($"Achievement {achievement} successful added");
            }
            else
            {
                Debug.LogWarning($"Achievement {achievement} fail to added");
            }
        });
    }

    #endregion
    
}