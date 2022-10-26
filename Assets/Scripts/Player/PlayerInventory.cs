using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerInventory : PlayerComponents
{
    [SerializeField]
    //Item que tiene actualmente el jugador.
    Item currentItem;
    //Booleana que indicar� si el jugador tiene un item o no.
    public bool hasItem;
    //Radio del �rea donde el jugador podr� interactuar con objetos.
    public float interactionRadius;
    //Layer de los items.
    public LayerMask itemLayer;
    //Item que tiene el jugador en la mano.
    [SerializeField] GameObject itemInHand;
    Vector2 itemInHandOriginalPos = new Vector2();

    //
    [SerializeField] SpriteRenderer interactPrompt;
    [SerializeField] Sprite gamepadSprite, keyboardSprite;

    //Punto desde el que se lanzar�n los items.
    public Transform throwPoint;
    //Velocidad de lanzamiento de los items.
    public float throwSpeed;

    private void Start()
    {
        itemInHandOriginalPos = itemInHand.transform.localPosition;
    }

    private void Update()
    {
        if (!hasItem)
        {
            ShowPrompt();
        }
        else if (interactPrompt.gameObject.activeInHierarchy)
        {
            interactPrompt.gameObject.SetActive(false);
        }
    }

    public void InteractWithItem(InputAction.CallbackContext obj)
    {
        //S�lo comprobamos si se ha pulsado el bot�n.
        if (obj.performed)
        {
            //Si ya tiene un item, pulsar este bot�n har� que lance el item que ya tiene.
            if (hasItem) ThrowItem();
            //Si no, pulsar este bot�n har� que el jugador recoja un item si tiene alguno a su alrededor.
            else PickItem();
        }
    }

    void ShowPrompt()
    {
        //Comprobamos si en el radio de interacci�n hay alg�n objeto con el layer Item y guardamos los que haya en un array.
        Collider2D[] nearItems = Physics2D.OverlapCircleAll(transform.position, interactionRadius, itemLayer);
        //Si hay alg�n objeto.
        if (nearItems.Length > 0)
        {
            interactPrompt.gameObject.SetActive(true);
        }
        else
        {
            interactPrompt.gameObject.SetActive(false);
        }
    }

    void PickItem()
    {
        Debug.Log("Cogiendo Item");
        //Comprobamos si en el radio de interacci�n hay alg�n objeto con el layer Item y guardamos los que haya en un array.
        Collider2D[] nearItems = Physics2D.OverlapCircleAll(transform.position, interactionRadius, itemLayer);
        //Si hay alg�n objeto.
        if (nearItems.Length > 0)
        {
            //Recorremos nuestra lista en el array de la base de datos para ver si el objeto que da este interectable existe.
            foreach (Item it in ItemDB.instance.items)
            {
                //Comparamos la ID del objeto extra�do con el foreach con la ID del primer objeto con el que hemos interactuado.
                //Con esto, s�lo cogemos el primer �tem detectado por el OverlapCircle.
                if (it.id == nearItems[0].GetComponent<PickableItem>().itemID)
                {
                    //Si ambas ids coinciden, quiere decir que se ha localizado el objeto dentro de la base de datos.
                    //Por tanto, lo a�adimos al inventario igualando el currentItem al item obtenido.
                    currentItem = it;
                    //Indicamos que el jugador tiene un objeto.
                    hasItem = true;
                    //Activamos el �tem de la mano.
                    itemInHand.SetActive(true);
                    GameObject itemSprite = nearItems[0].transform.GetChild(0).gameObject;
                    //Igualamos el sprite de los objetos y, adem�s, ajustamos la posici�n y la rotaci�n.
                    itemInHand.GetComponent<SpriteRenderer>().sprite = itemSprite.GetComponent<SpriteRenderer>().sprite;
                    itemInHand.transform.localPosition += itemSprite.transform.localPosition;
                    //Igualamos todos los par�metros guardados en el item a las variables del ataque.
                    SetAttackSettings();
                    //Destruimos el item en el mundo.
                    Destroy(nearItems[0].gameObject);
                    //
                    AudioController.instance.PlaySFX(AudioController.instance.pickItem);
                    //Salimos del m�todo.
                    return;
                }
            }
            //Si la ejecuci�n llega a este punto, quiere decir que la id indicada no existe en la base de datos de objetos.l
            Debug.LogWarning("No se ha encontrado el objeto en la base de datos. ID: " + nearItems[0].GetComponent<PickableItem>().itemID);
        }
        //Si no, no se har� nada e indicaremos en consola que no hay ning�n objeto cerca.
        else
        {
            Debug.Log("No hay objetos cerca");
        }
    }

    void ThrowItem()
    {
        Debug.Log("Lanzando Item");
        //Creamos un vector de lanzamiento. Este vector ser� el vector direcci�n desde el jugador hasta throwPoint.
        Vector2 throwVector = new Vector2(throwPoint.position.x - transform.position.x, throwPoint.position.y - transform.position.y);
        //Instanciamos el objeto que tenemos actualmente.
        GameObject throwedItem = Instantiate(currentItem.itemPrefab, throwPoint.position, Quaternion.identity);
        //Obtenemos su componente PickableItem. Indicamos que se est� lanzando el �tem y le pasamos el player que ha lanzado el objeto.
        throwedItem.GetComponent<PickableItem>().throwingItem = true;
        throwedItem.GetComponent<PickableItem>().parentPlayer = gameObject;
        //Le a�adimos velocidad al rigidbody con el vector direcci�n multiplicado por la velocidad de lanzamiento.
        throwedItem.GetComponent<Rigidbody2D>().velocity = throwVector * throwSpeed;
        RemoveCurrentItem();
        //
        AudioController.instance.PlaySFX(AudioController.instance.throwItem);
    }

    public void RemoveCurrentItem()
    {
        //Comprobamos que tengamos un objeto.
        if (currentItem != null)
        {
            ResetAttackSettings();
            //Vaciamos currentItem.
            currentItem = null;
            //Indicamos que el jugador no tiene ning�n objeto.
            hasItem = false;
            //Desactivamos el objeto en la mano y ponemos su posici�n por defecto.
            itemInHand.transform.localPosition = itemInHandOriginalPos;
            itemInHand.SetActive(false);
        }
    }

    void ResetAttackSettings()
    {
        //Si el ataque era a melee, lo reseteamos todo. No har� falta resetear las variables de ataque a distancia, ya que, cuando no tienes item,
        //tu ataque pasar� a ser a melee, y al coger otro objeto a distancia, las variables cambiar�n.
        if (currentItem.attackType == AttackTypes.MeleeAttack)
        {
            pA.meleeAttackCooldown = pA.defaultMeleeAttackCooldown;
            dD.knockbackMultiplier = 1;
        }
        pA.pAttack = AttackTypes.MeleeAttack;
        dD.isLethal = false;
    }

    void SetAttackSettings()
    {
        if (currentItem.attackType == AttackTypes.MeleeAttack)
        {
            pA.meleeAttackCooldown = currentItem.cooldown;
            dD.knockbackMultiplier = currentItem.knockbackMultiplier;
        }
        else if (currentItem.attackType == AttackTypes.RangedAttack)
        {
            pA.rangedAttackCooldown = currentItem.cooldown;
            pA.bulletPrefab = currentItem.bulletPrefab;
            pA.bulletSpeed = currentItem.bulletSpeed;
        }
        //Igualamos el tipo de ataque y si es letal o no a las variables de los scripts correspondientes.
        pA.pAttack = currentItem.attackType;
        dD.isLethal = currentItem.isLethal;
    }

    public void ChangePrompt(string deviceName)
    {
        if (deviceName == "Gamepad")
        {
            interactPrompt.sprite = gamepadSprite;
        }
        else if (deviceName == "Keyboard")
        {
            interactPrompt.sprite = keyboardSprite;
        }
    }

    private void OnDrawGizmos()
    {
        //Referencia visual del radio de interacci�n.
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
