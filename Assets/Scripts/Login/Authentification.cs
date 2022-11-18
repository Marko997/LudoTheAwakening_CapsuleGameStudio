using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//
using System;
using System.Globalization;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;


public class Authentification : MonoBehaviour
{
    //public static Scene1 scene1;
    ////public Text UserNamen;
    //public Text playerName;
    //[SerializeField] TextMeshProUGUI TopText;
    [SerializeField] TextMeshProUGUI MessageText;

    [Header("Login")]
    [SerializeField]
    TMP_InputField EmailLoginInput;
    [SerializeField] TMP_InputField PasswordLoginInput;
    [SerializeField] GameObject LoginPage;

    [Header("Register")]
    [SerializeField]
    TMP_InputField UserNameRegisterInput;
    [SerializeField] TMP_InputField EmailRegisterInput;
    [SerializeField] TMP_InputField PasswordRegisterInput;
    [SerializeField] GameObject RegisterPage;

    [Header("Recovery")]
    [SerializeField]
    TMP_InputField EmailRecoverryInput;
    [SerializeField] GameObject RecoveryPage;

    [Header("WelcomeUserName")]
    [SerializeField]
    public GameObject WelcomeUSerName;
    [SerializeField]
    public Text WelcomeUserNameText;
    //
    private string userEmail;
    private string userPassword;
    //private string userName;
    public GameObject loginPanel;
    public GameObject addLoginPanel;
    public GameObject recoverButton;

    #region Button Functions
    public void Start()
    {

        //PlayerPrefs.DeleteAll();
        
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
            PlayerPrefs.SetString("EMAIL", userEmail);
            PlayerPrefs.SetString("PASSWORD", userPassword);
        }

        WelcomeUserNameText.text = "Welcome " + name;
      
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        WelcomeUSerName.SetActive(true);
        MessageText.text = "Login in";
        yield return new WaitForSeconds(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

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
        OpenLoginPage();
    }

    private void OnError(PlayFabError Error)
    {
        MessageText.text = Error.ErrorMessage;
        Debug.Log(Error.GenerateErrorReport());
    }

    private void OnregisterSucces(RegisterPlayFabUserResult Result)
    {
        MessageText.text = "New Acoocunt is Created";
        OpenLoginPage();
    }

    public void OpenLoginPage()
    {

        LoginPage.SetActive(true);
        RegisterPage.SetActive(false);
        RecoveryPage.SetActive(false);

    }

    public void OpenRegisterPage()
    {

        LoginPage.SetActive(false);
        RegisterPage.SetActive(true);
        RecoveryPage.SetActive(false);

    }

    public void OpenRecoveryPage()
    {

        LoginPage.SetActive(false);
        RegisterPage.SetActive(false);
        RecoveryPage.SetActive(true);

    }

    #endregion

    //Login As Guest
    public static Authentification Instance;

    public static UnityEvent OnSignInSuccess = new UnityEvent();
    public static UnityEvent<string> OnSignInFailed = new UnityEvent<string>();
    public static UnityEvent<string> OnCreateAccountFailed = new UnityEvent<string>();
    public static UnityEvent<string, string> OnUserDataRetrieved = new UnityEvent<string, string>();
    public static UnityEvent<string, List<PlayerLeaderboardEntry>> OnLeaderboardRetrieved = new UnityEvent<string, List<PlayerLeaderboardEntry>>();
    public static UnityEvent<string, StatisticValue> OnStatisticRetrieved = new UnityEvent<string, StatisticValue>();

    public static string playfabID;
    public static UserAccountInfo userAccountInfo;

    void Awake()
    {
        Instance = this;
    }

    /*
        ACCOUNT
    */

    public void SignInWithDevice()
    {
        if (GetDeviceId(out string android_id, out string ios_id, out string custom_id))
        {
            if (!string.IsNullOrEmpty(android_id))
            {
                StartCoroutine(LoadNextScene());
                Debug.Log("Using Android Device ID: " + android_id);

                PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
                {
                    AndroidDeviceId = android_id,
                    TitleId = PlayFabSettings.TitleId,
                    CreateAccount = true
                }, result => {
                    Debug.Log($"Successful Login with Android Device ID");
                    playfabID = result.PlayFabId;
                    //CheckDisplayName("User", () => OnSignInSuccess.Invoke());
                }, error => {
                    Debug.Log($"<color=red>Unsuccessful Login with Android Device ID</color>");
                    OnSignInFailed.Invoke(error.ErrorMessage);
                });
            }
            else if (!string.IsNullOrEmpty(ios_id))
            {
                StartCoroutine(LoadNextScene());
                Debug.Log("Using IOS Device ID: " + ios_id);

                PlayFabClientAPI.LoginWithIOSDeviceID(new LoginWithIOSDeviceIDRequest()
                {
                    DeviceId = ios_id,
                    TitleId = PlayFabSettings.TitleId,
                    CreateAccount = true
                }, result => {
                    Debug.Log($"Successful Login with IOS Device ID");
                    playfabID = result.PlayFabId;
                    //CheckDisplayName("User", () => OnSignInSuccess.Invoke());
                }, error => {
                    Debug.Log($"<color=red>Unsuccessful Login with IOS Device ID</color>");
                    OnSignInFailed.Invoke(error.ErrorMessage);
                });
            }
        }
        else
        {
            StartCoroutine(LoadNextScene());
            Debug.Log("Using custom device ID: " + custom_id);

            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
            {
                CustomId = custom_id,
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true
            }, result => {
                Debug.Log($"Successful Login with custom device ID");
                playfabID = result.PlayFabId;
                //CheckDisplayName("User", () => OnSignInSuccess.Invoke());
            }, error => {
                Debug.Log($"<color=red>Unsuccessful Login with custom device ID</color>");
                OnSignInFailed.Invoke(error.ErrorMessage);
            });
        }
    }

    bool GetDeviceId(out string android_id, out string ios_id, out string custom_id)
    {
        android_id = string.Empty;
        ios_id = string.Empty;
        custom_id = string.Empty;

        if (CheckForSupportedMobilePlatform())
        {
#if UNITY_ANDROID
            //http://answers.unity3d.com/questions/430630/how-can-i-get-android-id-.html
            AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
            android_id = clsSecure.CallStatic<string>("getString", objResolver, "android_id");
#endif

#if UNITY_IPHONE
            ios_id = UnityEngine.iOS.Device.vendorIdentifier;
#endif
            return true;
        }
        else
        {
            custom_id = SystemInfo.deviceUniqueIdentifier;
            return false;
        }
    }

    bool CheckForSupportedMobilePlatform()
    {
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

}