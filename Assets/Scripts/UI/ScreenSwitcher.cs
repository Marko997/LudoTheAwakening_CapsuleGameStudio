using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ScreenSwitcher : MonoBehaviour
{
    public ScreenType desiredScreenType;

    ScreenManager screenManager;
    Button menuButton;

    private void Start()
    {
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(OnButtonClicked);
        screenManager = ScreenManager.Instance;
    }

    void OnButtonClicked()
    {
        screenManager.SwitchScreen(desiredScreenType);
    }
}
