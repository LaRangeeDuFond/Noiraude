using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sensor_Prox_2D : Sensor 
{
	public float m_GeneralRadius = 15f;
    public GameObject MasterGO;
    public Master ScriptMaster;
    private List<Transform> _m_ListTransform;
	private float m_PourcentageOrientation = 0.66f;
	private float m_PourcentageRepulsion = 0.33f;	
	private int _m_DictionnaryLimits = 50;
	private int compteurDico = 0;


    private Vector3 _m_VectAttraction, _m_VectOrientation , _m_VectRepulsion;
    
    void Awake()
    {
        ScriptMaster = MasterGO.GetComponent<Master>();
    }
	public void MChangePoucentages (string _field, float _value)
	{
		if (_field == "m_PourcentageOrientation")
		{
			m_PourcentageOrientation = _value;
		}
		else if (_field == "m_PourcentageRepulsion")
		{
			m_PourcentageRepulsion = _value;
		}
	}

    public void MGetTransformList()
    {
        _m_ListTransform.Clear();
        _m_ListTransform = new List<Transform>(ScriptMaster.MSendTransform());
    }
	void Update () 
	{

	}
}
