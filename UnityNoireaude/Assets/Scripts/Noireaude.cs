using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(SphereCollider))] 

public class Noireaude : MonoBehaviour 
{
	public float m_fieldAttraction = 15f;
	public float m_fieldOrientation = 10f;
	public float m_fieldRepulsion = 5f;

	private Dictionary<int,GameObject> _m_echoGlobal = new Dictionary<int, GameObject> ();
	private Dictionary<int,GameObject> _m_echoAttraction = new Dictionary<int, GameObject> ();
	private Dictionary<int,GameObject> _m_echoOrientation = new Dictionary<int, GameObject> ();
	private Dictionary<int,GameObject> _m_echoRepulsion = new Dictionary<int, GameObject> ();

	private void MInitFields ()
	{
		(collider as SphereCollider).isTrigger = true;
	}

	private void MInitRigidbody ()
	{
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;
	}

	private void MUpdateDictionnary ()
	{
		_m_echoRepulsion.Clear();
		_m_echoOrientation.Clear();
		_m_echoAttraction.Clear();

		foreach (KeyValuePair<int, GameObject> kvp in _m_echoGlobal)
		{
			float distance = Vector3.Distance(kvp.Value.transform.position, transform.position);

			if (distance <= m_fieldRepulsion)
			{
				_m_echoRepulsion [kvp.Key] = kvp.Value;
			}

			if (distance <= m_fieldOrientation && distance > m_fieldRepulsion)
			{
				_m_echoOrientation [kvp.Key] = kvp.Value;
			}

			if (distance <= m_fieldAttraction && distance > m_fieldOrientation)
			{
				_m_echoAttraction [kvp.Key] = kvp.Value;
			}
		}

	}

	private void MUpdateFields ()
	{
		(collider as SphereCollider).radius = m_fieldAttraction;
		m_fieldOrientation = m_fieldAttraction*0.66f;
		m_fieldRepulsion = m_fieldAttraction*0.33f;
	}



	void OnTriggerEnter(Collider _other)
	{
		_m_echoGlobal [_other.gameObject.GetInstanceID ()] = _other.gameObject;
	}

	void OnTriggerExit(Collider _other)
	{
		_m_echoGlobal.Remove (_other.gameObject.GetInstanceID ());
	}









	// Use this for initialization
	void Awake () 
	{
		MInitFields ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		MUpdateFields ();
		MUpdateDictionnary ();
	}


	public void OnDrawGizmos()
	{

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, m_fieldAttraction);

		foreach (KeyValuePair<int, GameObject> kvp in _m_echoAttraction) 
		{
			Gizmos.DrawWireSphere (kvp.Value.transform.position, 1f);
		}

		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere (transform.position, m_fieldOrientation);

		foreach (KeyValuePair<int, GameObject> kvp in _m_echoOrientation) 
		{
			Gizmos.DrawWireSphere (kvp.Value.transform.position, 1f);
		}

		Gizmos.color = Color.gray;
		Gizmos.DrawWireSphere (transform.position, m_fieldRepulsion);

		foreach (KeyValuePair<int, GameObject> kvp in _m_echoRepulsion) 
		{
			Gizmos.DrawWireSphere (kvp.Value.transform.position, 1f);
		}



		
	}

}
