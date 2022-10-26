using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Referencia al PlayerInitializer para inicializar a los jugadores después de cargar el primer nivel.
    public PlayerInitializer pI;
    //Array que contendrá los distintos niveles, ordenados por dificultad.
    [SerializeField] GameObject[] easyLevels, mediumLevels, hardLevels;
    //Array que contendrá las canciones del juego.
    [SerializeField] AudioClip[] soundtrack;
    //Dificultad del nivel que se va a inicializar, siendo 0 fácil, 1 medio y 2 difícil.
    int nextGameDifficult = 0;
    //Nivel máximo de dificultad que tendrá el juego.
    [SerializeField] int maxGameDifficult;
    //Índice del menú principal.
    [SerializeField] int menuSceneIndex;
    //Nivel actual.
    GameObject currentLevel;

    CameraMover cM;

    //SINGLETON.
    public static LevelManager instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        cM = GetComponentInChildren<CameraMover>();
    }

    private void Start()
    {
        InitializeLevel();
        pI.InitializePlayers();
    }

    public void InitializeLevel()
    {
        //Si la dificultad es mayor a la máxima.
        if (nextGameDifficult > maxGameDifficult)
        {
            //Mostramos el menú del ganador y volvemos al menú.
            ScoreManager.instance.ShowEndGameScores();
            return;
        }
        //Si tenemos algún nivel cargado,
        if (currentLevel != null)
        {
            //lo destruimos
            Destroy(currentLevel);
            //y reseteamos la posición de la cámara.
            cM.ResetPosition();
        }
        //Spawneamos a los jugadores.
        PlayerSpawn.instance.SpawnAllPlayers();
        AudioController aC = AudioController.instance;
        //Con un switch, dependiendo de qué dificultad de nivel tendremos que inicializar, instanciaremos
        switch (nextGameDifficult)
        {
            //En caso de ser fácil, un nivel aleatorio del array de niveles fáciles.
            case 0:
                currentLevel = Instantiate(easyLevels[Random.Range(0, easyLevels.Length)], transform.position, transform.rotation);
                aC.StartCoroutine(aC.DecreaseMusicVolumeInTime(soundtrack[0]));
                break;
            //En caso de ser medio, un nivel aleatorio del array de niveles medios.
            case 1:
                currentLevel = Instantiate(mediumLevels[Random.Range(0, mediumLevels.Length)], transform.position, transform.rotation);
                aC.StartCoroutine(aC.DecreaseMusicVolumeInTime(soundtrack[1]));
                break;
            //En caso de ser difícil, un nivel aleatorio del array de niveles difíciles.
            case 2:
                currentLevel = Instantiate(hardLevels[Random.Range(0, hardLevels.Length)], transform.position, transform.rotation);
                aC.StartCoroutine(aC.DecreaseMusicVolumeInTime(soundtrack[2]));
                break;
        }
        //Comenzamos a mover la cámara pasándole como posición final la asignada en el script GoalController de la meta que tiene el nivel.
        cM.StartCoroutine(cM.StartMovingCamera(currentLevel.GetComponentInChildren<GoalController>().lastCameraPosition.position));
        //Aumentamos la dificultad del siguiente nivel sólo si no se iguala o se supera el nivel máximo de dificultad.
        nextGameDifficult++;
    }

    public void EndGame()
    {
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
        //Volvemos al menú.
        SceneManager.LoadScene(menuSceneIndex);
    }
}
