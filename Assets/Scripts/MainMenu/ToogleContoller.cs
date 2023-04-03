using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToogleContoller : MonoBehaviour
{
    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;

    private string toggle1Key = "Toggle1State";
    private string toggle2Key = "Toggle2State";
    private string toggle3Key = "Toggle3State";

    void Start()
    {
        // Učitavanje sačuvanih vrednosti kada se igra pokrene
        toggle1.isOn = PlayerPrefs.GetInt(toggle1Key, 0) == 1;
        toggle2.isOn = PlayerPrefs.GetInt(toggle2Key, 0) == 1;
        toggle3.isOn = PlayerPrefs.GetInt(toggle3Key, 0) == 1;
    }

    public void SaveToggle1State(bool state)
    {
        // Čuvanje stanja kvačice (toggle) kada se klikne na nju
        PlayerPrefs.SetInt(toggle1Key, state ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveToggle2State(bool state)
    {
        // Čuvanje stanja kvačice (toggle) kada se klikne na nju
        PlayerPrefs.SetInt(toggle2Key, state ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveToggle3State(bool state)
    {
        // Čuvanje stanja kvačice (toggle) kada se klikne na nju
        PlayerPrefs.SetInt(toggle3Key, state ? 1 : 0);
        PlayerPrefs.Save();
    }
}
