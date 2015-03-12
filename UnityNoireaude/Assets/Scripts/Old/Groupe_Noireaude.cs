using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Groupe_Noireaude : MonoBehaviour {


	public Vector3 _m_CenterOfMassGlobal = new Vector3 ();
	public int _m_NbNoireaudes = 10;




	private List<Brain_Noireaude> _m_ListBrains = new List<Brain_Noireaude> ();


	public GameObject m_PrefabNoireaude;
	public Transform m_cible;
	public float m_Speed = 100f;
	public float m_PreviousSpeed=0f;
	public float m_PoucentageOrientation = 0.66f;
	public float m_PoucentageRepulsion = 0.33f;
	private float m_PreviousPoucentageOrientation=0f;
	private float m_PreviousPoucentageRepulsion=0f;
	public float m_mcoefAIM = 1f;
	public float m_mcoefOrient = 1f;
	private float _mpreviouscoefAIM = 1f;
	private float _mpreviouscoefOrient = 1f;

	//private List<Thread> m_TreadListe = new List<Thread> ();
	//private int compteurThread = 0;


	// Use this for initialization
	void Awake ()
	{
		for(int i = 0; i<_m_NbNoireaudes ; i++)
		{
			GameObject maNoireaude = Instantiate (m_PrefabNoireaude, new Vector3(i*3f,0f,0f),Quaternion.identity) as GameObject;
			_m_ListBrains.Add(maNoireaude.GetComponent<Brain_Noireaude>());
		}
		foreach (Brain_Noireaude monScript in _m_ListBrains) 
		{
			monScript.MSetCible(m_cible);
		}
	}
	void Start ()
	{
		MinitPrevious ();
	}
	void MinitPrevious ()
	{
		m_PreviousSpeed = m_Speed;
		m_PreviousPoucentageOrientation = m_PoucentageOrientation;
		m_PreviousPoucentageRepulsion = m_PoucentageRepulsion;

	}
	void MUpdatecoefAim()
	{
		if (m_mcoefAIM != _mpreviouscoefAIM)
		{
			foreach (Brain_Noireaude monScript in _m_ListBrains)
			{
				monScript.MSetcoefAIM (m_mcoefAIM);
			}
			_mpreviouscoefAIM=m_mcoefAIM;				
		}

	}
	void MUpdatecoefOrient()
	{
		if (m_mcoefOrient != _mpreviouscoefOrient)
		{
			foreach (Brain_Noireaude monScript in _m_ListBrains)
			{
				monScript.MSetcoefOrient (m_mcoefOrient);
			}
			_mpreviouscoefOrient=m_mcoefOrient;			
		}
		
	}

	void MComputeCenterOfMass ()
	{/*
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
		}*/

	}





	void MVerify_Send_Pourcentages ()
	{
		if (m_PreviousSpeed != m_Speed)
		{
			foreach (Brain_Noireaude monScript in _m_ListBrains) 
			{
				monScript.MSetSpeed(m_Speed);
			}
			m_PreviousSpeed = m_Speed;
		}
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

	// Update is called once per frame
	void Update () 
	{
		//MComputeCenterOfMass ();
		MUpdatecoefOrient ();
		MUpdatecoefAim ();
		MVerify_Send_Pourcentages ();


		//transform.position = _m_CenterOfMassGlobal;

	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, new Vector3(3f,3f,3f));

	}

}
