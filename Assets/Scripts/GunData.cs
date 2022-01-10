using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunData
{
	public enum ShootMode
	{
		Semi,
		FullAuto
	}

	public ShootMode m_ShootingMode;

	public float m_FireRate;

	public Vector3 m_MuzelLocation;

	public GameObject m_Prefab;
}
