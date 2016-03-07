using UnityEngine;
using System.Collections;

public class EnemyOrange : UnitEnemyBase
{
	protected override Vector3 target //целевая точка
	{
		get
		{
			float deltaX = transform.position.x - packMan.transform.position.x;
			float deltaY = transform.position.y - packMan.transform.position.y;

			deltaX *= deltaX;
			deltaY *= deltaY;

			if (deltaX + deltaY >= _move * _move * 9f)
			{
				return packMan.transform.position;
			}
			else
			{
				return rotation.targetScatterOrange.position;
			}
		}
	}

	protected override Transform _positionDefault
	{
		get
		{
			return rotation.transformOrange;
		}
	}

	protected override Vector3 _targetScatter
	{
		get
		{
			return rotation.targetScatterOrange.position;
		}
	}
}
