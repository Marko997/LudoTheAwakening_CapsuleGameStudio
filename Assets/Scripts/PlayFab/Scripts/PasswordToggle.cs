using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordToggle : MonoBehaviour
{
    //public InputField passwordInputField;
    //public Toggle toggle;

    //private void Start()
    //{
    //    toggle.onValueChanged.AddListener(ToggleValueChanged);
    //}

    //private void ToggleValueChanged(bool value)
    //{
    //    if (value)
    //    {
    //        passwordInputField.contentType = InputField.ContentType.Password;
    //    }
    //    else
    //    {
    //        passwordInputField.contentType = InputField.ContentType.Standard;
    //    }
    //}

    //public InputField passwordField;
    //private bool passwordHidden = true;

    //public void TogglePasswordVisibility()
    //{
    //    if (passwordHidden)
    //    {
    //        passwordField.contentType = InputField.ContentType.Standard;
    //        passwordField.text = passwordField.text;
    //    }
    //    else
    //    {
    //        passwordField.contentType = InputField.ContentType.Password;
    //        passwordField.text = passwordField.text;
    //    }

    //    passwordHidden = !passwordHidden;
    //}

    //////////////////////public InputField passwordInput;
    //////////////////////public Toggle toggle;
    //////////////////////private bool isPasswordVisible = false;

    //////////////////////void Update()
    //////////////////////{
    //////////////////////    if (toggle.isOn != isPasswordVisible)
    //////////////////////    {
    //////////////////////        isPasswordVisible = toggle.isOn;
    //////////////////////        passwordInput.contentType = isPasswordVisible ? InputField.ContentType.Standard : InputField.ContentType.Password;
    //////////////////////    }
    //////////////////////}



    public TMPro.TMP_InputField passwordInput = null;

    public void ToggleInputType()
    {
        if (this.passwordInput != null)
        {
            if (this.passwordInput.contentType == TMP_InputField.ContentType.Password)
            {
                this.passwordInput.contentType = TMP_InputField.ContentType.Standard;
            }
            else
            {
                this.passwordInput.contentType = TMP_InputField.ContentType.Password;
            }

            this.passwordInput.ForceLabelUpdate();
        }
    }

    //public InputField passwordField;

    //public void MyMethodThatIsntNamedUpdate(bool toggleValue)
    //{
    //    // If this is strange do it in the normal if/else way
    //    passwordField.contentType = toggleValue ?
    //        InputField.ContentType.Standard :
    //        InputField.ContentType.Password;

    //    passwordField.ForceLabelUpdate();
    //}

}
