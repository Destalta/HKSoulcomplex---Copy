using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerEvent : MonoBehaviour
{
    public float Duration;

    public UnityEvent Event;

    private void Start()
    {

    }

    private IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(Duration);
        Event.Invoke();
    }

    public void Play()
    {
        StartCoroutine(Coroutine());
    }

}
