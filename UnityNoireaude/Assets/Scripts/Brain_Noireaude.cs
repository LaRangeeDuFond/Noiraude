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
	private bool m_misaAJour = false;


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
	
	public void MSetSpeed (float speed)
	{
		m_Speed = speed;
	}

	public void MSetPourcentages (string _field, float _value)
	{
		_sensor.MChangePoucentages (_field,_value);
	}

	public void MSetDictionnaryLimits (int _value)
	{
		_sensor.MChangeDictionnaryLimits (_value);
	}



	private Vector3 MComputeRepulse ()
	{
		Vector3 VecteurRepulse = Vector3.zero;
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

	private Vector3 MComputeAttract ()
	{
		Vector3 VecteurAttract = Vector3.zero;
		Dictionary<int,GameObject> _m_echoAttraction = _sensor.Get_echoAttraction;
		foreach (KeyValuePair<int, GameObject> kvp in _m_echoAttraction)
		{
			Vector3 attraction =  kvp.Value.transform.position - transform.position;
			float Norme = Vector3.Distance (kvp.Value.transform.position, transform.position)-_sensor.m_fieldOrientation;
			Norme/= _sensor.m_fieldAttraction - _sensor.m_fieldOrientation;
			attraction.Normalize();
			
			VecteurAttract += attraction *(1-Norme);
		}
		
		return VecteurAttract;
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
		Vector3 _m_Cible = McomputeAimCenterMass();
		Vector3 _m_Repulse = MComputeRepulse ();
		Vector3 _m_Attract = MComputeAttract ();
		Vector3 _m_AimCible = McomputeAimCible ();
		_m_Cible.Normalize();
		_m_Repulse.Normalize();
		_m_Attract.Normalize();
		_m_AimCible.Normalize();

		/*
		// Vers le centre de masse
		_m_Velocity += _m_Cible/100f;*/

		// repulsion
		_m_Velocity += _m_Repulse*0.02f;

		// attraction
		_m_Velocity += _m_Attract*0.002f;

		//attraction Cible
		//_m_Velocity += _m_AimCible*0.0125f;

		//vecteur forward
		_m_Velocity += transform.forward*0.0125f;

		//division du vecteur
		_m_Velocity *= 0.33f;

	}
	

	private Vector3 MComputeAverageForward ()
	{
		Vector3 VecteurRotation = Vector3.zero;
		Dictionary<int,GameObject> _m_echoOrientation = _sensor.Get_echoOrientation;
		foreach (KeyValuePair<int, GameObject> kvp in _m_echoOrientation)
		{
			Vector3 rotation = kvp.Value.transform.forward;
			VecteurRotation += rotation ;
		}
		VecteurRotation.Normalize ();
		//Debug.Log ("Normalize ou diviser");
		return VecteurRotation;
	}

	private Quaternion MAlignAim ()
	{
		Vector3 vectAim = _m_Cible - transform.position;
		Vector3 vectAxis = Vector3.Cross (transform.forward,vectAim);
		float angle = Vector3.Angle (transform.forward, vectAim);
		if (angle > 15f){angle = 15f;}
		Quaternion monQuaternion = Quaternion.AngleAxis(angle,vectAxis);
		return monQuaternion;

	}

	private Quaternion MAlignForward()
	{

		Vector3 vectAverageForward = MComputeAverageForward ();
		Vector3 vectAxis = Vector3.Cross (transform.forward,vectAverageForward);
		float angle = Vector3.Angle (transform.forward, vectAverageForward);
		if (angle > 1f){angle = 1f;}
		Quaternion monQuaternion = Quaternion.AngleAxis(angle,vectAxis);
		return monQuaternion;

	}

	private void MRotation ()
	{
		transform.rotation = MAlignAim () * transform.rotation;
	}

	void MMove()
	{
		//transform.Translate (_m_Velocity*m_Speed*Time.deltaTime);


		Vector3 nouvellePosition = new Vector3 ();
		nouvellePosition.x = transform.position.x + (_m_Velocity.x)*m_Speed*Time.deltaTime;
		nouvellePosition.y = transform.position.y + (_m_Velocity.y)*m_Speed*Time.deltaTime;
		nouvellePosition.z = transform.position.z + (_m_Velocity.z)*m_Speed*Time.deltaTime;
		
		transform.position = nouvellePosition;


		/*
		//contrainte Y
		if(transform.position.y!=0f)
		{
			Vector3 pos = transform.position;
			pos.y = 0f;
			transform.position = pos;
		}*/
	}



	public void MSet_misaAJourTrue()
	{
		m_misaAJour=true;
	}


	// Update is called once per frame
	void FixedUpdate () 
	{

		if (m_misaAJour)
		{
			MRotation ();
			MComputeVelocity ();
			m_misaAJour = false;

		}
		MMove();
	}


}
