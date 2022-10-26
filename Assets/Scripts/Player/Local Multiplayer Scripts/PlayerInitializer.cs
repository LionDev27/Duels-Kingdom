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
        //Lista de configuraciones de los jugadores que se unieron en el menú de unión.
        List<PlayerConfiguration> pConfigs = PlayerConfigurationManager.instance.playerConfigs;
        //Con un for que se repite según el número de jugadores que haya.
        for (int i = 0; i < pConfigs.Count; i++)
        {
            //Instanciamos a cada jugador en su posición.
            GameObject tempPlayer = Instantiate(playerPrefabs[i], playerSpawns[i].position, playerSpawns[i].rotation);
            //Obtenemos su InputHandler y lo inicializamos, pasándole su PlayerConfiguration.
            tempPlayer.GetComponent<PlayerInputHandler>().InitializeNewPlayer(pConfigs[i]);
            //Cambiamos el prompt de interacción en función del dispositivo asociado al jugador.
            tempPlayer.GetComponent<PlayerInventory>().ChangePrompt((pConfigs[i].input.currentControlScheme));
            //Obtenemos el sprite del jugador.
            Sprite pSprite = playerPrefabs[i].GetComponentInChildren<SpriteRenderer>().sprite;
            //Instanciamos una nueva puntuación de jugador pasándole su sprite.
            ScoreManager.instance.InstantiatePlayerScore(pSprite);
            //Lo añadimos a la lista de jugadores.
            players.Add(tempPlayer);
        }
        uIC.enabled = true;
    }
}
