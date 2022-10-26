using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("Puntuaciones InGame")]
    //Transform del panel donde se mostrarán las puntuaciones.
    [SerializeField] Transform scorePanel;
    //Prefab de la puntuación del jugador.
    [SerializeField] GameObject scorePrefab;
    //Lista con los textos de puntuación de cada jugador, para ir actualizándolos.
    public List<Text> playerScores;

    [Header("Puntuaciones al acabar cada nivel")]
    //Menú de puntuaciones que se mostrará al final de cada nivel.
    [SerializeField] GameObject finalScoreMenu;
    //Transform donde se instanciarán las puntuaciones finales de cada nivel.
    [SerializeField] Transform playerScoresParent;
    //Prefab de la puntuación final del jugador.
    [SerializeField] GameObject playerFinalScorePrefab;
    //Tiempo en el que se mostrarán las puntuaciones.
    [SerializeField] float showScoreTime;
    //Lista con los textos de puntuación final de cada jugador para actualizarlos al final de cada nivel.
    public List<Text> playerFinalScores;

    [Header("Fin del juego y ganadores")]
    //Menú de puntuaciones del final del juego.
    [SerializeField] GameObject winnerMenu;
    //Texto que indicará quién será el ganador.
    [SerializeField] Text winnerText;
    //Sprite del jugador ganador.
    [SerializeField] Image winnerSprite;
    //Tiempo en el que se mostrarán las puntuaciones finales.
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
        //Instanciamos una puntuación del jugador.
        GameObject tempPS = Instantiate(scorePrefab, scorePanel);
        //Actualizamos su color.
        tempPS.GetComponentInChildren<Image>().sprite = s;
        //Guardamos su componente Text para actualizarlo.
        playerScores.Add(tempPS.GetComponentInChildren<Text>());
        //Instanciamos la puntuación final de cada nivel.
        InstantiatePlayerFinalScore(s);
    }

    void InstantiatePlayerFinalScore(Sprite s)
    {
        //Instanciamos una puntuación final.
        GameObject tempPFS = Instantiate(playerFinalScorePrefab, playerScoresParent);
        //Actualizamos su color.
        tempPFS.GetComponentInChildren<Image>().sprite = s;
        //Añadimos su componente Text para actualizarlo.
        playerFinalScores.Add(tempPFS.GetComponentInChildren<Text>());
    }

    public void AddScore(PlayerConfiguration pC, int score)
    {
        //Sumamos su puntuación en su PlayerConfiguration.
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
            //Accedemos a la puntuación de cada jugador.
            playerFinalScores[i].text = $"{LevelManager.instance.pI.players[i].GetComponent<PlayerInputHandler>().pConfig.playerScore} points";
        }
        //Mostramos el menú de puntuaciones.
        finalScoreMenu.SetActive(true);
        //Después del tiempo, desactivamos el menú de puntuaciones y cargamos el siguiente nivel.
        Invoke("HideFinalScoresAndLoadNextLevel", showScoreTime);
    }

    void HideFinalScoresAndLoadNextLevel()
    {
        //Cargamos el siguiente nivel.
        LevelManager.instance.InitializeLevel();
        //Desactivamos el menú de puntuaciones.
        finalScoreMenu.SetActive(false);
    }

    public void ShowEndGameScores()
    {
        //Lo activamos.
        winnerMenu.SetActive(true);
        //Variable local donde guardaremos al ganador actual.
        GameObject currentWinner = new GameObject();
        //Variable de puntuación que usaremos para compararla entre los distintos jugadores.
        int currentMaxScore = 0;
        foreach (GameObject player in LevelManager.instance.pI.players)
        {
            //Si la actual puntuación máxima es menor que la puntuación del jugador al que se está comprobando.
            if (currentMaxScore < player.GetComponent<PlayerInputHandler>().pConfig.playerScore)
            {
                //Actualizamos la máxima puntuación actual.
                currentMaxScore = player.GetComponent<PlayerInputHandler>().pConfig.playerScore;
                //Guardamos al jugador como ganador actual.
                currentWinner = player;
            }
        }
        //Actualizamos el texto con el índice del jugador ganador. Le sumamos 1 para que no cuente como jugador 0, sino como jugador 1.
        winnerText.text = $"Player {currentWinner.GetComponent<PlayerInputHandler>().pConfig.playerIndex + 1} has win!";
        //Actualizamos su sprite.
        winnerSprite.sprite = currentWinner.GetComponentInChildren<SpriteRenderer>().sprite;
        //Finalizamos el juego después del tiempo de muestra del ganador.
        LevelManager.instance.Invoke("EndGame", showWinnerTime);
    }
}
