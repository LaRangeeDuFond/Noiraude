using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(SphereCollider))] 

public class Sensor_Noireaude : MonoBehaviour 
{
	public float m_globalRadius = 4f;

	private float m_PourcentageOrientation = 0.66f;
	private float m_PourcentageRepulsion = 0.33f;
	public List<Transform> m_Global_transform;
	public Vector3 _m_VectRepulsion;
	public Vector3 _m_VectOrientation;
	public Vector3 _m_VectAttraction;

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
		(collider as SphereCollider).radius = m_globalRadius;
	}

	private void MUpdateVectors ()
	{
		_m_VectRepulsion   = Vector3.zero;
		_m_VectOrientation = Vector3.zero;
		_m_VectAttraction  = Vector3.zero;
		foreach (Transform _transform in m_Global_transform)
		{
			Vector3 monVector = _transform.position - transform.position;
			float distance = monVector.x*monVector.x + monVector.y*monVector.y + monVector.z*monVector.z;

			float m_fieldRepulsionCARRE   = (m_globalRadius*m_PourcentageRepulsion)   * (m_globalRadius*m_PourcentageRepulsion)   ;
			float m_fieldOrientationCARRE = (m_globalRadius*m_PourcentageOrientation) * (m_globalRadius*m_PourcentageOrientation) ;
			float m_fieldAttractionCARRE  =  m_globalRadius*m_globalRadius ;


			if (distance <= m_fieldRepulsionCARRE)
			{
				_m_VectRepulsion += transform.position - _transform.position;
			}
			if (distance <= m_fieldOrientationCARRE && distance > m_fieldRepulsionCARRE)
			{

				_m_VectOrientation += _transform.forward;
			}

			if (distance <= m_fieldAttractionCARRE && distance > m_fieldOrientationCARRE)
			{
				_m_VectAttraction += monVector;
			}
		}

	}




	void OnTriggerStay(Collider _other)
	{
		m_Global_transform.Clear();
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_globalRadius);
		int i = 0;
		while (i < hitColliders.Length)
		{
			m_Global_transform.Add (hitColliders[i].transform);
			i++;
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
		MUpdateVectors ();



	}
}