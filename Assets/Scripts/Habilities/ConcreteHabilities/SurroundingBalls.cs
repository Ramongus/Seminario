using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingBalls : AbstractAbilities
{
	[SerializeField] int ballsCount;
	[SerializeField] BallSurrounding ballPrefab;

	public override void SetInitiation(Vector3 castPos, Vector3 playerPos)
	{
		Player player = FindObjectOfType<Player>();
		float circleAngle = 90f / ballsCount;
		for (int i = 0; i < ballsCount; i++)
		{
			BallSurrounding ball = Instantiate(ballPrefab);
			ball.owner = player.transform;
			ball.SetInitialCircleAngle(i * circleAngle);
			if (i % 2 == 0) ball.SetInitialUpDownAngle(45f / 2f);
			else ball.SetInitialUpDownAngle(0);
		}
	}
}
