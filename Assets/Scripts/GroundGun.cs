using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGun : MonoBehaviour
{
	public GunData m_Data;

	public bool m_DestroyOnPickup = true;

	void Start()
	{
		PlaceGunModel();
	}

	private void OnValidate()
	{
		//PlaceGunModel();	
	}

	void PlaceGunModel()
	{
		foreach (Transform c in transform)
			Destroy(c.gameObject);

		GameObject modle = Instantiate(m_Data.m_Prefab, transform);
	}
}
