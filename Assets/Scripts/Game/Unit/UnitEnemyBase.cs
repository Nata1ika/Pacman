using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitEnemyBase : UnitBase
{
	public PackMan	packMan;

	public enum State
	{
		None,
		Chase, //преследование
		Scatter, //разбегание
		Frightened //страх
	};

	public class NextRotation
	{
		public LocationRotation.Rotation 	rotate; //направление
		public Vector3						pos; //предполагаемая позиция
	}

	public State 			state;

	protected override void MyStart()
	{
		base.MyStart();
		if (GameController.isInstantiated)
		{
			GameController.i.ChangeEnemyState += ChangeEnemyState;
		}
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
		if (GameController.isInstantiated)
		{
			GameController.i.ChangeEnemyState -= ChangeEnemyState;
		}
	}

	public override void Load(System.Action OnStop)
	{
		_speed = -1f;
		int currentLvl = GameController.currentLvl;
		if (GameController.i != null && GameController.i.level != null && GameController.i.level.Length > 0 && currentLvl >= 0)
		{
			currentLvl = currentLvl >= GameController.i.level.Length ? currentLvl = GameController.i.level.Length - 1 : currentLvl;
			speed = GameController.i.level[currentLvl].coefficientSpeedEnemy;
		}

		base.Load (OnStop);
	}

	protected override void Update()
	{
		if (_canGo)
		{
			if(IsClash(transform, packMan.transform))
			{
				GameController.i.EndGame();
			}
			else if (_first != null && _second == null)
			{
				_second = _first;
				transform.position = _first.transform.position;
			}
			else if (_second != null && _second.Same(transform, false))
			{
				_first = _second;
				direction = ChooseRotation();
				_second = rotation.FindIndex(_direction, _first, true);
			}
			else
			{
				base.Update();
			}
		}
	}

	protected virtual Vector3 target //целевая точка
	{
		get
		{
			return packMan.transform.position;
		}
	}

	void ChangeEnemyState(State newState)
	{
		state = newState;
	}

	//получить повторот на перекрестье
	LocationRotation.Rotation ChooseRotation()
	{
		//заполним все варианты
		List<NextRotation> next = new List<NextRotation>();
		if (_second.left && (LocationRotation.GetOppositeRotation(_direction) != LocationRotation.Rotation.Left || state == State.Frightened))
		{
			NextRotation element = new NextRotation();
			element.rotate = LocationRotation.Rotation.Left;
			element.pos = new Vector3(_second.transform.position.x - _move, _second.transform.position.y, 0);
			next.Add(element);
		}
		if (_second.right && (LocationRotation.GetOppositeRotation(_direction) != LocationRotation.Rotation.Right || state == State.Frightened))
		{
			NextRotation element = new NextRotation();
			element.rotate = LocationRotation.Rotation.Right;
			element.pos = new Vector3(_second.transform.position.x + _move, _second.transform.position.y, 0);
			next.Add(element);
		}
		if (_second.up && (LocationRotation.GetOppositeRotation(_direction) != LocationRotation.Rotation.Up || state == State.Frightened))
		{
			NextRotation element = new NextRotation();
			element.rotate = LocationRotation.Rotation.Up;
			element.pos = new Vector3(_second.transform.position.x, _second.transform.position.y + _move, 0);
			next.Add(element);
		}
		if (_second.down && (LocationRotation.GetOppositeRotation(_direction) != LocationRotation.Rotation.Down || state == State.Frightened))
		{
			NextRotation element = new NextRotation();
			element.rotate = LocationRotation.Rotation.Down;
			element.pos = new Vector3(_second.transform.position.x, _second.transform.position.y - _move, 0);
			next.Add(element);
		}


		if (state == State.Chase || state == State.Scatter) //преследование или разбегание
		{
			//находим минимальное расстояние до цели
			Vector3 pos = state == State.Chase ? target : _targetScatter;
			int index = -1;
			float minMove = -1f;
			for (int i=0; i<next.Count; i++)
			{
				float move = (pos.x - next[i].pos.x)*(pos.x - next[i].pos.x) + (pos.y - next[i].pos.y)*(pos.y - next[i].pos.y);
				if (move < minMove || minMove < 0)
				{
					minMove = move;
					index = i;
				}
			}
			return index >= 0 ? next[index].rotate : LocationRotation.Rotation.None;
		}
		else if (state == State.Frightened) //страх
		{
			int index = (next.Count > 0) ? UnityEngine.Random.Range(0, next.Count) : -1;
			return index >= 0 ? next[index].rotate : LocationRotation.Rotation.None;
		}
		else return LocationRotation.Rotation.None;
	}

	bool IsClash(Transform pos1, Transform pos2)
	{
		if (pos1.position.x - pos2.position.x < _EPS && pos1.position.x - pos2.position.x > -_EPS &&
		    pos1.position.y - pos2.position.y < _EPS && pos1.position.y - pos2.position.y > -_EPS)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public override LocationRotation.Rotation direction
	{
		protected set
		{
			base.direction = value;
			if (value == LocationRotation.Rotation.Up || value == LocationRotation.Rotation.Down)
			{
				_eyesUp.SetActive(true);
				_eyesRight.SetActive(false);
				_eyesUp.transform.eulerAngles = new Vector3(0, 0, value == LocationRotation.Rotation.Up ? 0 : 180f);
			}
			else
			{
				_eyesUp.SetActive(false);
				_eyesRight.SetActive(true);
				_eyesRight.transform.eulerAngles = new Vector3(0, 0, value == LocationRotation.Rotation.Right ? 0 : 180f);
			}
		}
	}

	protected override float speed
	{
		get
		{
			return _speed > 0 ? _speed : baseSpeed;
		}
		set
		{
			_speed = baseSpeed * value;
		}
	}

	protected virtual Vector3 _targetScatter //позиция 
	{
		get
		{
			return rotation.targetScatterRed.position;
		}
	}

	float						_speed;
	[SerializeField] GameObject _eyesUp;
	[SerializeField] GameObject _eyesRight;
	protected const float 		_move = 10f;
	const float					_EPS = 5f;
}
