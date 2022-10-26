using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawn : MonoBehaviour
{
    //Array de puntos de Spawn.
    Transform[] spawnPoints;

    //SINGLETON.
    public static PlayerSpawn instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        //Obtenemos los puntos de spawn.
        spawnPoints = GetComponent<PlayerInitializer>().playerSpawns;
    }

    public void OnDie(GameObject g, Damageable dmg)
    {
        //ANIMACI�N DE MUERTE.
        StartCoroutine(DeathAnimation(g, dmg));
        //Realizamos el shake de la c�mara.
        GameManager.instance.StartCameraShake();
    }

    IEnumerator DeathAnimation(GameObject g, Damageable dmg)
    {
        //Indicamos que est� muriendo.
        dmg.isDying = true;
        //Hacemos que no pueda recibir da�o.
        dmg.canTakeDamage = false;
        //Desactivamos la gravedad y su velocidad.
        Rigidbody2D eRB = g.GetComponent<Rigidbody2D>();
        eRB.gravityScale = 0f;
        eRB.velocity = Vector2.zero;
        //Desactivamos sus controles.
        g.GetComponent<PlayerInputHandler>().pConfig.input.DeactivateInput();
        Animator anim = g.GetComponent<Animator>();
        anim.SetTrigger("death");
        yield return new WaitForSeconds(1f);
        //Spawneamos al jugador.
        SpawnOnePlayer(g);
        //Iniciamos su estado de invencibilidad.
        dmg.StartCoroutine(dmg.StartInvincibleState());
        //Indicamos que ya no est� muriendo.
        dmg.isDying = false;
    }

    public void SpawnOnePlayer(GameObject player)
    {
        //Lo activamos.
        player.SetActive(true);
        //Guardamos el �ndice del jugador.
        int pIndex = player.GetComponent<PlayerInputHandler>().pConfig.playerIndex;
        //Activamos su movimiento en caso de estar desactivado.
        player.GetComponent<PlayerMovement>().canMove = true;
        //Activamos su ataque en caso de estar desactivado.
        player.GetComponent<PlayerAttack>().canAttack = true;
        //Activamos sus controles en caso de estar desactivados.
        player.GetComponent<PlayerInputHandler>().pConfig.input.ActivateInput();
        //
        PlayerSpawnController pSC = player.GetComponent<PlayerSpawnController>();
        //Le pasamos la posici�n del spawn que equivale a su �ndice.
        pSC.spawnPosition = spawnPoints[pIndex].transform;
        pSC.enabled = true;
        pSC.DeactivateGravity();
    }

    public void SpawnAllPlayers()
    {
        //Recorremos la lista de jugadores del script PlayerInitializer.
        foreach (GameObject player in LevelManager.instance.pI.players)
        {
            //Guardamos el �ndice del jugador.
            int pIndex = player.GetComponent<PlayerInputHandler>().pConfig.playerIndex;
            //Lo movemos a su posici�n del spawn correspondiente.
            player.transform.position = spawnPoints[pIndex].transform.position;
            //Activamos sus controles.
            player.GetComponent<PlayerInputHandler>().pConfig.input.ActivateInput();
            //Iniciamos su estado de invencibilidad.
            player.GetComponent<Damageable>().StartCoroutine(player.GetComponent<Damageable>().StartInvincibleState());
        }
    }
}
