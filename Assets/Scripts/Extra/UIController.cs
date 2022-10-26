using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class UIController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] bool inGame;
    [SerializeField] InputActionAsset actionsSet;
    [SerializeField] GameObject eventSytemPrefab;
    [SerializeField] AudioClip navigateSFX;
    InputSystemUIInputModule iSUI;

    [Header("FirstSelected")]
    [SerializeField] Button optionsButton;
    [SerializeField] Slider mainSlider;
    [SerializeField] Button pauseFirstButton;

    private void Start()
    {
        if (inGame)
        {
            iSUI = Instantiate(eventSytemPrefab).GetComponent<InputSystemUIInputModule>();
            //SetNewEventSystemToPlayerInput();
            SuscribePauseEvents();
        }
        SuscribeBackEvents();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void SuscribeBackEvents()
    {
        if (inGame)
        {
            foreach (PlayerConfiguration pC in PlayerConfigurationManager.instance.playerConfigs)
            {
                pC.input.actions.FindActionMap("UI").actions[2].Reset();
                pC.input.actions.FindActionMap("UI").actions[2].performed += (x) =>
                {
                    if (optionsMenu.activeInHierarchy)
                    {
                        CloseSettings();
                    }
                    else if (mainMenu.activeInHierarchy)
                    {
                        ResumeGame();
                    }
                };
                if (pC.input.currentControlScheme == "Gamepad")
                {
                    pC.input.actions.FindActionMap("UI").actions[3].Reset();
                    pC.input.actions.FindActionMap("UI").actions[3].performed += (x) =>
                    {
                        if (mainMenu.activeInHierarchy)
                        {
                            ResumeGame();
                        }
                    };
                }
            }
        }
        else
        {
            actionsSet.FindActionMap("UI").actions[2].Reset();
            actionsSet.FindActionMap("UI").actions[2].performed += (x) =>
            {
                if (optionsMenu.activeInHierarchy)
                {
                    CloseSettings();
                }
            };
        }
    }

    void SuscribePauseEvents()
    {
        foreach (PlayerConfiguration pC in PlayerConfigurationManager.instance.playerConfigs)
        {
            pC.input.actions.FindActionMap("InGamePlayer").actions[5].Reset();
            pC.input.actions.FindActionMap("InGamePlayer").actions[5].performed += (x) =>
            {
                //Cambiamos quién controla el EventSystem en este momento al dispositivo que ha presionado la tecla.
                //SetEventSystemToCurrentInputDevice(pC.input);
                PauseGame();
            };
        }
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        mainSlider.Select();
    }

    public void CloseSettings()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        optionsButton.Select();
    }

    public void PauseGame()
    {
        //Cambiamos el action map de los jugadores a UI;
        foreach (PlayerConfiguration pC in PlayerConfigurationManager.instance.playerConfigs)
        {
            pC.input.SwitchCurrentActionMap("UI");
        }
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        mainMenu.SetActive(true);
        pauseFirstButton.Select();
    }

    //void SetNewEventSystemToPlayerInput()
    //{
    //    foreach (PlayerConfiguration pC in PlayerConfigurationManager.instance.playerConfigs)
    //    {
    //        pC.input.uiInputModule = iSUI;
    //    }
    //}

    //void SetEventSystemToCurrentInputDevice(PlayerInput input)
    //{
    //    iSUI.actionsAsset = input.actions;
    //    Debug.Log(input.actions.FindActionMap("UI").actions[0].ToString());
    //    iSUI.move.Set(input.actions.FindActionMap("UI").actions[1]);
    //    iSUI.submit.Set(input.actions.FindActionMap("UI").actions[0]);
    //    iSUI.cancel.Set(input.actions.FindActionMap("UI").actions[2]);
    //}

    public void ResumeGame()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        //Cambiamos el action map de los jugadores a InGamePlayer;
        foreach (PlayerConfiguration pC in PlayerConfigurationManager.instance.playerConfigs)
        {
            pC.input.SwitchCurrentActionMap("InGamePlayer");
        }
        Time.timeScale = 1f;
    }

    public void SetMasterVolume(float volume)
    {
        AudioController.instance.SetMasterVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioController.instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        AudioController.instance.SetSFXVolume(volume);
    }
}
