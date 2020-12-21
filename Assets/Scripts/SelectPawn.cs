using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPawn : MonoBehaviour
{
    [SerializeField]
    GameObject[] pawns;

    int pawnIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ChangePawn(){
        for(int i=0; i < pawns.Length; i++){
            
            if( i == pawnIndex ){
            
                pawns[pawnIndex].gameObject.SetActive(true);
                
            }else{

                pawns[i].gameObject.SetActive(false);

            }
        }
    }

    public void SelectBlueSanta(){
        pawnIndex = 1;
        ChangePawn();
    }
    public void SelectRedSanta(){
        pawnIndex = 0;
        ChangePawn();
    }
}
