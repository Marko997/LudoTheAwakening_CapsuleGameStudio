using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ScreenSwitcher : MonoBehaviour
{
    public ScreenType desiredScreenType;
    public bool isPawnMoving = false;

    [SerializeField]ScreenManager screenManager;
    public Button menuButton;

    public string desiredPanelName;

    private void Awake()
    {
        screenManager = GetComponentInParent<ScreenManager>();
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(OnButtonClicked);
        menuButton.AddButtonSounds();
        //screenManager = ScreenManager.Instance;
    }

    public void OnButtonClicked()
    {

        if (!isPawnMoving)
        {
            screenManager.SwitchScreen(desiredScreenType);
        }
    }
    public void OpenFriendsPanel()
    {
        if (!isPawnMoving)
        {
            screenManager.SwitchScreen(ScreenType.Friends);
        }
    }

    public void OpenChangeIconPanel()
    {
        if (!isPawnMoving)
        {
            screenManager.SwitchScreen(ScreenType.ProfileIcon);
        }
    }

}
