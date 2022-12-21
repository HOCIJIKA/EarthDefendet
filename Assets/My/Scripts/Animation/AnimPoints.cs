using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimPoints : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private int PointsToShow;
    [SerializeField] private Text countText;    
    [Header("Множення оків")]
    [SerializeField] private int pointsMult;
    [SerializeField] private Text multText;
#pragma warning restore 0649

    public void SetPointToShow(int points, int mult)
    {
        PointsToShow = points;
        countText.text = points.ToString();
        pointsMult = mult;

        if (points < 10)
        {
            // задаєм колір в залежності від очок.
        }

        if (pointsMult > 1)
        {
            ShowMult();
        }

    }

    private void ShowMult()
    {
        multText.gameObject.SetActive(true);
        multText.text = string.Format("X {0}", pointsMult);
    }
}
