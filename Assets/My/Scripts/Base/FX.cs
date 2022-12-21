using UnityEngine;

public class FX : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private AudioSource FoneMusicSource;
    [SerializeField] private AudioSource ButtonsSorce;
    [SerializeField] private AudioSource ShopBySorce;
    [SerializeField] private AudioSource AddPoints;
    [Header("Налаштування FX")]
    [SerializeField] private bool vibroOn;
    [SerializeField] private bool songEffectOn;
    [SerializeField] private bool foneMusicOn;
    [Header("Тривалість вібросигналів в ms")]
    [SerializeField] private int damagePlayerVibroValue = 250;
    [SerializeField] private int damageEnemyVibroValue = 170;
    [SerializeField] private int buttonsUIVibroValue = 50;
    [SerializeField] private int addPointsVibroValue = 40;
#pragma warning disable 0649

    private SettingsData settingsData;
    private static FX instance;
    public static FX Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void PlayAddPoints()
    {
        if (songEffectOn)
            if (!AddPoints.isPlaying)
                AddPoints.Play();
    }

    public void UpdateProperties()
    {
        settingsData = GameC.Instance.GetData();

        foneMusicOn = settingsData.toggleFoneMusic ? true : false;
        songEffectOn = settingsData.toggleSoundEffect ? true : false;
        vibroOn = settingsData.toggleVibro ? true : false;      
        SetOnPlayFoneMusic(); 
    }

    public void SetOnPlayFoneMusic()
    {
        if (foneMusicOn && !FoneMusicSource.isPlaying)
            FoneMusicSource.Play();

        if (!foneMusicOn)
            FoneMusicSource.Stop();
    }

    public void PlayButtonsClip()
    {
        if (songEffectOn)
            ButtonsSorce.Play();
    }
    public void PlayButtonsShopBy()
    {
        if (songEffectOn)
            ShopBySorce.Play();
    }

    #region VibroFun

    /// <summary>
    /// Планеті нанесли урон
    /// </summary>
    public void DamagePlayerVibro()
    {
        if (vibroOn)
            Vibration.Vibrate((long)damagePlayerVibroValue);
    }

    /// <summary>
    /// Ракета попала в метеорит.
    /// </summary>
    public void DamageEnemyVibro()
    {
        if (vibroOn)
            Vibration.Vibrate((long)damageEnemyVibroValue);
    }

    /// <summary>
    /// Для кнопок
    /// </summary>
    public void ButtonsUIVibro()
    {
        if (vibroOn)
            Vibration.Vibrate((long)buttonsUIVibroValue);
    }

    /// <summary>
    /// Заробили монетку
    /// </summary>
    public void AddPointsVibro()
    {
        if (vibroOn)
            Vibration.Vibrate((long)addPointsVibroValue);
    }
    #endregion
}

public static class Vibration
{

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static void Vibrate()
    {
        if (isAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }


    public static void Vibrate(long milliseconds)
    {
#if !UNITY_EDITOR


        try
        {
            if (isAndroid())
                vibrator.Call("vibrate", milliseconds);
            else
                Handheld.Vibrate();
        }
        catch
        {
        }
#endif
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (isAndroid())
            vibrator.Call("vibrate", pattern, repeat);
        else
            Handheld.Vibrate();
    }

    public static bool HasVibrator()
    {
        return isAndroid();
    }

    public static void Cancel()
    {
        if (isAndroid())
            vibrator.Call("cancel");
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	return true;
#else
        return false;
#endif
    }
}



