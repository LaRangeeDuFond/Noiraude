using UnityEngine;
using System.Collections;

public class ScriptTest : MonoBehaviour {


	public GameObject m_cible;
	public float m_speed = 1f;

	private Quaternion MAlignAim ()
	{
		Vector3 vectAim = m_cible.transform.position - transform.position;
		Vector3 vectAxis = Vector3.Cross (transform.forward,vectAim);

		float angle = Vector3.Angle (transform.forward, vectAim);

		if (angle > 5f){angle = 5f;}

		Quaternion monQuaternion = Quaternion.AngleAxis(angle,vectAxis);
		return monQuaternion;
		
	}

	private void MRotation ()
	{
		transform.rotation = MAlignAim () * transform.rotation;
	}

	// Update is called once per frame
	void Update () 
	{
	
		MRotation ();
		Debug.Log (transform.forward);

		Vector3 nouvellePosition = new Vector3 ();
		nouvellePosition.x = transform.position.x + (transform.forward.x)*m_speed*Time.deltaTime;
		nouvellePosition.y = transform.position.y + (transform.forward.y)*m_speed*Time.deltaTime;
		nouvellePosition.z = transform.position.z + (transform.forward.z)*m_speed*Time.deltaTime;

		transform.position = nouvellePosition;

		//transform.Translate (MonVecteurDeMerde*m_speed*Time.deltaTime);

	}

	void OnDrawGizmos() {

		Gizmos.color = Color.yellow;



		Gizmos.DrawLine(Vector3.zero, transform.forward);

	}

}
