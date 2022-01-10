using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
	private static GameManeger s_Instance = null;

	public GameObject m_GroundGunPrefab;

	GameManeger()
	{
		if (s_Instance != null)
			return;

		s_Instance = this;
	}

	public static GameManeger Get()
	{
		return s_Instance;
	}

}
