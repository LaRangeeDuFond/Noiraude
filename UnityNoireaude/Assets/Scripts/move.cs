using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {

	public Transform aim1,aim2;

	void Awake ()
	{
		transform.position= aim2.position;
	}
	// Update is called once per frame
	void Update () 
	{
		if(transform.position == aim1.position)
		{
			while(transform.position!=aim2.position)
			{
				transform.Translate(Vector3.MoveTowards (transform.position,aim2.position,0.5f));
			}
		}
		if(transform.position == aim2.position)
		{
			while(transform.position!=aim1.position)
			{
				transform.Translate(Vector3.MoveTowards (transform.position,aim1.position,0.5f));
			}
		}
	
	}
}
