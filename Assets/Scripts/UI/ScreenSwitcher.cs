using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ScreenSwitcher : MonoBehaviour
{
    public ScreenType desiredScreenType;
    public bool isPawnMoving = false;

    [SerializeField]ScreenManager screenManager;
    Button menuButton;

    private void Start()
    {
        screenManager = GetComponentInParent<ScreenManager>();
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(OnButtonClicked);
        //screenManager = ScreenManager.Instance;
    }

    void OnButtonClicked()
    {
        if (!isPawnMoving)
        {
            screenManager.SwitchScreen(desiredScreenType);
        }
    }
}
