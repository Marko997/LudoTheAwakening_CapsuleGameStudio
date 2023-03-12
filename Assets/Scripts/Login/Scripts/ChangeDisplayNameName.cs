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
    public GameObject ChangeDisplayNameCanvas;
    private const string lastChangeTimeKey = "LastChangeTime";
    private const float changeNameInterval = 24 * 60 * 60; // 3 sata u sekundama

    public void SaveDisplayName()
    {
        // proverite da li je dovoljno prošlo vreme od poslednje promene imena
        if (CanChangeName())
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = UserNameRegisterInput.text,
            };
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayUpdate, OnErrorr);

            // saèuvajte trenutno vreme kao vreme poslednje promene imena
            PlayerPrefs.SetString(lastChangeTimeKey, DateTime.Now.ToString());
        }
        else
        {
            displayNameError.text = "Morate da saèekate 24 sata da bi ste ponovo mogli da promenite ime.";
        }
    }

    private bool CanChangeName()
    {
        // dobijte vreme poslednje promene imena iz PlayerPrefs
        string lastChangeTimeStr = PlayerPrefs.GetString(lastChangeTimeKey, "");
        if (string.IsNullOrEmpty(lastChangeTimeStr))
        {
            return true; // korisnik nikada ranije nije promenio ime, pa mu dozvolite da promeni ime
        }

        // konvertujte poslednje vreme promene imena u DateTime format
        DateTime lastChangeTime = DateTime.Parse(lastChangeTimeStr);

        // proverite da li je prošlo dovoljno vreme od poslednje promene imena
        return (DateTime.Now - lastChangeTime).TotalSeconds >= changeNameInterval;
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
        ChangeDisplayNameCanvas.SetActive(false);
    }

}
