﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Groupe_Noireaude : MonoBehaviour {


	public Vector3 _m_CenterOfMassGlobal = new Vector3 ();
	public int _m_NbNoireaudes = 2;




	private List<Brain_Noireaude> _m_ListBrains = new List<Brain_Noireaude> ();
	private List<Transform> _m_ListTransform = new List<Transform> ();


	public GameObject m_PrefabNoireaude;
	public GameObject m_cible;
	public int m_nbMAJ_group = 3;
	private int m_MAJ_compteur = 1;
	public float m_Speed = 100f;

	public int m_DictionnaryLimits = 50;
	private int _m_previousDictionnaryLimits = 0;

	public float m_PoucentageOrientation = 0.66f;
	public float m_PoucentageRepulsion = 0.33f;
	private float m_PreviousPoucentageOrientation;
	private float m_PreviousPoucentageRepulsion;






	// Use this for initialization
	void Awake ()
	{
		for(int i = 0; i<_m_NbNoireaudes ; i++)
		{
			GameObject maNoireaude = Instantiate (m_PrefabNoireaude, new Vector3(i*3f,0,0),Quaternion.identity) as GameObject;
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





	void MVerify_Send_Pourcentages ()
	{
		if (m_PreviousPoucentageOrientation != m_PoucentageOrientation)
		{
			MSendPourcentages ("m_PourcentageOrientation",m_PoucentageOrientation);
			m_PreviousPoucentageOrientation = m_PoucentageOrientation;
		}

		if (m_PreviousPoucentageRepulsion != m_PoucentageRepulsion)
		{
			MSendPourcentages ("m_PourcentageRepulsion",m_PoucentageRepulsion);
			m_PreviousPoucentageRepulsion = m_PoucentageRepulsion;
		}

	}

	void MSendPourcentages (string _field, float _value)
	{
		foreach (Brain_Noireaude monScript in _m_ListBrains)
		{
			monScript.MSetPourcentages (_field, _value);
		}
		
	}

	void MVerify_Send_DicoLimit ()
	{
		if (_m_previousDictionnaryLimits != m_DictionnaryLimits)
		{
			MSendDicoLimit (m_DictionnaryLimits);
			_m_previousDictionnaryLimits = m_DictionnaryLimits;
		}
		
	}

	void MSendDicoLimit (int _value)
	{
		foreach (Brain_Noireaude monScript in _m_ListBrains)
		{
			monScript.MSetDictionnaryLimits (_value);
		}
		
	}
	
	void MMAJ_noiraude(int group, int nb_group)
	{

		for (int i = group-1 ; i < _m_ListBrains.Count ; i += nb_group)
		{

			_m_ListBrains[i].MSetSpeed (m_Speed);
			_m_ListBrains[i].MSetCible (m_cible.transform.position);
			//_m_ListBrains[i].MSetCenterOfMass (_m_CenterOfMassGlobal);

			_m_ListBrains[i].MSet_misaAJourTrue();

		}
	}
	// Update is called once per frame
	void Update () 
	{
		//MComputeCenterOfMass ();

		MVerify_Send_Pourcentages ();
		MVerify_Send_DicoLimit ();


		MMAJ_noiraude(m_MAJ_compteur,m_nbMAJ_group);
		m_MAJ_compteur=(m_MAJ_compteur==m_nbMAJ_group)?1:m_MAJ_compteur+1;


		transform.position = _m_CenterOfMassGlobal;

	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, new Vector3(3f,3f,3f));

	}

}
