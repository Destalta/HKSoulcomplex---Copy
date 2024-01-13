using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionDetector : MonoBehaviour
{

    public bool Touching;
    public LayerMask CollisionMask;
    public bool UseCollisionMask;
    public UnityEvent TouchEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!UseCollisionMask)
        {
            TouchEvent.Invoke();
            Touching = true;
        }
        else
        {
            if (collision.gameObject.layer == CollisionMask.value)
            {
                TouchEvent.Invoke();
                Touching = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!UseCollisionMask)
        {
            Touching = false;
        }
        else
        {
            if (collision.gameObject.layer == CollisionMask.value)
            {
                Touching = false;
            }
        }
    }
}
