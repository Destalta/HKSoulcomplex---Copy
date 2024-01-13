using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuttable1 : MonoBehaviour, IHitResponder
{
    public bool IsCut;
    public Sprite CutSprite;
    public GameObject ParticlePrefab;
    public bool DestroyOnCut;

    private SpriteRenderer sp;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    public void Hit(HitInstance hitInstance)
    {
        if (IsCut == false)
        {
            IsCut = true;
            Instantiate(ParticlePrefab, transform.position, Quaternion.identity);
            if (DestroyOnCut)
            {
                Destroy(gameObject);
            }
            if (CutSprite != null)
            {
                sp.sprite = CutSprite;
            }
            else
            {
                sp.enabled = false;
            }
        }
    }
}
