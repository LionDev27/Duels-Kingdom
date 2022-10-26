using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerConfigurationManager : MonoBehaviour
{
    //Lista de configuraciones de los distintos jugadores.
    public List<PlayerConfiguration> playerConfigs;
    //Mínimo de jugadores que podrá haber en la partida.
    [SerializeField] private int minPlayers = 2;
    //Índice de la escena de juego que se cargará cuando todos los jugadores estén listos.
    public int gameSceneIndex;
    [SerializeField] GameObject helpTexts;

    //SINGLETON.
    public static PlayerConfigurationManager instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            //Hacemos que al cargar una escena, no se elimine este script.
            DontDestroyOnLoad(instance);
            //Inicializamos la lista.
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    //En el futuro, lo cambiaremos a SetPlayerSprite.
    public void SetPlayerColor(int index, Color c)
    {
        playerConfigs[index].playerColor = c;

    }

    public void SetPlayerReady(int index)
    {
        //Indicamos que el jugador está preparado.
        playerConfigs[index].isReady = true;
        //Recorremos la lista de jugadores y, con un contador, vemos cuántos jugadores están preparados.
        int playersReady = 0;
        foreach (PlayerConfiguration pC in playerConfigs)
        {
            if (pC.isReady)
            {
                playersReady++;
            }
        }
        //Si se han unido los suficientes jugadores y todos los jugadores están preparados.
        if (playerConfigs.Count == minPlayers && playerConfigs.Count == playersReady)
        {
            SceneManager.LoadScene(gameSceneIndex);
        }
    }

    public void SetPlayerNotReady(int index)
    {
        playerConfigs[index].isReady = false;
    }

    public void JoinPlayer(PlayerInput pI)
    {
        Debug.Log($"Player {pI.playerIndex} Joined");
        //Recorremos la lista de jugadores. Si el jugador que se quiere unir ya está en la lista, lo indicamos con la booleana.
        bool alreadyJoined = false;
        foreach (PlayerConfiguration pC in playerConfigs)
        {
            alreadyJoined = pC.playerIndex == pI.playerIndex;
        }
        //Si el jugador que se quiere unir no se ha unido aún.
        if (!alreadyJoined)
        {
            //Hacemos que este objeto sea su padre.
            pI.transform.SetParent(transform);
            //Lo añadimos a la lista de jugadores.
            playerConfigs.Add(new PlayerConfiguration(pI));
            //Si es el primer jugador en unirse, activamos el texto de ayuda.
            if (playerConfigs.Count == 1)
            {
                ShowHelpText();
            }
            SuscribeHelpTextEvents(pI);
        }
    }

    void SuscribeHelpTextEvents(PlayerInput pI)
    {
        foreach (InputAction action in pI.actions.FindActionMap("UI").actions)
        {
            action.performed += (x) => helpTexts.GetComponent<ChangeHelpTextsWithLastDevice>().ChangeHelpTexts(pI.currentControlScheme);
        }
    }

    void ShowHelpText()
    {
        helpTexts.SetActive(true);
    }
}

public class PlayerConfiguration
{
    //Usaremos esto para unir a un jugador nuevo.
    public PlayerConfiguration(PlayerInput pI)
    {
        playerIndex = pI.playerIndex;
        input = pI;
    }

    //Variables que tendrá cada jugador: input, índice, boolena que indica si está listo, el color y la puntuación.
    public PlayerInput input;
    public int playerIndex;
    public bool isReady;
    public Color playerColor;
    public int playerScore;
}
