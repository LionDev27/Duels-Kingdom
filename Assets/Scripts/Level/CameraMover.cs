using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    //Velocidad a la que se mueve la cámara.
    [SerializeField] float moveSpeed;
    //Tiempo que tarda en empezar a moverse.
    [SerializeField] float startMovingTime;
    //
    [SerializeField] float timeToShowScoreWhenArriving;
    //Posiciones iniciales y finales.
    Vector2 initPos, finalPos;
    //Booleana que permite el movimiento.
    bool canMove;

    Rigidbody2D rB;

    private void Awake()
    {
        rB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //Igualamos la posición inicial a la posición actual.
        initPos = transform.position;
    }

    private void FixedUpdate()
    {
        //Si tiene el movimiento activado, entramos en el método.
        if (canMove)
        {
            MoveCamera();
        }
    }

    void MoveCamera()
    {
        //La dirección será la final - la inicial, pero sólo en x. Esto lo hacemos así por si en algún nivel no se moviera de izquierda a derecha.
        Vector2 moveDir = new Vector2(finalPos.x - initPos.x, 0);
        //Normalizamos el vector.
        moveDir.Normalize();
        //Actualizamos la velocidad del Rigidbody multiplicada por su velocidad.
        rB.velocity = moveDir * moveSpeed * Time.fixedDeltaTime;
        //Si la posición en x es mayor o igual a la posición en x final.
        if (transform.position.x >= finalPos.x)
        {
            //Dejamos de mover este objeto.
            canMove = false;
            //Igualamos su velocidad a 0.
            rB.velocity = Vector2.zero;
            //Comenzamos la cuenta atrás para mostrar la puntuación.
            FindObjectOfType<GoalController>().InvokeEndLevelWithNoPlayersInGoal(timeToShowScoreWhenArriving);
        }
    }

    public IEnumerator StartMovingCamera(Vector2 fPos)
    {
        //Esperamos el tiempo de espera.
        yield return new WaitForSeconds(startMovingTime);
        //Indicamos que puede moverse el objeto.
        canMove = true;
        //Igualamos la posición final a la obtenida.
        finalPos = fPos;
    }

    public void ResetPosition()
    {
        //Movemos el objeto a la posición inicial.
        transform.position = initPos;
    }
}
