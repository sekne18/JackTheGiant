using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    void OnEnable() //is called when gameObject is activated, when object is deactivated and activated again this function will be called
    {
        Invoke("DestroyCollectable",6f); //after 6sec, call Destroy...
    }

    //void OnDisable() // called when we deactivate object
    //{

    //}

    void DestroyCollectable()
    {
        gameObject.SetActive(false);
    }
    
}
