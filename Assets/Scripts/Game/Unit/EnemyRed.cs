using UnityEngine;
using System.Collections;

public class EnemyRed : UnitEnemyBase
{
	protected override Vector3 target //целевая точка
	{
		get
		{
			return packMan.transform.position;
		}
	}

	protected override Transform _positionDefault
	{
		get
		{
			return rotation.transformRed;
		}
	}

	protected override Vector3 _targetScatter
	{
		get
		{
			return rotation.targetScatterRed.position;
		}
	}
}
