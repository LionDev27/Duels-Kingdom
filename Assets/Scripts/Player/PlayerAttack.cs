using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Enumerador con los tipos de ataque que podrá tener el jugador.
public enum AttackTypes { MeleeAttack, RangedAttack }

public class PlayerAttack : PlayerComponents
{
    //Tipo de ataque que tendrá el jugador dependiendo del item que lleve encima.
    public AttackTypes pAttack;
    //Vector que nos indicará hacia dónde apunta el jugador. Lo obtendremos mediante el sistema de inputs.
    public Vector2 aimVector;
    //Vector que nos indicará dónde será el ataque a melee por defecto, es decir, el ataque a melee sin pulsar ningún botón de apuntado.
    public Vector2 defaultAttackPosition;
    //Objeto vacío que usaremos como punto de ataque. Desde él, aplicaremos el daño a melee e instanciaremos las balas.
    public Transform attackPoint;
    //Offset que podremos aplicar al punto de ataque, para controlarlo fácilmente desde el inspector.
    public Vector2 attackPointOffset;
    //Si true, el jugador puede atacar.
    public bool canAttack = true;

    [Header("Ataque a melee")]
    //Cooldown entre ataques a melee. Será el valor por defecto.
    public float defaultMeleeAttackCooldown;
    //Cooldown entre ataques melee. Será el valor que cambiará dependiendo de los items que tenga.
    public float meleeAttackCooldown;

    [Header("Ataque a distancia")]
    //Prefab de la bala que se instanciará al disparar.
    public GameObject bulletPrefab;
    //Cooldown entre ataques a distancia.
    public float rangedAttackCooldown;
    //Velocidad de la bala.
    public float bulletSpeed;

    private void Start()
    {
        meleeAttackCooldown = defaultMeleeAttackCooldown;
    }

    private void Update()
    {
        RotateAttackPointToView();
    }

    public void GetAimValues(InputAction.CallbackContext context) 
    {
        //Si el jugador presiona algun botón de apuntado.
        if (context.performed)
        {
            //Igualamos el vector de apuntado al vector pulsado.
            aimVector = context.ReadValue<Vector2>();
        }
        //Si no, dejaremos una posición por defecto, por si se ataca sin pulsar ninguna tecla de apuntado.
        else
        {
            //Si el personaje está rotado mirando hacia la izquierda.
            if (transform.rotation.y > 0)
            {
                //La defaultPosition se cambiará a la izquierda.
                aimVector = -defaultAttackPosition;
            }
            //Si no.
            else
            {
                //Igualamos aimVector a la posición default del ataque;
                aimVector = defaultAttackPosition;
            }
        }
    }

    void RotateAttackPointToView()
    {
        //Movemos attackpoint con el jugador sumando su posición al vector de apuntado y al offset.
        attackPoint.position = new Vector2(
            transform.position.x + aimVector.x + attackPointOffset.x,
            transform.position.y + aimVector.y + attackPointOffset.y)
            ;
    }

    public void Attack(InputAction.CallbackContext obj)
    {
        //Si puede atacar
        if (canAttack)
        {
            //Impedimos que el jugador ataque de nuevo.
            canAttack = false;
            //Y elegimos uno de los tipos de ataque dependiendo de cuál esté seleccionado actualmente.
            switch (pAttack)
            {
                case AttackTypes.MeleeAttack:
                    StartCoroutine(MeleeAttack());
                    break;
                case AttackTypes.RangedAttack:
                    StartCoroutine(RangedAttack());
                    break;
            }
        }
    }

    IEnumerator MeleeAttack()
    {
        Debug.Log("Attacking: Melee Attack");

        //Ejecutamos la animación de ataque.
        anim.SetTrigger("attack");
        //Aplicamos la espera del ataque a melee.
        yield return new WaitForSeconds(meleeAttackCooldown);

        canAttack = true;
    }

    IEnumerator RangedAttack()
    {
        Debug.Log("Attacking: Ranged Attack");
        //Instanciamos la bala.
        GameObject tempBullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
        //Indicamos el vector director y lo normalizamos.
        Vector2 shootDir = attackPoint.position - transform.position;
        shootDir.Normalize();
        //Obtenemos su velocidad y la igualamos al vector director multiplicada por la velocidad.
        tempBullet.GetComponent<Rigidbody2D>().velocity = shootDir * bulletSpeed;
        //Igualamos el ítem de referencia al objeto actual.
        tempBullet.GetComponent<DamageDealer>().referenceItem = transform;
        yield return new WaitForSeconds(rangedAttackCooldown);

        canAttack = true;
    }
}
