using UnityEngine;
using System.Collections;

public class Actuator_Noireaude_Idle : MonoBehaviour 
{
	public float m_Speed = 100f;
	private Sensor_Noireaude m_sensor;
	private Vector3 _m_Velocity = new Vector3 ();
	public Transform _m_Cible;
	
	public float m_mcoefAIM = 1f;
	public float m_mcoefOrient = 1f;



	void MinitSensor()
	{
		m_sensor = GetComponent<Sensor_Noireaude>();
	}
	void Awake () 
	{
		MinitSensor();
	}
	public Vector3 MComputeRepulse()
	{
		return m_sensor._m_VectRepulsion;
	}
	public Vector3 MComputeAttract()
	{
		return m_sensor._m_VectAttraction;
	}
	public void MSetCible(Transform _cible)
	{
		_m_Cible = _cible;
	}
	public Vector3 MComputeOrient()
	{
		return m_sensor._m_VectOrientation;
	}

	private Vector3 ComputeAIM ()
	{
		return transform.position - _m_Cible.position;
	}
	
	private void MComputeVelocity ()
	{
		_m_Velocity = Vector3.zero;

		Vector3 _m_Repulse = MComputeRepulse ();
		Vector3 _m_Attract = MComputeAttract ();
		_m_Repulse.Normalize();
		_m_Attract.Normalize();
		//_m_AimCible.Normalize();
		//Vector3 _m_Cible = McomputeAimCenterMass();
		//Vector3 _m_AimCible = McomputeAimCible ();

		// Vers le centre de masse
		//_m_Velocity += _m_Cible.position100f;
		
		// repulsion
		_m_Velocity += _m_Repulse*0.01f;
		
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
		VecteurRotation = m_sensor._m_VectOrientation;
		VecteurRotation.Normalize ();
		//Debug.Log ("Normalize ou diviser");
		return VecteurRotation;
	}
	
	private Quaternion MAlignAll ()
	{
		Vector3 vectAverageForward = MComputeAverageForward ();
		Vector3 vectAim = _m_Cible.position - transform.position;
		Vector3 allrotation = (vectAverageForward*m_mcoefOrient) + (vectAim*m_mcoefAIM);
		Vector3 vectAxis = Vector3.Cross (transform.forward,allrotation);
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
		if (angle > 10f){angle = 10f;}
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
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		MRotation ();
		MComputeVelocity ();
		MMove();
	}
}
