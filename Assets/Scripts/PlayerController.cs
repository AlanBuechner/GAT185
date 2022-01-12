using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
	[Header("Look")]
	[SerializeField]
	private float m_Sensitivity = 500.0f;
	[SerializeField]
	private float m_MinPitch = -85.0f, m_MaxPitch = 85.0f;
	private float m_CameraPitch = 0.0f;

	[Header("Movment")]
	[SerializeField]
	private float m_RunSpeed = 10.0f;
	[SerializeField]
	private float m_WalkSpeed = 5.0f;
	[SerializeField]
	private float m_JumpHeight = 1.0f;

	[Header("Physics")]
	[SerializeField]
	private float m_Gravity = -9.81f;
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Shooting")]
	[SerializeField]
	private Transform m_GunTransform;
	private CharacterController m_CharacterController;
	private GunData m_CurrentGun;

	private Camera m_Camera;

	void Start()
	{
		m_Camera = GetComponentInChildren<Camera>();
		m_CharacterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		Look();
		Move();

		#region Interation

		if (Input.GetKeyDown(KeyCode.Q))
			DropGun(m_Camera.transform.position + transform.forward);

		if(Input.GetKeyDown(KeyCode.E))
		{
			RaycastHit hit;
			if(Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out hit, 3.0f, ~(1 << 3)))
			{
				GroundGun gun;
				if((gun = hit.transform.GetComponent<GroundGun>()) != null)
				{
					DropGun(hit.point);
					SetGun(gun.m_Data);
					if (gun.m_DestroyOnPickup)
						Destroy(hit.transform.gameObject);
				}
			}
		}

		#endregion

	}


	private void Look()
	{
		Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * m_Sensitivity * Time.deltaTime;
		transform.Rotate(new Vector3(0, mouseDelta.x, 0), Space.Self);
		m_CameraPitch -= mouseDelta.y;
		m_CameraPitch = Mathf.Clamp(m_CameraPitch, m_MinPitch, m_MaxPitch);
		Vector3 camRot = m_Camera.transform.rotation.eulerAngles;
		camRot.x = m_CameraPitch;
		m_Camera.transform.rotation = Quaternion.Euler(camRot);
	}

	private void Move()
	{
		bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		float speed = (isRunning) ? m_RunSpeed : m_WalkSpeed;
		Vector3 dir = Vector3.zero;
		dir += transform.forward * Input.GetAxis("Vertical");
		dir += transform.right * Input.GetAxis("Horizontal");
		if (dir.sqrMagnitude > 1) dir.Normalize();

		m_Velocity.x = dir.x * speed;
		m_Velocity.z = dir.z * speed;

		if (m_CharacterController.isGrounded)
		{
			m_Velocity.y = -0.5f; // small force so the character controller will ditect is grounded
			if (Input.GetKeyDown(KeyCode.Space))
				m_Velocity.y = Mathf.Sqrt(-2 * m_Gravity * m_JumpHeight);
		}
		else
			m_Velocity.y += m_Gravity * Time.deltaTime;

		m_CharacterController.Move(m_Velocity * Time.deltaTime);
	}

	private void SetGun(GunData data)
	{
		// remove all chiled objects of the gun transform
		foreach (Transform child in m_GunTransform)
			Destroy(child.gameObject);

		// create the gun modle in the world
		Instantiate(data.m_Prefab, m_GunTransform);
		
		// set the current gun
		m_CurrentGun = data;
	}

	private void DropGun(Vector3 dropPoint)
	{
		if (m_CurrentGun != null)
		{
			GameObject groundGun = Instantiate(GameManeger.Get().m_GroundGunPrefab, dropPoint, transform.rotation);
			groundGun.GetComponent<GroundGun>().m_Data = m_CurrentGun;
		}

		// remove all chiled objects of the gun transform
		foreach (Transform child in m_GunTransform)
			Destroy(child.gameObject);

		m_CurrentGun = null;
	}
}
