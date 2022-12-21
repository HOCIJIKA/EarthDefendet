using UnityEngine;
using System.Collections.Generic;
using Firebase;

public class GameC : MonoBehaviour
{
	public static GameC Instance => _instance;

	private const int NumberOfContainers = 4;
	[SerializeField] private MenuUI menuUI;

	private SaveLoadSystem saveLoadSystem;
	public SaveLoadSystem SaveLoadSystem => saveLoadSystem;

	public MenuUI MenuUI => menuUI;

	[SerializeField] private PointsCollector pointsCollector;
	public PointsCollector PointsCollector => pointsCollector;

	[SerializeField] private Shooter shooter;
	public Shooter Shooter => shooter;

	[SerializeField] private Earth earth;
	public Earth Earth => earth;

	private static GameC _instance;
	private FirebaseApp _firebaseApp;

	private void Awake()
	{
		if (_instance == null)
			_instance = this;

		saveLoadSystem = new SaveLoadSystem();

		Application.targetFrameRate = 144;
	}

	private void Start()
	{
		FirsStart();
		menuUI.Init();
		pointsCollector.Init(this);
		shooter.Init();
		earth.Init(menuUI);
		SkillsUpgrade.Instance.Init();
		SkillsContainer.Instance.Init();
		LvlController.Instance.Init();

		ChackLocalization.Instance.SetLoc();
		//GameAnalytics.Initialize();
		Advertising.Instance.Init();
		FX.Instance.UpdateProperties();
		InitialFireBase();
	}

	private void FirsStart()
	{
		// чи вперше гра запущена.
		if (GetData().NotFirsStart) return;

		// створюєм ліст контейнерів.
		var isActiveContainers = new List<bool>();// SaveLoadSystem.Data.Settings.IsActiveContainers;
		for (int i = 0; i < NumberOfContainers; i++) 
			isActiveContainers.Add(false);

		GetData().IsActiveContainers = isActiveContainers;

		// створили список контейнерів і заповнили його 
		var containerNumberSkill = new List<int>();
		for (int i = 0; i < ListButtonsSkills.Instance.GetCountButtonsSkill(); i++)
			containerNumberSkill.Add(-1);
		GetData().ContainerNumberSkill = containerNumberSkill;

		// Створили список апу скілів. Всі скіли на "0" рівні
		var LvlsSkills = new List<int>();
		for (int i = 0; i < ListButtonsSkills.Instance.GetCountButtonsSkill(); i++)
			LvlsSkills.Add(0);
		GetData().LvlsSkills = LvlsSkills;

		// параметри FX
		GetData().toggleFoneMusic = true;
		GetData().toggleSoundEffect = true;
		GetData().toggleVibro = true;
		GetData().Autoplay = true;
		GetData().OnSatellite = true;

		//парамерти LvlController
		GetData().CurrentLvl = 1;

		GetData().NotFirsStart = true;
		SaveData();
	}

	public void SaveData()
    {
		SaveLoadSystem.Save();
	}

	public SettingsData GetData()
    {
		return SaveLoadSystem.Data.Settings;
	}

	private void InitialFireBase()
	{
		Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
			var dependencyStatus = task.Result;
			if (dependencyStatus == Firebase.DependencyStatus.Available) {
				// Create and hold a reference to your FirebaseApp,
				// where app is a Firebase.FirebaseApp property of your application class.
				_firebaseApp = Firebase.FirebaseApp.DefaultInstance;

				// Set a flag here to indicate whether Firebase is ready to use by your app.
			} else {
				UnityEngine.Debug.LogError(System.String.Format(
					"Could not resolve all Firebase dependencies: {0}", dependencyStatus));
				// Firebase Unity SDK is not safe to use here.
			}
		});
	}
}