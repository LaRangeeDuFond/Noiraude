using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class Master : MonoBehaviour {


	public Vector3 _m_CenterOfMassGlobal = new Vector3 ();
	public int _m_NbNoireaudes = 2;



	private List<Sensor_Prox_2D> _m_Sensor2D = new List<Sensor_Prox_2D>();
    private List<Actuator_Transform> _m_Atransform = new List<Actuator_Transform>();
	public List<Transform> _m_ListTransform = new List<Transform> ();


	public GameObject m_PrefabNoireaude;
	public GameObject m_cible;
	public int m_nbMAJ_group = 3;
	private int m_MAJ_compteur = 1;
	public float m_Speed = 100f;
	private int _m_previousDictionnaryLimits = 0;

	public float m_PoucentageOrientation = 0.66f;
	public float m_PoucentageRepulsion = 0.33f;
	private float m_PreviousPoucentageOrientation;
	private float m_PreviousPoucentageRepulsion;
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
			_m_Sensor2D.Add(maNoireaude.GetComponent<Sensor_Prox_2D>());
			_m_Atransform.Add(maNoireaude.GetComponent<Actuator_Transform>());
			_m_ListTransform.Add(maNoireaude.transform);
		}
		foreach (Sensor_Prox_2D sensorProx in _m_Sensor2D)
		{
			sensorProx.SetMasterGO(gameObject);
		}

	
	}
	void MUpdatecoefAim()
	{
		if (m_mcoefAIM != _mpreviouscoefAIM)
		{
            foreach (Actuator_Transform monScript in _m_Atransform)
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
            foreach (Actuator_Transform monScript in _m_Atransform)
			{
				monScript.MSetcoefOrient (m_mcoefOrient);
			}
			_mpreviouscoefOrient=m_mcoefOrient;			
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
    void MVerify_Send_Pourcentages()
    {
        if (m_PreviousPoucentageOrientation != m_PoucentageOrientation)
        {
            MSendPourcentages("m_PourcentageOrientation", m_PoucentageOrientation);
            m_PreviousPoucentageOrientation = m_PoucentageOrientation;
        }

        if (m_PreviousPoucentageRepulsion != m_PoucentageRepulsion)
        {
            MSendPourcentages("m_PourcentageRepulsion", m_PoucentageRepulsion);
            m_PreviousPoucentageRepulsion = m_PoucentageRepulsion;
        }

    }
	void MSendPourcentages (string _field, float _value)
	{
		foreach (Sensor_Prox_2D monScript in _m_Sensor2D)
		{
			monScript.MChangePoucentages (_field, _value);
		}
		
	}
	void MMAJ_noiraude(int group, int nb_group)
	{
        for (int i = group - 1; i < _m_Atransform.Count; i += nb_group)
		{
			MMAJ_UneNoireaude(i);
			
		}
	}

	void MMAJ_UneNoireaude(int i)
	{

        _m_Atransform[i].MSetSpeed(m_Speed);
        _m_Atransform[i].MSetCible(m_cible.transform);


	}


	// Update is called once per frame
	void Update () 
	{
		//MComputeCenterOfMass ();
		MUpdatecoefOrient ();
		MUpdatecoefAim ();
		MVerify_Send_Pourcentages ();
		MMAJ_noiraude(m_MAJ_compteur,m_nbMAJ_group);
		m_MAJ_compteur=(m_MAJ_compteur==m_nbMAJ_group)?1:m_MAJ_compteur+1;


		//transform.position = _m_CenterOfMassGlobal;

	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, new Vector3(3f,3f,3f));

	}

}
