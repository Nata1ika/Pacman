using UnityEngine;
using System.Collections;

public class EnemyPink : UnitEnemyBase
{
	protected override Vector3 target //целевая точка
	{
		get
		{
			Vector3 addVector = Vector3.zero;
			if (packMan.direction == LocationRotation.Rotation.Left)
			{
				addVector.x -= _move * 4;
			}
			else if (packMan.direction == LocationRotation.Rotation.Right)
			{
				addVector.x += _move * 4;
			}
			else if (packMan.direction == LocationRotation.Rotation.Down)
			{
				addVector.y -= _move * 4;
			}
			else if (packMan.direction == LocationRotation.Rotation.Up)
			{
				addVector.y += _move * 4;
			}
			return packMan.transform.position + addVector;
		}
	}

	protected override Transform _positionDefault
	{
		get
		{
			return rotation.transformPink;
		}
	}

	protected override Vector3 _targetScatter
	{
		get
		{
			return rotation.targetScatterPink.position;
		}
	}
}
