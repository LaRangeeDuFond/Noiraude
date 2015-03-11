using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(SphereCollider))] 

public class Sensor_Noireaude : MonoBehaviour 
{
	public float m_fieldAttraction = 15f;
	public float m_fieldOrientation = 10f;
	public float m_fieldRepulsion = 5f;

	private float m_PourcentageOrientation = 0.66f;
	private float m_PourcentageRepulsion = 0.33f;
	public List<Transform> m_Global_transform;

	private void MInitFields ()
	{
		(collider as SphereCollider).isTrigger = true;
	}

	private void MInitRigidbody ()
	{
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;
	}

	private void MUpdateFields ()
	{
		(collider as SphereCollider).radius = m_fieldAttraction;
		m_fieldOrientation = m_fieldAttraction*m_PourcentageOrientation;
		m_fieldRepulsion = m_fieldAttraction*m_PourcentageRepulsion;
	}

	private void MUpdateDictionnary ()
	{
		foreach (KeyValuePair<int, GameObject> kvp in _m_echoGlobal)
		{
			Vector3 monVector = kvp.Value.transform.position - transform.position;
			float distance = monVector.x*monVector.x + monVector.y*monVector.y + monVector.z*monVector.z;

			float m_fieldRepulsionCARRE = m_fieldRepulsion*m_fieldRepulsion;
			float m_fieldOrientationCARRE = m_fieldOrientation*m_fieldOrientation;
			float m_fieldAttractionCARRE = m_fieldAttraction*m_fieldAttraction;


			if (distance <= m_fieldRepulsionCARRE)
			{
				_m_echoRepulsion [kvp.Key] = kvp.Value;
				compteurDico += 1;
			}

			if (compteurDico<=_m_DictionnaryLimits)
			{

				if (distance <= m_fieldOrientationCARRE && distance > m_fieldRepulsionCARRE)
				{

					_m_echoOrientation [kvp.Key] = kvp.Value;
					compteurDico += 1;
				}

				if (distance <= m_fieldAttractionCARRE && distance > m_fieldOrientationCARRE)
				{
					_m_echoAttraction [kvp.Key] = kvp.Value;
					compteurDico += 1;
				}
			}
		}

	}




	void OnTriggerStay(Collider _other)
	{
		m_Global_transform.Clear();
		foreach (Transform _transform in _other) 
		{
			m_Global_transform.Add (_transform);
		}

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
		MUpdateDictionnary ();



	}
}