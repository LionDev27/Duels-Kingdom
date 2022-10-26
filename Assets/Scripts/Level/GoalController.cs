using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    //Array de puntuaciones por haber llegado a la meta.
    [SerializeField] int[] scoresForArriving;
    //Cantidad de jugadores que han llegado a la meta. Lo usaremos como orden de llegada.
    List<GameObject> playersArrived = new List<GameObject>();
    //Tiempo para pasar de nivel que empezará a contar desde que llega el primer jugador a la meta.
    [SerializeField] float endLevelTimer;
    //Booleana que indicará si todos los jugadores han llegado.
    bool allPlayersArrived = false;

    //Posición en la que acabará la cámara.
    public Transform lastCameraPosition;

    PlayerInitializer pInitializer;

    private void Start()
    {
        pInitializer = LevelManager.instance.pI;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CancelInvoke("EndLevelAndNoPlayersInGoal");
            //Añadimos la puntuación del jugador.
            ScoreManager.instance.AddScore(collision.GetComponent<PlayerInputHandler>().pConfig, scoresForArriving[playersArrived.Count]);
            //Añadimos al jugador a la lista de jugadores que han llegado.
            playersArrived.Add(collision.gameObject);
            //Desactivamos sus controles.
            collision.GetComponent<PlayerInputHandler>().pConfig.input.DeactivateInput();
            //Hacemos que no pueda recibir daño.
            collision.GetComponent<Damageable>().canTakeDamage = false;
            //Eliminamos su objeto.
            collision.GetComponent<PlayerInventory>().RemoveCurrentItem();
            //Si es el primer jugador en llegar, llamamos al método EndLevel después del tiempo de espera.
            if (collision.gameObject == playersArrived[0]) Invoke("EndLevel", endLevelTimer);
            //Si todos los jugadores de la partida han llegado a la meta.
            if (playersArrived.Count == pInitializer.players.Count)
            {
                //Cancelamos el Invoke para que no se llame dos veces al método.
                CancelInvoke();
                //Indicamos que han llegado todos los jugadores.
                allPlayersArrived = true;
                //Hacemos un nuevo invoke con un pequeño periodo de tiempo para que no se muestre de forma brusca el menú de puntuaciones.
                Invoke("EndLevel", 1f);
            }
        }
    }

    public void InvokeEndLevelWithNoPlayersInGoal(float time)
    {
        Invoke("EndLevelAndNoPlayersInGoal", time);
    }

    void EndLevel()
    {
        //Si no han llegado todos los jugadores, desactivamos los controles de los jugadores que no han llegado.
        if (!allPlayersArrived)
        {
            //Recorremos la lista de jugadores.
            foreach (GameObject g in pInitializer.players)
            {
                //Recorremos la lista de jugadores que han llegado.
                foreach (GameObject pA in playersArrived)
                {
                    //Si el jugador no ha llegado
                    if (g != pA)
                    {
                        //Desactivamos sus controles.
                        g.GetComponent<PlayerInputHandler>().pConfig.input.DeactivateInput();
                        //Hacemos que no pueda recibir daño.
                        g.GetComponent<Damageable>().canTakeDamage = false;
                        //Eliminamos su objeto.
                        g.GetComponent<PlayerInventory>().RemoveCurrentItem();
                    }
                }
            }
        }
        //Mostramos las puntuaciones de los jugadores.
        ScoreManager.instance.ShowFinalScores();
    }

    void EndLevelAndNoPlayersInGoal()
    {
        //Si no han llegado todos los jugadores, desactivamos los controles de los jugadores que no han llegado.
        if (!allPlayersArrived)
        {
            //Recorremos la lista de jugadores.
            foreach (GameObject g in pInitializer.players)
            {
                //Desactivamos sus controles.
                g.GetComponent<PlayerInputHandler>().pConfig.input.DeactivateInput();
                //Hacemos que no pueda recibir daño.
                g.GetComponent<Damageable>().canTakeDamage = false;
                //Eliminamos su objeto.
                g.GetComponent<PlayerInventory>().RemoveCurrentItem();
            }
        }
        //Mostramos las puntuaciones de los jugadores.
        ScoreManager.instance.ShowFinalScores();
    }
}
