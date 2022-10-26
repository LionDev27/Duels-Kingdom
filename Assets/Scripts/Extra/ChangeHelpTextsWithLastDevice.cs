using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class ChangeHelpTextsWithLastDevice : MonoBehaviour
{
    [SerializeField] GameObject gamepadTexts, keyboardTexts;
    [SerializeField] bool mainMenu, initializingPlayers;
    [SerializeField] InputActionAsset c;
    [SerializeField] KeyCode[] keyboardInputs, gamepadInputs;
    bool isKeyboard;

    private void Start()
    {
        if (!initializingPlayers)
        {
            SuscribeEvents();
        }
    }

    private void Update()
    {
        if (mainMenu)
        {
            CheckInputs();
        }
    }

    void SuscribeEvents()
    {
        if (!mainMenu)
        {
            //Obtenemos el playerconfiguration del primer jugador.
            PlayerConfiguration pC = PlayerConfigurationManager.instance.playerConfigs[0];
            ChangeHelpTexts(pC.input.currentControlScheme);
        }
        else
        {
            InputActionMap uI = c.FindActionMap("UI");
            for (int i = 0; i < uI.actions.Count; i++)
            {
                uI.actions[i].performed += (x) =>
                {
                    if (isKeyboard)
                    {
                        ChangeHelpTexts("Keyboard");
                    }
                    else
                    {
                        ChangeHelpTexts("Gamepad");
                    }
                };
            }
        }
    }

    void CheckInputs()
    {
        if (!isKeyboard)
        {
            foreach (KeyCode k in keyboardInputs)
            {
                if (Input.GetKeyDown(k))
                {
                    isKeyboard = true;
                    break;
                }
            }
        }
        else
        {
            foreach (KeyCode k in gamepadInputs)
            {
                if (Input.GetKeyDown(k) || Gamepad.current.dpad.IsPressed() || Gamepad.current.leftStick.IsActuated())
                {
                    isKeyboard = false;
                    break;
                }
            }
        }
    }

    public void ChangeHelpTexts(string device)
    {
        if (device == "Gamepad")
        {
            keyboardTexts.SetActive(false);
            gamepadTexts.SetActive(true);
        }
        else if (device == "Keyboard")
        {
            gamepadTexts.SetActive(false);
            keyboardTexts.SetActive(true);
        }
    }
}
