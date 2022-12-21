using Assets.SimpleLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChackLocalization : MonoBehaviour
{
    private static ChackLocalization instance;
    public static ChackLocalization Instance => instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLoc()
    {
        LocalizationManager.Read();

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Ukrainian:
                LocalizationManager.Language = "Ukrainian";
                break;
            case SystemLanguage.Russian:
                LocalizationManager.Language = "Russian";
                break;
            default:
                LocalizationManager.Language = "English";
                break;
        }
    }
    public void SetLocalization(string localization)
    {
        LocalizationManager.Language = localization;
    }
}
