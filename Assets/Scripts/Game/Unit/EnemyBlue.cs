using UnityEngine;
using System.Collections;

public class EnemyBlue : UnitEnemyBase
{
	protected override Vector3 target //целевая точка
	{
		get
		{
			return 2 * (packMan.transform.position - _redEnemy.position) + _redEnemy.position;
		}
	}

	protected override Transform _positionDefault
	{
		get
		{
			return rotation.transformBlue;
		}
	}

	protected override Vector3 _targetScatter
	{
		get
		{
			return rotation.targetScatterBlue.position;
		}
	}

	[SerializeField] Transform		_redEnemy;
}
