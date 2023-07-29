using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
public class ChangeDisplayNameName : MonoBehaviour
{
    public InputField UserNameRegisterInput;
    public TMP_Text displayNameError;
    //public GameObject ChangeDisplayNameCanvas;
    private const string lastLogoutTimeKey = "LastLogoutTime";
    private const float changeNameInterval = 0; // 24 sata u sekundama//24 * 60 * 60

    private string errorMessage;


    public void SaveDisplayName()
    {
        // proverite da li je dovoljno pro�lo vreme od poslednjeg izlogovanja
        if (CanChangeName())
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = UserNameRegisterInput.text,
            };
            //PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayUpdate, OnErrorr);
            PlayerPrefs.SetString("NAME", UserNameRegisterInput.text);
            Debug.Log(PlayerPrefs.GetString("NAME"));
            // sa�uvajte trenutno vreme kao vreme poslednje promene imena i izlogovanja
            var now = DateTime.Now;
            PlayerPrefs.SetString(lastLogoutTimeKey, now.ToString());
        }
        else
        {
            displayNameError.text = "Must to wait 3 hours to change name.";
        }
    }

    private bool CanChangeName()
    {
        // dobijte vreme poslednjeg izlogovanja iz PlayerPrefs
        string lastLogoutTimeStr = PlayerPrefs.GetString(lastLogoutTimeKey, "");
        if (string.IsNullOrEmpty(lastLogoutTimeStr))
        {
            return true; // korisnik se nikada nije izlogovao, pa mu dozvolite da promeni ime
        }

        // konvertujte poslednje vreme izlogovanja u DateTime format
        DateTime lastLogoutTime = DateTime.Parse(lastLogoutTimeStr);

        // proverite da li je pro�lo dovoljno vreme od poslednjeg izlogovanja
        return (DateTime.Now - lastLogoutTime).TotalSeconds >= changeNameInterval;
    }

    private void OnErrorr(PlayFabError obj)
    {
        Debug.Log("Something is wrong");

        errorMessage = obj.ErrorMessage;
    }

    private void OnDisplayUpdate(UpdateUserTitleDisplayNameResult obj)
    {
        Debug.Log("Success change name");
        UserNameRegisterInput.text = obj.DisplayName; // a�uriranje teksta u polju
                                                      //ChangeDisplayNameCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!CanChangeName())
        {
            displayNameError.text = "Morate da sacekate 24 sata da biste ponovo mogli da promenite ime.";
        }
        else if (!string.IsNullOrEmpty(errorMessage))
        {
            displayNameError.text = errorMessage;
            errorMessage = null;
        }
    }
}
