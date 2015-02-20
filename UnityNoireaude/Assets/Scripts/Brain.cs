using UnityEngine;
using System.Collections;

public abstract class Brain : MonoBehaviour {
	
	public Sensor m_Sensors_Prox , m_Sensors_Stress , m_Sensors_MvmtHumain , m_Sensors_Son;
	public Actuator m_Actuator_Idle , m_Actuator_Fuite , m_Actuator_Fixe ;
	private Vector3 _m_Cible = new Vector3 ();	
	private Vector3 _m_Velocity = new Vector3 ();
	public float m_Speed_Max = 100f;
	public float m_Speed = m_Speed_Max;
	private float m_mcoefAIM = 1f;
	private float m_mcoefOrient = 1f;

	protected abstract Actuator _MThink();

	void Awake () 
	{
		m_Actuator_Idle.enabled = false;
		m_Actuator_Fuite.enabled = false;
		m_Actuator_Fixe.enabled = false;
	}

	void Update () 
	{
		m_Actuator_Idle.enabled = false;
		m_Actuator_Fuite.enabled = false;
		m_Actuator_Fixe.enabled = false;
		Actuator toActivate = _MThink();
		toActivate.enabled = true;
	}
}