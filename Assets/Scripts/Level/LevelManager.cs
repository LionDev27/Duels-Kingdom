using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Referencia al PlayerInitializer para inicializar a los jugadores despu�s de cargar el primer nivel.
    public PlayerInitializer pI;
    //Array que contendr� los distintos niveles, ordenados por dificultad.
    [SerializeField] GameObject[] easyLevels, mediumLevels, hardLevels;
    //Array que contendr� las canciones del juego.
    [SerializeField] AudioClip[] soundtrack;
    //Dificultad del nivel que se va a inicializar, siendo 0 f�cil, 1 medio y 2 dif�cil.
    int nextGameDifficult = 0;
    //Nivel m�ximo de dificultad que tendr� el juego.
    [SerializeField] int maxGameDifficult;
    //�ndice del men� principal.
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
        //Si la dificultad es mayor a la m�xima.
        if (nextGameDifficult > maxGameDifficult)
        {
            //Mostramos el men� del ganador y volvemos al men�.
            ScoreManager.instance.ShowEndGameScores();
            return;
        }
        //Si tenemos alg�n nivel cargado,
        if (currentLevel != null)
        {
            //lo destruimos
            Destroy(currentLevel);
            //y reseteamos la posici�n de la c�mara.
            cM.ResetPosition();
        }
        //Spawneamos a los jugadores.
        PlayerSpawn.instance.SpawnAllPlayers();
        AudioController aC = AudioController.instance;
        //Con un switch, dependiendo de qu� dificultad de nivel tendremos que inicializar, instanciaremos
        switch (nextGameDifficult)
        {
            //En caso de ser f�cil, un nivel aleatorio del array de niveles f�ciles.
            case 0:
                currentLevel = Instantiate(easyLevels[Random.Range(0, easyLevels.Length)], transform.position, transform.rotation);
                aC.StartCoroutine(aC.DecreaseMusicVolumeInTime(soundtrack[0]));
                break;
            //En caso de ser medio, un nivel aleatorio del array de niveles medios.
            case 1:
                currentLevel = Instantiate(mediumLevels[Random.Range(0, mediumLevels.Length)], transform.position, transform.rotation);
                aC.StartCoroutine(aC.DecreaseMusicVolumeInTime(soundtrack[1]));
                break;
            //En caso de ser dif�cil, un nivel aleatorio del array de niveles dif�ciles.
            case 2:
                currentLevel = Instantiate(hardLevels[Random.Range(0, hardLevels.Length)], transform.position, transform.rotation);
                aC.StartCoroutine(aC.DecreaseMusicVolumeInTime(soundtrack[2]));
                break;
        }
        //Comenzamos a mover la c�mara pas�ndole como posici�n final la asignada en el script GoalController de la meta que tiene el nivel.
        cM.StartCoroutine(cM.StartMovingCamera(currentLevel.GetComponentInChildren<GoalController>().lastCameraPosition.position));
        //Aumentamos la dificultad del siguiente nivel s�lo si no se iguala o se supera el nivel m�ximo de dificultad.
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
        //Volvemos al men�.
        SceneManager.LoadScene(menuSceneIndex);
    }
}
