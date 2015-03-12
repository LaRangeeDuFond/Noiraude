using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Sensor_Noireaude))]

public class Brain_Noireaude : MonoBehaviour 
{
	public Sensor_Noireaude _sensor;
	public Actuator_Noireaude_Idle _actuator;
	private Vector3 _m_CenterOfMass = new Vector3 ();
	public Transform _m_Cible ;

	
	public void MSetcoefAIM (float value)
	{
		_actuator.m_mcoefAIM = value;
	}
	public void MSetcoefOrient (float value)
	{
		_actuator.m_mcoefOrient = value;
	}
	public void MSetCenterOfMass (Vector3 Objectif)
	{
		_m_CenterOfMass = Objectif;
	}
	public void MSetCible (Transform _cible)
	{
		_m_Cible = _cible;
		_actuator.MSetCible (_m_Cible);
	}
	
	public void MSetSpeed (float _speed)
	{
		_actuator.m_Speed = _speed;
	}

	public void MSetPourcentages (string _field, float _value)
	{
		_sensor.MChangePoucentages (_field,_value);
	}


}
