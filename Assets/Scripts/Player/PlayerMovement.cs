using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerComponents
{
    [Header("Variables de movimiento")]
    //Vector donde guardaremos la informaci�n proporcionada por el sistema de inputs. Nos dir� si estamos pulsando uno de los botones de movimiento.
    public Vector2 movement;
    //Velocidad de desplazamiento lateral del jugador.
    public float speed;
    //Booleana que impedir� o permitir� el movimiento.
    public bool canMove;

    [Header("Variables de salto")]
    //Vector donde guardaremos la informaci�n proporcionada por el sistema de inputs. Nos dir� si estamos pulsando uno de los botones de salto.
    public Vector2 jumpVector;
    //Booleana que indicar� si el jugador est� tocando el suelo.
    public bool isGrounded;
    //Booleana que indicar� si el jugador est� saltando.
    public bool isJumping;
    //Fuerza con la que saltar� el jugador.
    public float jumpForce;
    //Velocidad con la que el vector vertical de velocidadd ir� decreciendo al dejar de saltar.
    public float decreaseJumpSpeed;
    //Tiempo que ser� efectivo el mantener pulsado el bot�n de saltar para saltar m�s tiempo.
    public float jumpTime;
    //Contador que usaremos para saber si se ha superado el tiempo l�mite con el bot�n de salto pulsado.
    float jumpTimeCounter;

    [SerializeField] PhysicsMaterial2D slippery, friction;

    private void FixedUpdate()
    {
        Movement();
        Jump();
        CheckGround();
    }

    private void Update()
    {
        SetAnimatorYVelocity();
    }

    //Obtenemos los valores de los vectores de movimiento y de salto del sistema de inputs para comprobar si se pulsa alguno de los botones.
    //Lo hacemos de esta manera para que se pueda implementar el sistema de multijugador local.
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        jumpVector = context.ReadValue<Vector2>();
    }

    void Movement()
    {
        //Si permitimos el movimiento.
        if (canMove)
        {
            //Aplicamos la rotaci�n dependiendo de si nos movemos hacia la izquierda o la derecha.
            RotatePlayer(movement.x);

            //Igualamos la velocidad del jugador al vector de movimiento obtenido del sistema de inputs y lo multiplicamos por speed y Time.deltaTime.
            rB.velocity = new Vector2(movement.x * speed * Time.deltaTime, rB.velocity.y);

            anim.SetBool("walking", movement.x != 0);
        }
    }

    void RotatePlayer(float xVal)
    {
        if (xVal > 0.1f)
        {
            //El personaje se mueve hacia la derecha.
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);


        }
        else if (xVal < -0.1f)
        {
            //El personaje se mueve hacia la izquierda.
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
    }

    void Jump()
    {
        //Si se est� pulsando el bot�n de salto y el personaje est� en el suelo.
        if (jumpVector.y > 0 && isGrounded)
        {
            //Aplicamos la fuerza de salto en el eje Y de la velocidad.
            rB.velocity = new Vector2(rB.velocity.x, jumpVector.y * jumpForce);
            //Igualamos el contador al tiempo l�mite que el jugador puede mantener pulsado el bot�n de salto.
            jumpTimeCounter = jumpTime;
            isJumping = true;
            AudioController.instance.PlaySFX(AudioController.instance.jump);
        }
        //Si se est� pulsando el bot�n de salto y el jugador ya est� saltando.
        if (jumpVector.y > 0 && isJumping)
        {
            //Se aplicar� continuamente el salto.
            rB.velocity = new Vector2(rB.velocity.x, jumpVector.y * jumpForce);
            //Hacemos que el contador decrezca con el tiempo.
            jumpTimeCounter -= Time.deltaTime;
            //Si el contador llega a 0
            if (jumpTimeCounter <= 0)
            {
                //Hacemos que el jugador deje de impulsarse hacia arriba poco a poco, ya que s�lo con la gravedad el jugador se queda impuls�ndose hacia
                //arriba demasiado tiempo.
                rB.velocity = Vector2.MoveTowards(rB.velocity, Vector2.zero, decreaseJumpSpeed * Time.deltaTime);
                //Indicamos que se ha dejado de saltar.
                isJumping = false;
            }
        }
        //Si se ha dejado de pulsar el bot�n de salto y el personaje est� saltando.
        if (jumpVector.y <= 0 && isJumping)
        {
            //Volvemos a hacer que el jugador deje de impulsarse hacia arriba poco a poco.
            rB.velocity = Vector2.MoveTowards(rB.velocity, Vector2.zero, decreaseJumpSpeed * Time.deltaTime);
            //Indicamos que se ha dejado de saltar.
            isJumping = false;
        }
    }

    void CheckGround()
    {
        anim.SetBool("grounded", isGrounded);
        if (isGrounded)
        {
            rB.sharedMaterial = friction;
        }
        else
        {
            rB.sharedMaterial = slippery;
        }
    }

    void SetAnimatorYVelocity()
    {
        if (!isGrounded)
        {
            float yVelocity = rB.velocity.y;
            Mathf.Clamp(yVelocity, -1f, 1f);
            anim.SetFloat("yVelocity", yVelocity);
        }
    }
}
