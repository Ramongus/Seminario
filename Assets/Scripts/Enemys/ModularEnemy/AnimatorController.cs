using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public Animator GetAnimator()
	{
		return animator;
	}


}
