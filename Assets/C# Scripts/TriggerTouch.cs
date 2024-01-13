using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerTouch : MonoBehaviour
{
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    public LayerMask layerMask;
    public float Debounce;
    private bool isDebounce;
    private bool av = false;

    private IEnumerator DebounceCoroutine()
    {
        yield return new WaitForSeconds(Debounce);
        isDebounce = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if ((layerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            av = false;
            if (!isDebounce)
            {
                isDebounce = true;
                StartCoroutine(DebounceCoroutine());

                OnEnter.Invoke();
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((layerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            av = true;
            if (!isDebounce)
            {
                OnExit.Invoke();
            }
        }
        
    }

    private void Update()
    {
        if (!isDebounce && av)
        {
            av = false;
            OnExit.Invoke();
        }
    }
}
