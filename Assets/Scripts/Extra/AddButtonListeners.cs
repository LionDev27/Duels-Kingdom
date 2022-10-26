using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButtonListeners : MonoBehaviour
{
    [SerializeField] Button[] buttons;

    private void Start()
    {
        AddListenerToButtons();
    }

    void AddListenerToButtons()
    {
        foreach (Button b in buttons)
        {
            b.onClick.AddListener(OnNavigatePlaySFX);
        }
    }

    void OnNavigatePlaySFX()
    {
        AudioController.instance.PlaySFX(AudioController.instance.menuNav);
    }
}
