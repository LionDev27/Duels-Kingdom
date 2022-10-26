using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    //Configuración de este jugador.
    public PlayerConfiguration pConfig;
    //Controles del jugador.
    public Controls c;

    PlayerComponents pComp;

    private void Awake()
    {
        c = new Controls();

        pComp = GetComponent<PlayerComponents>();
    }

    public void InitializeNewPlayer(PlayerConfiguration pC)
    {
        //Igualamos la configuración de este jugador a la recibida.
        pConfig = pC;
        //Cambiamos el ActionMap del input al InGamePlayer.
        pConfig.input.SwitchCurrentActionMap("InGamePlayer");
        //Suscribimos todos las acciones a los inputs.
        pConfig.input.onActionTriggered += SuscribeEventsToInput;
        //Igualamos los controles del playercomponent.
        pComp.c = c;
    }

    void SuscribeEventsToInput(InputAction.CallbackContext obj)
    {
        //Comparamos el nombre de cada acción y lo suscribimos en función a esta.
        if (obj.action.name == c.InGamePlayer.Movement.name)
        {
            pComp.pM.OnMove(obj);
        }
        if (obj.action.name == c.InGamePlayer.Jump.name)
        {
            pComp.pM.OnJump(obj);
        }
        if (obj.action.name == c.InGamePlayer.Attack.name)
        {
            pComp.pA.Attack(obj);
        }
        if (obj.action.name == c.InGamePlayer.Aim.name)
        {
            pComp.pA.GetAimValues(obj);
        }
        if (obj.action.name == c.InGamePlayer.InteractItems.name)
        {
            pComp.pI.InteractWithItem(obj);
        }
    }

    private void OnEnable()
    {
        c.Enable();
    }

    private void OnDisable()
    {
        c.Disable();
    }
}
