using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D col) {
        switch(col.gameObject.tag){
            case "Food":
                gameManager.Eat();
            break;

            case "Tail":
                gameManager.setHiscore();
            break;

            
        }
    }
}
