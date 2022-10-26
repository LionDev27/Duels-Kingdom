using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMenuController : MonoBehaviour
{
    //Texto que mostrará que jugador es cada uno.
    [SerializeField] Text titleText;
    //Colores de cada jugador.
    [SerializeField] Color[] playerColors;
    //
    [SerializeField] GameObject readyButton;
    [SerializeField] GameObject ticImage;
    [SerializeField] Image fillImage;
    //Input del jugador
    PlayerInput input;

    private void Start()
    {
        SetPlayerIndex();
        SuscribeBackEvents();
    }

    void SetPlayerIndex()
    {
        //Actualizamos el texto.
        titleText.text = $"PLAYER {input.playerIndex + 1}";
        //Asignamos el color dependiendo de qué jugador seamos.
        SetColor();
    }

    public void SetColor()
    {
        for (int i = 0; i < PlayerConfigurationManager.instance.GetComponent<PlayerInputManager>().maxPlayerCount; i++)
        {
            if (input.playerIndex == i)
            {
                PlayerConfigurationManager.instance.SetPlayerColor(input.playerIndex, playerColors[i]);
                fillImage.color = playerColors[i];
            }
        }
    }

    public void SetPlayerInput(PlayerInput pI)
    {
        input = pI;
    }

    public void ReadyPlayer()
    {
        PlayerConfigurationManager.instance.SetPlayerReady(input.playerIndex);
        readyButton.SetActive(false);
        ticImage.SetActive(true);
    }

    public void OnClickPlaySFX()
    {
        AudioController.instance.PlaySFX(AudioController.instance.menuNav);
    }

    void SuscribeBackEvents()
    {
        input.actions.FindActionMap("UI").actions[2].Reset();
        input.actions.FindActionMap("UI").actions[2].performed += (x) =>
        {
            if (readyButton.activeInHierarchy)
            {
                GoToPreviousScene();
            }
            else
            {
                EnableReadyButton();
            }
        };
    }

    void GoToPreviousScene()
    {
        //Destruimos el PlayerConfigurationManager.
        GameObject pCM = FindObjectOfType<PlayerConfigurationManager>().gameObject;
        if (pCM != null)
        {
            Destroy(pCM);
        }
        GameObject aC = FindObjectOfType<AudioController>().gameObject;
        if (pCM != null)
        {
            Destroy(aC);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    void EnableReadyButton() 
    {
        ticImage.SetActive(false);
        readyButton.SetActive(true);
        PlayerConfigurationManager.instance.SetPlayerNotReady(input.playerIndex);
    }
}
