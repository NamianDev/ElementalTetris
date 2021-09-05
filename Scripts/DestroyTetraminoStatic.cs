using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTetraminoStatic : MonoBehaviour
{
    public void DestroyThisObj()
    {
        Destroy(this.gameObject); 
    }
}
