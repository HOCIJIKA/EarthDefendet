using System.Collections;
using System.Linq;
using Firebase;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
	public Button LeaderboardButon;
	public static MenuUI Instance => _instance;

	[Header("Multiple points button")]
	[SerializeField] private Button addPointsAdsButton;
	[Header("Меню")]
	[SerializeField] private GameObject menuPanel;
	[SerializeField] private Toggle foneMusicOn;
	[SerializeField] private Toggle soundEffectOn;
	[SerializeField] private Toggle vibrOn;
	[Header("Shop")]
	[SerializeField] private GameObject shopPanel;
	[Header("Loss")]
	[SerializeField] private GameObject lossPanel;
	[Header("UpPanelElements")]
	[SerializeField] private Text PointsText;
	[SerializeField] private Slider sliderEarthHP;
	[Space(10)]
	[SerializeField] private Text RocketCountText;

	[SerializeField] private GameObject _rememberKnobMenuButton;
	[SerializeField] private GameObject _rememberKnobShopButton;
	[SerializeField] private GameObject _rememberKnobAdsButton;

	[SerializeField] private GameObject _selectSkillText;

	[SerializeField] private LevelLeaderboard _levelLeaderboard;
	
	private static MenuUI _instance;
	private FX _fX;

	private void Awake()
	{
		if (_instance == null)
			_instance = this;
	}

    public void Init()
	{
		_fX = FindObjectOfType<FX>();
		SetToggleParam();
	}

	#region Menu

	#region Setting Toggles

	private void SetToggleParam()
    {
		foneMusicOn.isOn = GameC.Instance.GetData().toggleFoneMusic;
		soundEffectOn.isOn = GameC.Instance.GetData().toggleSoundEffect;
		vibrOn.isOn = GameC.Instance.GetData().toggleVibro;
	}

	public void SetToggleFoneMusic(bool f)
	{
		_fX.PlayButtonsClip();
		GameC.Instance.GetData().toggleFoneMusic = f;
		foneMusicOn.isOn = f;
		GameC.Instance.SaveData();
		_fX.UpdateProperties();
	}
	public void SetToggleSoundEffect(bool f)
	{
		_fX.PlayButtonsClip();
		GameC.Instance.GetData().toggleSoundEffect = f;
		soundEffectOn.isOn = f;
		GameC.Instance.SaveData();
		_fX.UpdateProperties();
	}
	public void SetToggleVibration(bool f)
	{
		_fX.PlayButtonsClip();
		GameC.Instance.GetData().toggleVibro = f;
		vibrOn.isOn = f;
		GameC.Instance.SaveData();
		_fX.UpdateProperties();
	}
	#endregion

	public void OpenCloseShop(bool f)
	{
		_fX.PlayButtonsClip();
		shopPanel.SetActive(f);
		
		if (!f)
		{
			HideRememberKnob();
		}
	}

    public void RestartLevel()
	{
		SceneManager.LoadScene(0);
	}
    
    public void Exit()
    {
	    PlayerPrefs.Save();
	    Application.Quit();
    }

    public void LogOut()
    {
	    PlayerPrefs.SetString("LastUserEmail", "");
	    PlayerPrefs.SetString("LastUserPassword", "");
	    PlayerPrefs.SetInt("LoadLoginData", 0);
	    Time.timeScale = 1f;
	    SceneManager.LoadScene("Auth");
    }
    
	#endregion

	public void ShowAllPoints(int points)// показуєм кількість набраних очокна рівні.
	{
		PointsText.text = points.ToString();
	}

	public void SetEarthSliderMaxValue( float maxValue)
	{
		sliderEarthHP.maxValue = maxValue;
	}

	public void SetEarthSliderValue(float value)
	{
		sliderEarthHP.value = value;
	}

	public void ShowRocketCount( int rc)
	{
		RocketCountText.text = rc.ToString();
	}

	public void OpenMenu(bool isOpen)
	{
		_fX.PlayButtonsClip();

		Time.timeScale = isOpen ? 0 : 1;
		
		menuPanel.SetActive(isOpen);
	}

	public void OpenLoss(bool f)
    {
		_fX.PlayButtonsClip();
		lossPanel.SetActive(f);
    }

	#region RememberKnob
	
	public void DeactivateAddPointsAdsButton()
    {
		addPointsAdsButton.interactable = false;
	}

	public void ShowRememberKnob()
	{
		_rememberKnobMenuButton.SetActive(true);
		_rememberKnobShopButton.SetActive(true);
		_rememberKnobAdsButton.SetActive(true);
	}

	public void HideRememberAdsKnob()
	{
		_rememberKnobMenuButton.SetActive(false);
		_rememberKnobAdsButton.SetActive(false);
	}
	
	private void HideRememberKnob()
	{
		_rememberKnobMenuButton.SetActive(false);
		_rememberKnobShopButton.SetActive(false);
	}
	
	#endregion

	#region GPGS

	public void ShowLeaderboard()
	{
		//GPGSManager.Instance.ShowLevelLeaderboard();

		_levelLeaderboard.gameObject.SetActive(true);
		FirebaseManager.Instance.LoadedUsersData();
		StartCoroutine(ShowLeaderboards());
	}

	private IEnumerator ShowLeaderboards()
	{
		var firebaseData = FirebaseManager.Instance;
		yield return new WaitUntil(() => firebaseData.IsUsersDataLoaded);

		var mostFivePlayers = firebaseData.Users.Take(5).ToList();
	
		foreach (var loadedUser in mostFivePlayers)
		{
			_levelLeaderboard.CreateNewPlayerStatsUI(loadedUser.Number,loadedUser.Username, loadedUser.MaxPoints, loadedUser.MaxLevel);
		}
		
		_levelLeaderboard.CreateMyStatsUI(firebaseData.MyData);
	}

	public void SowSelectSkillText()
	{
		_selectSkillText.SetActive(true);
	}
	
	public void HideSelectSkillText()
	{
		_selectSkillText.SetActive(false);
	}

	#endregion
	
	
}
