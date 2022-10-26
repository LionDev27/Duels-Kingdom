using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInitializer : MonoBehaviour
{
    //Spawns de los jugadores.
    public Transform[] playerSpawns;
    //Lista con todos los jugadores InGame.
    public List<GameObject> players;
    //Prefab de los jugadores.
    [SerializeField] GameObject[] playerPrefabs;
    [SerializeField] UIController uIC;

    public void InitializePlayers()
    {
        //Lista de configuraciones de los jugadores que se unieron en el men� de uni�n.
        List<PlayerConfiguration> pConfigs = PlayerConfigurationManager.instance.playerConfigs;
        //Con un for que se repite seg�n el n�mero de jugadores que haya.
        for (int i = 0; i < pConfigs.Count; i++)
        {
            //Instanciamos a cada jugador en su posici�n.
            GameObject tempPlayer = Instantiate(playerPrefabs[i], playerSpawns[i].position, playerSpawns[i].rotation);
            //Obtenemos su InputHandler y lo inicializamos, pas�ndole su PlayerConfiguration.
            tempPlayer.GetComponent<PlayerInputHandler>().InitializeNewPlayer(pConfigs[i]);
            //Cambiamos el prompt de interacci�n en funci�n del dispositivo asociado al jugador.
            tempPlayer.GetComponent<PlayerInventory>().ChangePrompt((pConfigs[i].input.currentControlScheme));
            //Obtenemos el sprite del jugador.
            Sprite pSprite = playerPrefabs[i].GetComponentInChildren<SpriteRenderer>().sprite;
            //Instanciamos una nueva puntuaci�n de jugador pas�ndole su sprite.
            ScoreManager.instance.InstantiatePlayerScore(pSprite);
            //Lo a�adimos a la lista de jugadores.
            players.Add(tempPlayer);
        }
        uIC.enabled = true;
    }
}
