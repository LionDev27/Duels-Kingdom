using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerMenu : MonoBehaviour
{
    //Prefab del menú de jugador.
    public GameObject playerMenuPrefab;
    public PlayerInput input;

    private void Awake()
    {
        //Buscamos el objeto PlayerMenus en la escena. Esto lo hacemos porque este script lo va a tener un prefab y no podemos asignarle una referencia.
        GameObject menuParent = GameObject.Find("PlayerMenus");
        //Lo instanciamos.
        GameObject tempMenu = Instantiate(playerMenuPrefab, menuParent.transform);
        //Asiganmos en UIInputModule del PlayerInput al del menu.
        input.uiInputModule = tempMenu.GetComponentInChildren<InputSystemUIInputModule>();
        //Asignamos el índice del jugador.
        tempMenu.GetComponent<PlayerMenuController>().SetPlayerInput(input);
    }
}
