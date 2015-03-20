﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Sensor_Noireaude))]

public class Brain_Noireaude : MonoBehaviour {

	private bool activ3D = true;


	public Sensor_Noireaude _sensor;

    private Vector3 _m_CenterOfMass = Vector3.zero;
    private Vector3 _m_Cible = Vector3.zero;

	private Vector3 _m_Velocity = Vector3.zero;

	public float m_Speed        = 1f;
	private bool m_misaAJour    = false;
	private float m_mcoefAIM    = 1f;
	private float m_mcoefOrient = 1f;


	void MinitSensor()
	{
		_sensor = GetComponent<Sensor_Noireaude>();
	}

	void Awake () 
	{
		MinitSensor();

	}
	public void MSetactiv3D (bool value)
	{
		activ3D = value;
	}
	public void MSetcoefAIM (float value)
	{
		m_mcoefAIM = value;
	}
	public void MSetcoefOrient (float value)
	{
		m_mcoefOrient = value;
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


	/*
	private Vector3 MComputeRepulse ()
	{
		Vector3 VecteurRepulse = Vector3.zero;
		Dictionary<int,GameObject> _m_echoRepulsion = _sensor.Get_echoRepulsion;
		foreach (KeyValuePair<int, GameObject> kvp in _m_echoRepulsion)
		{
			Vector3 repulsion = transform.position - kvp.Value.transform.position;
			float Norme = repulsion.x*repulsion.x + repulsion.y*repulsion.y + repulsion.z*repulsion.z;
			Norme/= _sensor.m_fieldRepulsion*_sensor.m_fieldRepulsion;
			//repulsion.Normalize();

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

	}*/

	private Vector3 McomputeAimCible ()
	{
		Vector3 AimCible = _m_Cible - transform.position;
		AimCible.Normalize();
		return AimCible;
	}

	private void MComputeVelocity ()
	{
		_m_Velocity = Vector3.zero;
		//Vector3 _m_Cible = McomputeAimCenterMass();
		Vector3 _m_Repulse = (_sensor.Get_VectorRepulsion)/(_sensor.GetRepulsFact);
        Vector3 _m_Attract = (_sensor.Get_VectorAttraction) / (_sensor.GetAttractFact);

		//Vector3 _m_AimCible = McomputeAimCible ();
		//_m_Cible.Normalize();
		//_m_Attract.Normalize();
		//_m_AimCible.Normalize();

		/*
		// Vers le centre de masse
		_m_Velocity += _m_Cible/100f;*/

		// repulsion
		_m_Velocity += _m_Repulse;

		// attraction
		//_m_Velocity += _m_Attract;

		//attraction Cible
		//_m_Velocity += _m_AimCible*0.0125f;

		//vecteur forward

		_m_Velocity += transform.forward;

		//division du vecteur
		//_m_Velocity *= 0.33f;
        //_m_Velocity *= 0.5f;
	}
	
	/*
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
	}*/

	private Quaternion MAlignAll ()
	{
		Vector3 vectAverageForward = _sensor.Get_VectorOrientation;
		Vector3 vectAim = _m_Cible - transform.position;
		Vector3 allrotation = (vectAverageForward*m_mcoefOrient) + (vectAim*m_mcoefAIM);
		Vector3 vectAxis = Vector3.Cross (transform.forward,allrotation);
		float angle = Vector3.Angle (transform.forward, vectAim);
		angle = (angle > 15f) ? 15f : angle;
		Quaternion monQuaternion = Quaternion.AngleAxis(angle,vectAxis);
		return monQuaternion;

	}

	private Quaternion MAlignForward()
	{

		Vector3 vectAverageForward = _sensor.Get_VectorOrientation;
		Vector3 vectAxis = Vector3.Cross (transform.forward,vectAverageForward);
		float angle = Vector3.Angle (transform.forward, vectAverageForward);
		//angle = (angle > 10f) ? 10f : angle;
		Quaternion monQuaternion = Quaternion.AngleAxis(angle,vectAxis);
		return monQuaternion;

	}

	private void MRotation ()
	{
		transform.rotation = MAlignAll () * transform.rotation;
	}

	void MMove()
	{
		//transform.Translate (_m_Velocity*m_Speed*Time.deltaTime);


		Vector3 nouvellePosition = new Vector3 ();
		nouvellePosition.x = transform.position.x + (_m_Velocity.x)*m_Speed*Time.deltaTime;
		nouvellePosition.y = transform.position.y + (_m_Velocity.y)*m_Speed*Time.deltaTime;
		nouvellePosition.z = transform.position.z + (_m_Velocity.z)*m_Speed*Time.deltaTime;

		transform.position = nouvellePosition;
        //transform.position = transform.position + _m_Velocity * m_Speed * Time.deltaTime;
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
			_sensor.MUpdateVectors ();
			MRotation ();
			MComputeVelocity ();
			m_misaAJour = false;

		}
		MMove();
	}


}
