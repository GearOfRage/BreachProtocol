using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : MonoBehaviour
{
    public static ColorPalette _instance;

    [SerializeField] public Color purpleDark;
    [SerializeField] public Color white;
    [SerializeField] public Color yellowLight;
    [SerializeField] public Color blue;
    [SerializeField] public Color greenLight;
    [SerializeField] public Color greenDark;
    [SerializeField] public Color redLight;
    [SerializeField] public Color redDark;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
}