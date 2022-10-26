using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //Punto A y B.
    [SerializeField] Transform aPoint, bPoint;
    //Punto al que se está yendo actualmente.
    [SerializeField] Transform currentDestination;
    //Cuando la distancia entre la IA y el waypoint sea inferior a este valor, se escogerá un nuevo waypoint.
    [SerializeField] float changeWaypointDistance = 0.1f;
    //Velocidad de movimiento.
    [SerializeField] float moveSpeed;
    public GameObject attackPoint;
    //Booleana que permite el movimiento.
    public bool canMove;
    //
    public bool isGrounded;
    SpriteRenderer sR;

    public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sR = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        currentDestination = aPoint;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            CheckDestinationDistance();
        }
        RotateSprite();
    }

    void CheckDestinationDistance()
    {
        //Movemos la posición de la IA poco a poco con MoveTowards. Sólo lo hacemos en x.
        transform.position = new Vector2(
            Mathf.MoveTowards(transform.position.x, currentDestination.position.x, moveSpeed * Time.deltaTime),
            transform.position.y
            );
        //Si la distancia al punto al que se está yendo es menor a la distancia a partir de la cual se cambia de waypoint.
        if (Vector2.Distance(gameObject.transform.position, currentDestination.position) < changeWaypointDistance)
        {
            //Cambiamos de waypoint.
            ChangeDestination();
        }
    }

    void ChangeDestination()
    {
        if (currentDestination == aPoint)
        {
            currentDestination = bPoint;
        }
        else
        {
            currentDestination = aPoint;
        }
    }

    void RotateSprite()
    {
        Vector2 lookDir = currentDestination.position - transform.position;
        lookDir.Normalize();
        //Si es menor que 0, está mirando a la izquierda.
        sR.flipX = !(lookDir.x < 0);
    }
}
