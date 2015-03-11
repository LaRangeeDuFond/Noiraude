using UnityEngine;
using System.Collections;

public class Actuator_Transform : Actuator 
{
    public Transform _m_Cible;    
    private Vector3 _m_Velocity;
    private float m_Speed;
    private Sensor_Prox_2D m_Prox_Sensor;
    private float m_mcoefAIM = 1f;
    private float m_mcoefOrient = 1f;

    public void MSetcoefAIM(float value)
    {
        m_mcoefAIM = value;
    }
    public void MSetcoefOrient(float value)
    {
        m_mcoefOrient = value;
    }
    public void MSetCible(Transform Cible)
    {
        _m_Cible = Cible;
    }

    public void MSetSpeed(float speed)
    {
        m_Speed = speed;
    }
	public void Awake()
	{
		m_Prox_Sensor = GetComponent<Sensor_Prox_2D> ();
	}
    
    void MMove()
    {
        Vector3 nouvellePosition = Vector3.zero;
        nouvellePosition.x = transform.position.x + (_m_Velocity.x) * m_Speed * Time.deltaTime;
        nouvellePosition.y = transform.position.y + (_m_Velocity.y) * m_Speed * Time.deltaTime;
        nouvellePosition.z = transform.position.z + (_m_Velocity.z) * m_Speed * Time.deltaTime;

        transform.position = nouvellePosition;
    }
    private Quaternion MAlignAll()
    {
        Vector3 vectAverageForward = m_Prox_Sensor.GetVectOrientation;
        Vector3 vectAim = _m_Cible.position - transform.position;
        Vector3 allrotation =  (vectAim * m_mcoefAIM);
		float dist;
		dist = (_m_Cible.position.x - transform.position.x)*(_m_Cible.position.x - transform.position.x)+(_m_Cible.position.z - transform.position.z)*(_m_Cible.position.z - transform.position.z)+(_m_Cible.position.x - transform.position.x)+(_m_Cible.position.y - transform.position.y)*(_m_Cible.position.y - transform.position.y);
		if (dist > 10f) 
		{
			allrotation += (vectAverageForward * m_mcoefOrient);
		}
        Vector3 vectAxis = Vector3.Cross(transform.forward, allrotation);
        float angle = Vector3.Angle(transform.forward, vectAim);
        if (angle > 15f) { angle = 15f; }
        Quaternion monQuaternion = Quaternion.AngleAxis(angle, vectAxis);
        return monQuaternion;
    }
    private void MRotation()
    {
        transform.rotation = MAlignAll() * transform.rotation;
    }
    private void MComputeVelocity()
    {
        _m_Velocity = Vector3.zero;
        Vector3 _m_Repulse = m_Prox_Sensor.GetVectRepulsion;
        Vector3 _m_Attract = m_Prox_Sensor.GetVectAttraction;
        (transform.position - _m_Cible.position).Normalize();
        _m_Repulse.Normalize();
        _m_Attract.Normalize();
        _m_Velocity += _m_Repulse;
        _m_Velocity += _m_Attract;
		_m_Velocity += transform.forward;

    }
    void Update()
    {
		MComputeVelocity();
		MRotation();
		float dist;
		dist = (_m_Cible.position.x - transform.position.x)*(_m_Cible.position.x - transform.position.x)+(_m_Cible.position.z - transform.position.z)*(_m_Cible.position.z - transform.position.z)+(_m_Cible.position.x - transform.position.x)+(_m_Cible.position.y - transform.position.y)*(_m_Cible.position.y - transform.position.y);
		if (dist > 1f) 
		{
			MMove();
		}
        
    }
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, 5f*0.33f);
		
	}
}
