using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(SphereCollider))] 

public class Sensor_Noireaude : MonoBehaviour 
{
	public float m_fieldGlobal             = 15f;
	private Vector3 _m_VectRepuls          = Vector3.zero;
	private Vector3 _m_VectAttract         = Vector3.zero;
	private Vector3 _m_VectOrient          = Vector3.zero;
	private float m_PourcentageOrientation = 0.66f;
	private float m_PourcentageRepulsion   = 0.33f;
	private float m_factRepuls             = 2f;


	private Dictionary<int,GameObject> _m_echoGlobal = new Dictionary<int, GameObject> ();

	public float SetFactRepuls
	{
		set
		{
			this.m_factRepuls = value;
		}
	}
	private void MInitFields ()
	{
		(collider as SphereCollider).isTrigger = true;
	}
	public void MinitVectors()
	{
		_m_VectRepuls  = Vector3.zero;
		_m_VectAttract = Vector3.zero;
		_m_VectOrient  = transform.forward;
	}

	private void MInitRigidbody ()
	{
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;
	}

	public void MUpdateVectors ()
	{
		MinitVectors ();

		foreach (KeyValuePair<int, GameObject> kvp in _m_echoGlobal)
		{
			Vector3 monVector = kvp.Value.transform.position - transform.position;
			float distance = monVector.x*monVector.x + monVector.y*monVector.y + monVector.z*monVector.z;

			float m_fieldRepulsionCARRE   = (m_fieldGlobal*m_PourcentageRepulsion)*(m_fieldGlobal*m_PourcentageRepulsion);
			float m_fieldOrientationCARRE = (m_fieldGlobal*m_PourcentageOrientation)*(m_fieldGlobal*m_PourcentageOrientation);
			float m_fieldAttractionCARRE  = m_fieldGlobal*m_fieldGlobal;


			if (distance <= m_fieldRepulsionCARRE)
			{
				Vector3 repulsion = monVector;
				float NormeCARRE = repulsion.x*repulsion.x + repulsion.y*repulsion.y + repulsion.z*repulsion.z;
				repulsion.Normalize();
				
				_m_VectRepuls += repulsion *(1-Mathf.Sqrt(NormeCARRE))*m_factRepuls;
			}

			if (distance <= m_fieldOrientationCARRE && distance > m_fieldRepulsionCARRE)
			{

				_m_VectRepuls += transform.forward;
			}
			 
			if (distance <= m_fieldAttractionCARRE && distance > m_fieldOrientationCARRE)
			{
				_m_VectAttract += (monVector*(-1f));
			}
		}

	}

	private void MUpdateFields ()
	{
		(collider as SphereCollider).radius = m_fieldGlobal;
	}


	void OnTriggerEnter(Collider _other)
	{


		_m_echoGlobal [_other.gameObject.GetInstanceID ()] = _other.gameObject;

	}

	void OnTriggerExit(Collider _other)
	{
		_m_echoGlobal.Remove (_other.gameObject.GetInstanceID ());

	}

	public void MChangePoucentages (string _field, float _value)
	{
		if (_field == "m_PourcentageOrientation")
		{
			m_PourcentageOrientation = _value;
		}

		else if (_field == "m_PourcentageRepulsion")
		{
			m_PourcentageRepulsion = _value;
		}

	}


	public Vector3 Get_VectorAttraction
	{
		get 
		{
			return _m_VectAttract;
		}
	}
	public Vector3 Get_VectorRepulsion
	{
		get 
		{
			return _m_VectRepuls;
		}
	}
	public Vector3 Get_VectorOrientation
	{
		get 
		{
			return _m_VectOrient;
		}
	}
	// Use this for initialization
	void Awake () 
	{
		MInitFields ();
		MInitRigidbody ();
	}
	
	// Update is called once per frame
	void Update () 
	{

		MUpdateFields ();
		//MUpdateVectors ();



	}

	/*
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
	


		
	}*/

}