using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonateText : MonoBehaviour
{
    [SerializeField] private Text _textColor;
    [SerializeField] private int _maxIntensity = 255;
    
    private float r;
    private float g;
    private float b;
    
    private void OnEnable()
    {
        r = _textColor.color.r * 255;
        g = _textColor.color.g * 255;
        b = _textColor.color.b * 255;
    }

    private void FixedUpdate()
    {
        /*if (b < 0)
        {
            b = 0;
        }
        else if (g < 0)
        {
            g = 0;
        }
        else if (r < 0)
        {
            r = 0;
        }
        else if (r == 0 && g == _maxIntensity)
        {
            b--;
        }
        else if (g == _maxIntensity && b == 0)
        {
            r++;
        }
        else if (r == _maxIntensity && b == 0)
        {
            g--;
        }
        else if (g == 0 && r == _maxIntensity)
        {
            b++;
        }
        else if (b == _maxIntensity && g == 0)
        {
            r--;
        }
        else if (r == 0 & b == _maxIntensity)
        {
            g++;
        }*/

        
        //Debug.LogWarning($" Text color: ({r / 255} ___ {g / 255} ___ {b / 255})");
        //_textColor.color = new Color(r / 255, g / 255, b / 255);
    }
}
