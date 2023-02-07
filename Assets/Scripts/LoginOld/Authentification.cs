using System.Collections;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Authentification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI MessageText;

    [Header("Login")]
    [SerializeField] TMP_InputField EmailLoginInput;
    [SerializeField] TMP_InputField PasswordLoginInput;
    [SerializeField] GameObject LoginPage;

    [Header("Register")]
    [SerializeField] TMP_InputField UserNameRegisterInput;
    [SerializeField] TMP_InputField EmailRegisterInput;
    [SerializeField] TMP_InputField PasswordRegisterInput;
    [SerializeField] GameObject RegisterPage;

    [Header("Recovery")]
    [SerializeField] TMP_InputField EmailRecoverryInput;
    [SerializeField] GameObject RecoveryPage;

    [Header("WelcomeUserName")]
    [SerializeField] public GameObject WelcomeUSerName;
    [SerializeField] public Text WelcomeUserNameText;
  
    private string userEmail;
    private string userPassword;

    public GameObject loginPanel;
    public GameObject addLoginPanel;
    public GameObject recoverButton;

    public EntityKey token;

    #region Button Functions
    public void Start()
    {
        //PlayerPrefs.DeleteAll();//Za odjavu zaboravi mail i sifru//Funkcionise po principu odkomentarises ,porenes,zakomentarrises i ulogujes se jer pamti logovanje.Kad bi stojalo odkomenatisano ne bi se auto logovao
        if (PlayerPrefs.HasKey("EMAIL"))
        {
            userEmail = PlayerPrefs.GetString("EMAIL");
            userPassword = PlayerPrefs.GetString("PASSWORD");
            var request = new LoginWithEmailAddressRequest
            {
                Email = userEmail,
                Password = userPassword,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
            };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSucess, OnError);
        }   
    }

    public void GetUserEmail(string emailIn)
    {
        userEmail = emailIn;
    }

    public void GetUserPassword(string passwordIn)
    {
        userPassword = passwordIn;
    }

    public void RegisterUser()
    {
        var request = new RegisterPlayFabUserRequest
        {       
            DisplayName = UserNameRegisterInput.text,
            Email = EmailRegisterInput.text,
            Password = PasswordRegisterInput.text,

            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnregisterSucces, OnError);
    }

    public void Login()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = EmailLoginInput.text,
            Password = PasswordLoginInput.text,
            
        InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSucess, OnError);
    }

    private void OnLoginSucess(LoginResult result)
    {
        string name = null;

        if (result.InfoResultPayload != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
            token = result.EntityToken.Entity;
            PlayerPrefs.SetString("EMAIL", userEmail);
            PlayerPrefs.SetString("PASSWORD", userPassword);
            //OVO NIJE BEZBEDNO!!!!
        }
        Debug.Log(token.Type);



        WelcomeUserNameText.text = "Welcome " + name;
        //SubimittName();
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        WelcomeUSerName.SetActive(true);
        MessageText.text = "Login in";
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene("MainScene"); 
    }

    public void RecoverUser()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = EmailRecoverryInput.text,
            TitleId = "2D715",
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnReciverySucces, OnError);
    }

    private void OnReciverySucces(SendAccountRecoveryEmailResult obj)
    {
        MessageText.text = "Recovery Email send.Go to your email";
        OpenPages(true, false, false);
    }

    private void OnError(PlayFabError Error)
    {
        MessageText.text = Error.ErrorMessage;
        Debug.Log(Error.GenerateErrorReport());
    }

    private void OnregisterSucces(RegisterPlayFabUserResult Result)
    {
        MessageText.text = "New Acoocunt is Created";
        OpenPages(true, false, false);
    }

    public void OpenLoginPage()
    {
        OpenPages(true, false, false);
    }

    public void OpenRegisterPage()
    {
        OpenPages(false, true, false);
    }

    public void OpenRecoveryPage()
    {
        OpenPages(false, false, true);
    }
    public void OpenPages(bool LoginP, bool RegisterP, bool RecoveryP)
    {
        LoginPage.SetActive(LoginP);
        RegisterPage.SetActive(RegisterP);
        RecoveryPage.SetActive(RecoveryP);
    }

    #endregion

    //Login As Guest
    #region Guest
    public void LoginAsGuest()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSucess, OnError);
        //GetToken();
    }


    //public void SubimittName()
    //{
    //    var request = new UpdateUserTitleDisplayNameRequest
    //    {
    //        DisplayName = "Guest"
    //    };
    //    PlayFabClientAPI.UpdateUserTitleDisplayName(request, OndisplayNameUpdate, OnError);
    //}

    //void OndisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    //{
    //    Debug.Log("OK");
    //}
    #endregion
}