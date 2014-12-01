using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Groupe_Noireaude : MonoBehaviour {


	public Vector3 _m_CenterOfMassGlobal = new Vector3 ();
	private int _m_NbNoireaudes = 100;




	private List<Brain_Noireaude> _m_ListBrains = new List<Brain_Noireaude> ();
	private List<Transform> _m_ListTransform = new List<Transform> ();


	public GameObject m_PrefabNoireaude;
	public GameObject m_cible;

	public float m_Speed = 100f;






	// Use this for initialization
	void Awake ()
	{
		for(int i = 0; i<_m_NbNoireaudes ; i++)
		{
			GameObject maNoireaude = Instantiate (m_PrefabNoireaude, new Vector3(5*i,0,0),Quaternion.identity) as GameObject;
			_m_ListBrains.Add(maNoireaude.GetComponent<Brain_Noireaude>());
			_m_ListTransform.Add(maNoireaude.transform);
		}
	
	}


	void MComputeCenterOfMass ()
	{
		if (_m_ListTransform.Count > 0)
		{

			_m_CenterOfMassGlobal = Vector3.zero;

			foreach (Transform monTransform in _m_ListTransform)
			{
				_m_CenterOfMassGlobal += monTransform.position;
			}


			_m_CenterOfMassGlobal /= _m_ListTransform.Count;


		}

		else
		{
			_m_CenterOfMassGlobal = Vector3.zero;
			Debug.Log ("pas de noireaudes");
		}

	}

	void MSendCenterOfMass()
	{
		foreach (Brain_Noireaude monScript in _m_ListBrains)
		{
			monScript.MSetCenterOfMass (_m_CenterOfMassGlobal);
		}
	}

	void MSendCible()
	{
		foreach (Brain_Noireaude monScript in _m_ListBrains)
		{
			monScript.MSetCible (m_cible.transform.position);
		}
	}

	void MSendSpeed ()
	{
		foreach (Brain_Noireaude monScript in _m_ListBrains)
		{
			monScript.MSetSpeed (m_Speed);
		}
	}





	// Update is called once per frame
	void Update () 
	{
		MComputeCenterOfMass ();
		MSendCenterOfMass();
		MSendCible();
		MSendSpeed ();
		transform.position = _m_CenterOfMassGlobal;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, new Vector3(3f,3f,3f));

	}

}
