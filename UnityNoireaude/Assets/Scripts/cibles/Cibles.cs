using UnityEngine;
using System.Collections;

public class Cibles : MonoBehaviour 
{
	public float _randX,_randY = 0f;

	// Use this for initialization
	void m_changeRand () 
	{
		_randX = Random.Range(-40f,40f);
		_randY = Random.Range(-40f,40f);
	
	}
	void Start() 
	{
		InvokeRepeating("m_changeRand", 3f, 3F);
	}

	
	// Update is called once per frame
	void Update () 
	{
		transform.position = Vector3.Lerp(transform.position,new Vector3(_randX,0f,_randY),0.01f);
	
	}
}
