using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using System.Linq;
using PlayFab.AuthenticationModels;

public class ChangeDisplayNameName : MonoBehaviour
{

    ////// public InputField UserNameRegisterInput;
    ////// public Text displayNameError;
    //////// public GameObject ChangeDisplayNameCanvas;

    ////// public void SaveDisplayName()
    ////// {

    //////     //playerUserName.text = UserNameRegisterInput.text;
    //////     var request = new UpdateUserTitleDisplayNameRequest
    //////     {
    //////         DisplayName = UserNameRegisterInput.text,
    //////     };
    //////     PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayUpdate, OnErrorr);
    ////// }

    ////// private void OnErrorr(PlayFabError obj)
    ////// {
    //////     Debug.Log("Something is wrong");

    //////     displayNameError.text = obj.ErrorMessage;
    ////// }

    ////// private void OnDisplayUpdate(UpdateUserTitleDisplayNameResult obj)
    ////// {
    //////     Debug.Log("Succes change name");
    //////     UserNameRegisterInput.text = obj.DisplayName; // ažuriranje teksta u polju
    //////     //ChangeDisplayNameCanvas.SetActive(false);
    ////// }

    //////private void OnDisplayUpdate(UpdateUserTitleDisplayNameResult obj)
    //////{
    //////    Debug.Log("Succes change name");
    //////    //ChangeDisplayNameCanvas.SetActive(false);
    //////}


    ////////////////////////public InputField UserNameRegisterInput;
    ////////////////////////public Text displayNameError;
    //////////////////////////public GameObject ChangeDisplayNameCanvas;

    ////////////////////////public void SaveDisplayName()
    ////////////////////////{
    ////////////////////////    var request = new UpdateUserTitleDisplayNameRequest
    ////////////////////////    {
    ////////////////////////        DisplayName = UserNameRegisterInput.text,
    ////////////////////////    };
    ////////////////////////    PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayUpdate, OnErrorr);
    ////////////////////////}

    ////////////////////////private void OnErrorr(PlayFabError obj)
    ////////////////////////{
    ////////////////////////    Debug.Log("Something is wrong");

    ////////////////////////    displayNameError.text = obj.ErrorMessage;
    ////////////////////////}

    ////////////////////////private void OnDisplayUpdate(UpdateUserTitleDisplayNameResult obj)
    ////////////////////////{
    ////////////////////////    Debug.Log("Success change name");
    ////////////////////////    UserNameRegisterInput.text = obj.DisplayName; // ažuriranje teksta u polju
    ////////////////////////    //ChangeDisplayNameCanvas.SetActive(false);
    ////////////////////////}


    public InputField UserNameRegisterInput;
    public Text displayNameError;
    //public GameObject ChangeDisplayNameCanvas;
    private const string lastLogoutTimeKey = "LastLogoutTime";
    private const float changeNameInterval = 24 * 60 * 60; // 24 sata u sekundama

    public void SaveDisplayName()
    {
        // proverite da li je dovoljno prošlo vreme od poslednjeg izlogovanja
        if (CanChangeName())
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = UserNameRegisterInput.text,
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayUpdate, OnErrorr);

            // saèuvajte trenutno vreme kao vreme poslednje promene imena i izlogovanja
            var now = DateTime.Now;
            PlayerPrefs.SetString(lastLogoutTimeKey, now.ToString());
            //PlayerPrefs.SetString(lastChangeTimeKey, now.ToString());
        }
        else
        {
            displayNameError.text = "Morate da saèekate 24 sata da biste ponovo mogli da promenite ime.";
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

        // proverite da li je prošlo dovoljno vreme od poslednjeg izlogovanja
        return (DateTime.Now - lastLogoutTime).TotalSeconds >= changeNameInterval;
    }

    private void OnErrorr(PlayFabError obj)
    {
        Debug.Log("Something is wrong");

        displayNameError.text = obj.ErrorMessage;
    }

    private void OnDisplayUpdate(UpdateUserTitleDisplayNameResult obj)
    {
        Debug.Log("Success change name");
        UserNameRegisterInput.text = obj.DisplayName; // ažuriranje teksta u polju
                                                      //ChangeDisplayNameCanvas.SetActive(false);
    }

}
