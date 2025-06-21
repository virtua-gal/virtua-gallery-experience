using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifetime);
    }

}
