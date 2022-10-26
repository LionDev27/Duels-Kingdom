using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCameraViewController : MonoBehaviour
{
    [SerializeField] UnityEvent onBecameVisibleActions;
    [SerializeField] UnityEvent onBecameInvisibleActions;

    private void OnBecameVisible()
    {
        onBecameVisibleActions.Invoke();
    }

    private void OnBecameInvisible()
    {
        onBecameInvisibleActions.Invoke();
    }
}
