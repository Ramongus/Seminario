using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCopyCamaraRot : MonoBehaviour
{
	private void Start()
	{
		transform.forward = Camera.main.transform.forward;
	}
}
