using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KnockbackApplier))]
public class DamageDealer : MonoBehaviour
{
    //Referencia al transform del objeto de referencia que usa este DamageDealer.
    public Transform referenceItem;
    //Tags que se comprobar�n cuando un objeto entre en el trigger del objeto.
    public string playerTag, enemyTag, destroyableTag;
    //Booleana que cambiar� seg�n el tipo de ataque que tengamos.
    public bool isLethal;
    //Booelana que indicar� si este �tem es una bala o no. Lo usaremos para hacer comprobaciones extra.
    public bool isBullet;
    //Multiplicador de knockback que se modificar� seg�n los objetos que tengamos.
    public float knockbackMultiplier;
    //Referencia al KnockbackApplier del objeto.
    KnockbackApplier kA;

    private void Start()
    {
        kA = GetComponent<KnockbackApplier>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el Trigger se activa con un jugador o con un enemigo.
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            //Comprobamos si est� colisionando con el propio jugador que ataca. Si es as�, ignora la colisi�n.
            //Esto lo hacemos para que el jugador no se pueda pegar a s� mismo.
            if (collision.gameObject == referenceItem.gameObject) return;
            //Si el objeto puede recibir da�o.
            if (collision.GetComponent<Damageable>().canTakeDamage)
            {
                Debug.Log("Attacking player or enemy");
                //Comprobamos si es letal. Si es as�, accedemos a su DAMAGEABLE y activamos sus comportamientos de muerte.
                if (isLethal)
                {
                    //A�adimos la puntuaci�n al jugador que tiene este DamageDealer.
                    ScoreManager.instance.AddScore(
                        referenceItem.GetComponent<PlayerInputHandler>().pConfig,
                        collision.GetComponent<Damageable>().scoreGiven
                        );
                    //Si matamos a un jugador.
                    if (collision.CompareTag("Player"))
                    {
                        //Empezamos su corrutina de muerte.
                        collision.GetComponent<Damageable>().StartDeathCoroutine();
                    }
                    //Si no.
                    else
                    {
                        //Empezamos su corrutina de muerte.
                        collision.GetComponent<Damageable>().StartEnemyDeathCoroutine();
                    }
                }
                //Si no es letal.
                else
                {
                    Vector2 knockback = new Vector2();
                    //Si el �tem de referencia es el jugador y este DamageDealer no lo tiene una bala.
                    if (referenceItem.CompareTag("Player") && !isBullet)
                    {
                        //Definimos el vector director Knockback, que ser� la posici�n de este objeto - la del jugador.
                        knockback = transform.position - referenceItem.position;
                    }
                    //Si es la bala o un enemigo.
                    else
                    {
                        //Definimos el vector director Knockback, que ser� la posici�n del objeto con el que se ha colisionado - la posici�n de este objeto.
                        knockback = collision.transform.position - transform.position;
                    }
                    //Aplicamos el knockback. Si este DamageDealer lo tiene un enemigo, no pasamos item de referencia.
                    if (referenceItem.CompareTag("Enemy"))
                    {
                        kA.ApplyKnockback(knockback, collision.transform, knockbackMultiplier);
                    }
                    else
                    {
                        kA.ApplyKnockback(knockback, collision.transform, knockbackMultiplier, referenceItem.gameObject);
                    }
                    AudioController.instance.PlaySFX(AudioController.instance.hit);
                }
            }
        }
        //Si colisiona con un destruible (la caja)
        if (collision.CompareTag("Destroyable"))
        {
            //Iniciamos su m�todo para que se destruya e instancie un objeto.
            collision.GetComponent<DestroyableBox>().DestroyAndInstantiateObject();
        }
    }
}
