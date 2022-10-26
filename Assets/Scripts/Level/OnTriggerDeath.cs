using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDeath : MonoBehaviour
{
    [SerializeField] bool isSpikes;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si entra un jugador.
        if (collision.CompareTag("Player"))
        {
            Damageable dmg = collision.GetComponent<Damageable>();
            if (!dmg.canTakeDamage && !isSpikes)
            {
                return;
            }
            //Empezamos su corrutina de muerte.
            dmg.StartDeathCoroutine();
        }
        //Si entra un enemigo.
        if (collision.CompareTag("Enemy"))
        {
            //Accedemos a su damageable y empezamos su corrutina de muerte.
            collision.GetComponent<Damageable>().StartEnemyDeathCoroutine();
        }
        //Si es una caja o un item.
        if (collision.CompareTag("Destroyable") || collision.CompareTag("Item"))
        {
            //Destruimos el objeto.
            Destroy(collision.gameObject);
        }
    }
}
