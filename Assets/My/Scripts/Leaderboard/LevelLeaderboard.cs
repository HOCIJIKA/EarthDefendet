using UnityEngine;
using UnityEngine.ProBuilder;

public class LevelLeaderboard : MonoBehaviour
{
    [SerializeField] private GameObject _playerStatsUIPref;
    [SerializeField] private Transform _playerStatsContainer;
    [SerializeField] private Transform _myStatsContainer;
    
    public void CreateNewPlayerStatsUI(int number, string name, string points, string level)
    {
        var playerStats = Instantiate(_playerStatsUIPref, _playerStatsContainer);
        playerStats.GetComponent<PlayerStats>().SetValues(number, name, points, level);
    }

    public void CreateMyStatsUI(LoadedUser myData)
    {
        var playerStats = Instantiate(_playerStatsUIPref, _myStatsContainer);
        playerStats.GetComponent<PlayerStats>().SetValues(myData.Number, myData.Username, myData.MaxPoints, myData.MaxPoints);
    }
}
