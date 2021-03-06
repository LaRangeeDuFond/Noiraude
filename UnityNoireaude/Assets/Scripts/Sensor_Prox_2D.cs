﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sensor_Prox_2D : Sensor 
{
	public float m_GeneralRadius = 5f;
    public GameObject MasterGO;
    public Master ScriptMaster;
    public List<Transform> _m_ListTransform;
	private float m_PourcentageOrientation = 0.66f;
	private float m_PourcentageRepulsion = 0.33f;


    private Vector3 _m_VectAttraction, _m_VectOrientation , _m_VectRepulsion;
    
    void Start()
    {
        ScriptMaster = MasterGO.GetComponent<Master>();
    }
	public void SetMasterGO(GameObject master)
	{
		MasterGO = master;
	}
    public Vector3 GetVectAttraction
    {
        get
        {
            return _m_VectAttraction;
        }
    }
    public Vector3 GetVectOrientation
    {
        get
        {
            return _m_VectOrientation;
        }
    }
    public Vector3 GetVectRepulsion
    {
        get
        {
            return _m_VectRepulsion;
        }
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
        _m_ListTransform = new List<Transform>(ScriptMaster._m_ListTransform);
    }
	public void MInitVectors()
	{
		_m_VectAttraction  = Vector3.zero;
		_m_VectOrientation = transform.forward;
		_m_VectRepulsion   = Vector3.zero;
	}
	public void MComputeVectors()
	{
		float attract,orient,repuls;
		attract  = m_GeneralRadius;
		attract *= attract;
		orient   = m_GeneralRadius*m_PourcentageOrientation;
		orient  *= orient;
		repuls   = m_GeneralRadius*m_PourcentageRepulsion;
		repuls  *= repuls;
		foreach(Transform otherTransform in _m_ListTransform)
		{
			//On va chercher la distance au carre de de chaques boid au game object concerne et la comparer
			//au rayon au carre.
			//  SQR((otherTransform.position.x - transform.position.x)²+(otherTransform.position.z - transform.position.z)²) < r
			float dist;
			dist = (otherTransform.position.x - transform.position.x)*(otherTransform.position.x - transform.position.x)+(otherTransform.position.z - transform.position.z)*(otherTransform.position.z - transform.position.z)+(otherTransform.position.y - transform.position.y)*(otherTransform.position.y - transform.position.y);
			if(dist <= attract)
			{
				if(dist > orient)
				{
					_m_VectAttraction.x += (otherTransform.position.x - transform.position.x);
					_m_VectAttraction.z += (otherTransform.position.z - transform.position.z);
				}
				if(dist <= orient && dist > repuls)
				{
					_m_VectOrientation = _m_VectOrientation + otherTransform.forward;
				}
				if(dist <= repuls)
				{
					_m_VectAttraction.x += (transform.position.x - otherTransform.position.x);
					_m_VectAttraction.z += (transform.position.z - otherTransform.position.z);
				}
			}
		}
	}

	void Update () 
	{
		MGetTransformList();
		MInitVectors();
		MComputeVectors();
	}
}
