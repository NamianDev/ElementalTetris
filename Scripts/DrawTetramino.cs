using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTetramino : MonoBehaviour
{
    public ResourseManager RM;
    void Awake()
    {
        RM = FindObjectOfType<ResourseManager>();


        if (RM.NextTetraminoElement == 0)
        {
            RM.NextTetraminoElement = 1;
        }
        GetComponent<Animator>().SetInteger("NextElementBlock", RM.NextTetraminoElement);
        if (this.gameObject.GetComponent<TetrisBlock>())
        {
            RM.CurentBlockElement = RM.NextTetraminoElement;
            RM.NextTetraminoElement = Random.Range(1, 7);
        }
        
    }
}
