using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPawn : MonoBehaviour
{
    [SerializeField]
    GameObject[] pawns;
    public bool pressed;
    AllPawnsTemplates template;


	public void ChangePawn(int pawnIndex){
        for(int i=0; i < pawns.Length; i++){

            if( i == pawnIndex ){
            
                pawns[pawnIndex].gameObject.SetActive(true);

                
            }else{

                pawns[i].gameObject.SetActive(false);

            }
        }
    }

    public void SelectBlueSanta(){
        //pawnIndex = 1;
        ChangePawn(1);

    }
    public void SelectRedSanta(){
        //pawnIndex = 0;
        ChangePawn(0);
    }
}
