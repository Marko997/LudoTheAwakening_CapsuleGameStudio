using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    Button heroInfoButton;
    // Start is called before the first frame update
    void Start()
    {
        heroInfoButton = GetComponent<Button>();
        heroInfoButton.onClick.AddListener(() => CharacterShowcase.Instance.OnInfoButtonClicked(transform.parent.name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
