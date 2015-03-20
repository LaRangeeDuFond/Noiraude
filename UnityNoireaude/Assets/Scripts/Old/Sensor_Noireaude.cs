using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(SphereCollider))] 

public class Sensor_Noireaude : MonoBehaviour 
{
	public float m_fieldGlobal             = 10f;
    public float m_power                   = 3f;
	private Vector3 _m_VectRepuls          = Vector3.zero;
    private float _m_RepulsFact            = 1f;
    private float _m_AttractFact           = 1f;
	private Vector3 _m_VectAttract         = Vector3.zero;
	private Vector3 _m_VectOrient          = Vector3.zero;
	private float m_PourcentageOrientation = 0.66f;
	private float m_PourcentageRepulsion   = 0.33f;
    private float _m_FieldOfView           = 300f;


	private Dictionary<int,GameObject> _m_echoGlobal = new Dictionary<int, GameObject> ();

    public float GetAttractFact
	{
		get
		{
            return _m_AttractFact;
		}
	}
	public float GetRepulsFact
	{
		get
		{
            return _m_RepulsFact;
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
        _m_RepulsFact = .1f;
        _m_AttractFact = .1f;

		foreach (KeyValuePair<int, GameObject> kvp in _m_echoGlobal)
		{
			float distance           = Vector3.Distance(kvp.Value.transform.position,transform.position);
			float m_fieldRepulsion   = (m_fieldGlobal*m_PourcentageRepulsion);
			float m_fieldOrientation = (m_fieldGlobal*m_PourcentageOrientation);
			float m_fieldAttraction  = m_fieldGlobal;

            if ((int)(Vector3.Angle(-transform.forward, (transform.position - kvp.Value.transform.position))) < (360 - _m_FieldOfView)*0.5f)
            {
                if (distance <= m_fieldRepulsion)
                {
                    float factor = (m_fieldRepulsion - distance) * m_power;
                    _m_RepulsFact += factor;
                    _m_VectRepuls += ((transform.position - kvp.Value.transform.position).normalized) * factor;
                }

                if (distance <= m_fieldOrientation && distance > m_fieldRepulsion)
                {

                    _m_VectOrient += transform.forward;
                }

                if (distance <= m_fieldAttraction && distance > m_fieldOrientation)
                {
                    float factor = (distance - m_fieldAttraction) * m_power;
                    _m_AttractFact += factor;
                    _m_VectAttract += ((kvp.Value.transform.position - transform.position).normalized) * factor;
                }
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

}