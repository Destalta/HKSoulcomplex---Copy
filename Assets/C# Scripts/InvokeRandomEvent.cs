using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeRandomEvent : MonoBehaviour
{
    public UnityEvent[] Events;

    public void InvokeRandom()
    {
        Events[Random.Range(0, Events.Length)].Invoke();
    }
}
