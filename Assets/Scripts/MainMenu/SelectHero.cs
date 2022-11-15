using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHero : MonoBehaviour
{
    [SerializeField] GameObject[] heroes;

    public void ChangePawn(int pawnIndex)
    {
        for (int i = 0; i < heroes.Length; i++)
        {

            if (i == pawnIndex)
            {
                heroes[pawnIndex].gameObject.SetActive(true);
            }
            else
            {
                heroes[i].gameObject.SetActive(false);
            }
        }
    }
}
