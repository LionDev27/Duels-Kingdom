using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("Puntuaciones InGame")]
    //Transform del panel donde se mostrar�n las puntuaciones.
    [SerializeField] Transform scorePanel;
    //Prefab de la puntuaci�n del jugador.
    [SerializeField] GameObject scorePrefab;
    //Lista con los textos de puntuaci�n de cada jugador, para ir actualiz�ndolos.
    public List<Text> playerScores;

    [Header("Puntuaciones al acabar cada nivel")]
    //Men� de puntuaciones que se mostrar� al final de cada nivel.
    [SerializeField] GameObject finalScoreMenu;
    //Transform donde se instanciar�n las puntuaciones finales de cada nivel.
    [SerializeField] Transform playerScoresParent;
    //Prefab de la puntuaci�n final del jugador.
    [SerializeField] GameObject playerFinalScorePrefab;
    //Tiempo en el que se mostrar�n las puntuaciones.
    [SerializeField] float showScoreTime;
    //Lista con los textos de puntuaci�n final de cada jugador para actualizarlos al final de cada nivel.
    public List<Text> playerFinalScores;

    [Header("Fin del juego y ganadores")]
    //Men� de puntuaciones del final del juego.
    [SerializeField] GameObject winnerMenu;
    //Texto que indicar� qui�n ser� el ganador.
    [SerializeField] Text winnerText;
    //Sprite del jugador ganador.
    [SerializeField] Image winnerSprite;
    //Tiempo en el que se mostrar�n las puntuaciones finales.
    [SerializeField] float showWinnerTime;

    //SINGLETON.
    public static ScoreManager instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    //CAMBIAR A SPRITES EN EL FUTURO.
    public void InstantiatePlayerScore(Sprite s)
    {
        //Instanciamos una puntuaci�n del jugador.
        GameObject tempPS = Instantiate(scorePrefab, scorePanel);
        //Actualizamos su color.
        tempPS.GetComponentInChildren<Image>().sprite = s;
        //Guardamos su componente Text para actualizarlo.
        playerScores.Add(tempPS.GetComponentInChildren<Text>());
        //Instanciamos la puntuaci�n final de cada nivel.
        InstantiatePlayerFinalScore(s);
    }

    void InstantiatePlayerFinalScore(Sprite s)
    {
        //Instanciamos una puntuaci�n final.
        GameObject tempPFS = Instantiate(playerFinalScorePrefab, playerScoresParent);
        //Actualizamos su color.
        tempPFS.GetComponentInChildren<Image>().sprite = s;
        //A�adimos su componente Text para actualizarlo.
        playerFinalScores.Add(tempPFS.GetComponentInChildren<Text>());
    }

    public void AddScore(PlayerConfiguration pC, int score)
    {
        //Sumamos su puntuaci�n en su PlayerConfiguration.
        pC.playerScore += score;
        //Actualizamos el campo de texto.
        playerScores[pC.playerIndex].text = pC.playerScore.ToString();
    }

    public void LoseScore(PlayerConfiguration pC, int score)
    {
        pC.playerScore -= score;
        if (pC.playerScore <= 0)
        {
            pC.playerScore = 0;
        }
        playerScores[pC.playerIndex].text = pC.playerScore.ToString();
    }

    public void ShowFinalScores()
    {
        //Actualizamos todos los campos de texto.
        for (int i = 0; i < playerFinalScores.Count; i++)
        {
            //Accedemos a la puntuaci�n de cada jugador.
            playerFinalScores[i].text = $"{LevelManager.instance.pI.players[i].GetComponent<PlayerInputHandler>().pConfig.playerScore} points";
        }
        //Mostramos el men� de puntuaciones.
        finalScoreMenu.SetActive(true);
        //Despu�s del tiempo, desactivamos el men� de puntuaciones y cargamos el siguiente nivel.
        Invoke("HideFinalScoresAndLoadNextLevel", showScoreTime);
    }

    void HideFinalScoresAndLoadNextLevel()
    {
        //Cargamos el siguiente nivel.
        LevelManager.instance.InitializeLevel();
        //Desactivamos el men� de puntuaciones.
        finalScoreMenu.SetActive(false);
    }

    public void ShowEndGameScores()
    {
        //Lo activamos.
        winnerMenu.SetActive(true);
        //Variable local donde guardaremos al ganador actual.
        GameObject currentWinner = new GameObject();
        //Variable de puntuaci�n que usaremos para compararla entre los distintos jugadores.
        int currentMaxScore = 0;
        foreach (GameObject player in LevelManager.instance.pI.players)
        {
            //Si la actual puntuaci�n m�xima es menor que la puntuaci�n del jugador al que se est� comprobando.
            if (currentMaxScore < player.GetComponent<PlayerInputHandler>().pConfig.playerScore)
            {
                //Actualizamos la m�xima puntuaci�n actual.
                currentMaxScore = player.GetComponent<PlayerInputHandler>().pConfig.playerScore;
                //Guardamos al jugador como ganador actual.
                currentWinner = player;
            }
        }
        //Actualizamos el texto con el �ndice del jugador ganador. Le sumamos 1 para que no cuente como jugador 0, sino como jugador 1.
        winnerText.text = $"Player {currentWinner.GetComponent<PlayerInputHandler>().pConfig.playerIndex + 1} has win!";
        //Actualizamos su sprite.
        winnerSprite.sprite = currentWinner.GetComponentInChildren<SpriteRenderer>().sprite;
        //Finalizamos el juego despu�s del tiempo de muestra del ganador.
        LevelManager.instance.Invoke("EndGame", showWinnerTime);
    }
}
