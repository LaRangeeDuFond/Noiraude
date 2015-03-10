using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Sensor_Prox : Sensor 
{
	public float m_fieldAttraction = 15f;
	public float m_fieldOrientation = 10f;
	public float m_fieldRepulsion = 5f;
	
	private float m_PourcentageOrientation = 0.66f;
	private float m_PourcentageRepulsion = 0.33f;
	
	private int _m_DictionnaryLimits = 50;
	private int compteurDico = 0;
	
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

	void Awake () 
	{
		MInitFields ();
		MInitRigidbody ();
	}

	
	private void MUpdateDictionnary ()
	{
		_m_echoRepulsion.Clear();
		_m_echoOrientation.Clear();
		_m_echoAttraction.Clear();
		compteurDico = 0;
		
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
	
	private void MUpdateFields ()
	{
		(collider as SphereCollider).radius = m_fieldAttraction;
		m_fieldOrientation = m_fieldAttraction*m_PourcentageOrientation;
		m_fieldRepulsion = m_fieldAttraction*m_PourcentageRepulsion;
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
	
	public void MChangeDictionnaryLimits (int _value)
	{
		_m_DictionnaryLimits = _value;
	}
	
	
	public Dictionary<int,GameObject> Get_echoAttraction
	{
		get 
		{
			return _m_echoAttraction;
		}
	}
	public Dictionary<int,GameObject> Get_echoOrientation
	{
		get 
		{
			return _m_echoOrientation;
		}
	}
	public Dictionary<int,GameObject> Get_echoRepulsion
	{
		get 
		{
			return _m_echoRepulsion;
		}
	}
	void Update () 
	{
		
		MUpdateFields ();
		MUpdateDictionnary ();		
	}
}
