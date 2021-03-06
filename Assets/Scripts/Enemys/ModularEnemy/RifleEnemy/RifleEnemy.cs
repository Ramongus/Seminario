﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleEnemy : MonoBehaviour
{
	[SerializeField] Transform lineCastPoint;
	[SerializeField] LineRenderer linePrefab;
	[SerializeField] float attackRange;
	LineRenderer currentLineRender;
	Transform target;
	bool isAttacking;
	[SerializeField] float damage;
	HealthSystem healthSystem;

	private void Awake()
	{
		target = FindObjectOfType<Player>().transform;
		healthSystem = GetComponent<HealthSystem>();
		healthSystem.OnHealthChange += HealthSystem_OnHealthChange;
	}

	private void HealthSystem_OnHealthChange(object sender, System.EventArgs e)
	{
		if(healthSystem.GetHealth() <= 0)
		{
			Destroy(currentLineRender);
		}
	}

	private void Update()
	{
		if (!isAttacking)
		{
			Vector3 toTarget = target.position - transform.position;
			Vector3 toTargetIgnoringHeight = new Vector3(toTarget.x, transform.position.y, toTarget.z);
			transform.forward = toTargetIgnoringHeight.normalized;

			Vector3 toTargetFromLineCast = target.position - lineCastPoint.position;
			Vector3 toTargetFromLineCastDir = toTargetFromLineCast.normalized;

			if(currentLineRender != null)
			{
				SetLinePosition(lineCastPoint.position);
				SetLineVertexPosition(0, Vector3.zero);
				SetLineVertexPosition(1, new Vector3(toTargetFromLineCastDir.x, 0, toTargetFromLineCastDir.z).normalized * attackRange);
			}
		}
	}

	public void SetLineRenderer(LineRenderer line)
	{
		currentLineRender = line;
		currentLineRender.transform.position = lineCastPoint.position;
	}

	public LineRenderer GetLineRenderer()
	{
		return currentLineRender;
	}

	public LineRenderer GetLineRendererPrefab()
	{
		return linePrefab;
	}

	public void SetLinePosition(Vector3 position)
	{
		currentLineRender.transform.position = position;
	}

	public void SetLineVertexPosition(int index, Vector3 position)
	{
		currentLineRender.SetPosition(index, position);
	}

	public void SetOpacityToMaterial(float op)
	{
		Color lineColor = new Color(1, 0, 0, op);
		currentLineRender.startColor = lineColor;
		currentLineRender.endColor = lineColor;
	}

	public Color GetLineColor()
	{
		return currentLineRender.startColor;
	}

	public void SetLineColor(Color color)
	{
		currentLineRender.startColor = color;
		currentLineRender.endColor = color;
	}

	public void SetIsAttacking(bool attacking)
	{
		isAttacking = attacking;
	}

	public float GetAttackRange()
	{
		return attackRange;
	}

	public float GetDamage()
	{
		return damage;
	}
}
