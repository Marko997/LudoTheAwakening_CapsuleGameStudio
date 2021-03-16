using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
	public Spell fireBlast;
	public int level = 1;
	public int exp;
	private void Start()
	{
		//fireBlast = new Spell("Fire blast", 1, 27, 350);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			fireBlast.Cast();
			//exp += fireBlast.expGained;
		}
	}
}
