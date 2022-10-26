using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    //Tiempo que estar� muerto el objeto.
    public float deadTime;
    //Tiempo en el que no se podr� mover el objeto por el knockback.
    public float knockbackTime;
    //Tiempo en el que ser� invencible.
    public float invincibleTime;
    //Booleana que indicar� si puede recibir da�o o no.
    public bool canTakeDamage;
    //Booleana que indicar� si se est� realizando la animaci�n de muerte.
    public bool isDying = false;
    //Puntos que da al morir.
    public int scoreGiven;
    //Puntos que pierde al morir.
    [SerializeField] int scoreLost;
    //�ltimo jugador que ha golpeado este objeto. Usaremos esto para gestionar la muerte despu�s de haber sido empujado.
    public GameObject lastPlayerHittedThisObject = null;

    private void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            StartCoroutine(StartInvincibleState());
        }
    }

    private void FixedUpdate()
    {
        
    }

    public IEnumerator KnockbackApplied(GameObject kbApplier)
    {
        if (canTakeDamage)
        {
            //Indicamos qu� objeto ha sido el que ha golpeado a este.
            lastPlayerHittedThisObject = kbApplier;
            //Si este objeto es un jugador.
            if (transform.CompareTag("Player"))
            {
                //Indicamos que el jugador no se puede mover y esperamos el tiempo necesario para volver a permitirlo.
                PlayerMovement pM = GetComponent<PlayerMovement>();
                pM.canMove = false;
                yield return new WaitForSeconds(knockbackTime);
                pM.canMove = true;
                //Comprobamos cada 0.25 segundos si est� tocando el suelo. Cuando lo est� tocando, quitamos el �ltimo objeto que ha golpeado a este.
                InvokeRepeating("PlayerCheckGround", 0f, 0.25f);
            }
            //Si no, significa que es un enemigo.
            else
            {
                //Desactivamos el movimiento de la IA.
                EnemyAI eAI = GetComponent<EnemyAI>();
                eAI.canMove = false;
                eAI.anim.SetBool("canMove", false);
                yield return new WaitForSeconds(knockbackTime);
                eAI.canMove = true;
                eAI.anim.SetBool("canMove", true);
                InvokeRepeating("EnemyCheckGround", 0f, 0.25f);
            }
        }
    }

    void PlayerCheckGround()
    {
        if (GetComponent<PlayerMovement>().isGrounded)
        {
            lastPlayerHittedThisObject = null;
            CancelInvoke();
        }
    }

    void EnemyCheckGround()
    {
        if (GetComponent<EnemyAI>().isGrounded)
        {
            lastPlayerHittedThisObject = null;
            CancelInvoke();
        }
    }

    public void StartDeathCoroutine()
    {
        if (!isDying)
        {
            //Le damos los puntos al objeto que ha golpeado a este objeto, si es que existe.
            AddScoreToLastPlayerHittedThisObject();
            //Aplicamos la p�rdida de puntos.
            ScoreManager.instance.LoseScore(GetComponent<PlayerInputHandler>().pConfig, scoreLost);
            //Quitamos el objeto que tengamos.
            GetComponent<PlayerInventory>().RemoveCurrentItem();
            AudioController.instance.PlaySFX(AudioController.instance.death);
            PlayerSpawn pS = PlayerSpawn.instance;
            //Accedemos al m�todo de muerte del script PlayerSpawn.
            pS.OnDie(gameObject, this);
        }
    }

    public void StartEnemyDeathCoroutine()
    {
        if (!isDying)
        {
            //Le damos los puntos al objeto que ha golpeado a este objeto, si es que existe.
            AddScoreToLastPlayerHittedThisObject();
            StartCoroutine(EnemyDeathAnimation());
        }
    }

    IEnumerator EnemyDeathAnimation()
    {
        //Indicamos que est� muriendo.
        isDying = true;
        //Hacemos que no pueda recibir da�o.
        canTakeDamage = false;
        //Desactivamos la gravedad.
        Rigidbody2D eRB = GetComponent<Rigidbody2D>();
        eRB.gravityScale = 0f;
        eRB.velocity = Vector2.zero;
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        //Desactivamos el movimiento de la IA.
        EnemyAI eAI = GetComponent<EnemyAI>();
        eAI.canMove = false;
        eAI.attackPoint.SetActive(false);
        eAI.anim.SetTrigger("death");
        yield return new WaitForSeconds(1.01f);
        //Destruimos el objeto.
        Destroy(gameObject);
    }

    public IEnumerator StartInvincibleState()
    {
        //Indicamos que no puede recibir da�o.
        canTakeDamage = false;
        SpriteRenderer sR = GetComponent<PlayerComponents>().sR;
        StartCoroutine(ToggleSpriteRenderer(sR));
        //Esperamos los segundos de invencibilidad.
        yield return new WaitForSeconds(invincibleTime);
        //Indicamos que puede volver a recibir da�o.
        canTakeDamage = true;
        sR.enabled = true;
    }

    public void AddScoreToLastPlayerHittedThisObject()
    {
        //Si ha sido golpeado por otro objeto.
        if (lastPlayerHittedThisObject != null)
        {
            //A�adimos la puntuaci�n al otro objeto.
            ScoreManager.instance.AddScore(lastPlayerHittedThisObject.GetComponent<PlayerInputHandler>().pConfig, scoreGiven);
        }
    }

    IEnumerator ToggleSpriteRenderer(SpriteRenderer sR)
    {
        while (canTakeDamage == false)
        {
            sR.enabled = false;
            yield return new WaitForSeconds(0.2f);
            sR.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
