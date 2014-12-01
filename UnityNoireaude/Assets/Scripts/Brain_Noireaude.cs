using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Sensor_Noireaude))]

public class Brain_Noireaude : MonoBehaviour {


	public Sensor_Noireaude _sensor;

	private Vector3 _m_CenterOfMass = new Vector3 ();
	private Vector3 _m_Cible = new Vector3 ();

	private Vector3 _m_Velocity = new Vector3 ();

	public float m_Speed = 100f;


	void MinitSensor()
	{
		_sensor = GetComponent<Sensor_Noireaude>();
	}



	void Awake () 
	{
		MinitSensor();

	}



	public void MSetCenterOfMass (Vector3 Objectif)
	{
		_m_CenterOfMass = Objectif;
	}

	public void MSetCible (Vector3 Cible)
	{
		_m_Cible = Cible;
	}

	private Vector3 MComputeRepulse ()
	{
		Vector3 VecteurRepulse = new Vector3 ();
		Dictionary<int,GameObject> _m_echoRepulsion = _sensor.Get_echoRepulsion;
		foreach (KeyValuePair<int, GameObject> kvp in _m_echoRepulsion)
		{
			Vector3 repulsion = transform.position - kvp.Value.transform.position;
			float Norme = Vector3.Distance (kvp.Value.transform.position, transform.position);
			Norme/= _sensor.m_fieldRepulsion;
			repulsion.Normalize();

			VecteurRepulse += repulsion *(1-Norme);
		}

		return VecteurRepulse;
	}

	private Vector3 McomputeAimCenterMass ()
	{
		Vector3 AimCenterMass = _m_CenterOfMass - transform.position;
		AimCenterMass.Normalize();

		return AimCenterMass;

	}

	private Vector3 McomputeAimCible ()
	{
		Vector3 AimCible = _m_Cible - transform.position;
		AimCible.Normalize();
		return AimCible;
	}


	private void MComputeVelocity ()
	{
		_m_Velocity = Vector3.zero;


		// Vers le centre de masse
		_m_Velocity += McomputeAimCenterMass()/100f;

		// repulsiion
		_m_Velocity += MComputeRepulse ()/50f;

		_m_Velocity += McomputeAimCible ()/50f;

	}


	public void MSetSpeed (float speed)
	{
		m_Speed = speed;
	}


	void MMove()
	{
		transform.Translate (_m_Velocity*m_Speed*Time.deltaTime);
	}


	// Update is called once per frame
	void FixedUpdate () 
	{
		MComputeVelocity ();
		MMove();
	}
}
