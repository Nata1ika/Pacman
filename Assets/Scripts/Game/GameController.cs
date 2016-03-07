using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameController : SingletonMonoBehaviour<GameController>  
{
	[Serializable] public class Level
	{
		public float 					coefficientSpeedEnemy;
		public StateEnemy[] 			statesEnemy;
	}

	[Serializable] public class StateEnemy
	{
		public UnitEnemyBase.State 		state;
		public float 					time;
	}

	public Action<bool>					PauseGame;
	public Action<UnitEnemyBase.State>	ChangeEnemyState;
	public Level[] 						level; //таблица уровней

	public static int 					currentLvl
	{
		get
		{
			if (PlayerPrefs.HasKey(_lvlTag) && useSave)
			{
				return PlayerPrefs.GetInt(_lvlTag);
			}
			else
			{
				return 0; 
			}
		}
		set
		{
			PlayerPrefs.SetInt(_lvlTag, value);
		}
	}

	public static bool useSave
	{
		get
		{
			if (PlayerPrefs.HasKey(_useSaveTag) && PlayerPrefs.GetInt(_useSaveTag) > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		set
		{
			PlayerPrefs.SetInt(_useSaveTag, value ? 1 : -1);
		}
	}

	void Start()
	{
		_wait.SetActive(true);
		_rotation.Load(LocationLoad);
	}

	void LocationLoad()
	{
		CreateEmenyStates();
		if (! useSave)
		{
			ScoreController.NewGame();
		}
		_eat.Load(EatLoad);
	}

	void EatLoad()
	{
		int index = 0;
		for (int i=0; i<_unit.Length; i++)
		{
			_unit[i].Load
			(
				()=>
				{
					index ++;
					if (index == _unit.Length)
					{
						UnitLoad();
					}
				}
			);
		}
	}

	void CreateEmenyStates()
	{
		Level curr = level[currentLvl];
		_listEnemyState = new List<StateEnemy>();
		for (int i=0; i<curr.statesEnemy.Length; i++)
		{
			_listEnemyState.Add(curr.statesEnemy[i]);
		}
	}

	void UnitLoad()
	{
		_wait.SetActive(false);
		_enemyChangeState = true;
		_currentState = UnitEnemyBase.State.None;
	}

	public void Eat(Transform packman)
	{
		_eat.ProcessEat(packman);
	}

	public void SetPauseGame(bool pause)
	{
		_enemyChangeState = ! pause;
		if (PauseGame != null)
		{
			PauseGame(pause);
		}
	}

	public void NewGame()
	{
		useSave = false;
		_wait.SetActive(true);
		_currentState = UnitEnemyBase.State.None;
		LocationLoad();
		SetPauseGame(false);
		_enemyChangeState = true;
	}

	public void AddEnemyFrightened(float timeFrightened)
	{
		StateEnemy newState = new StateEnemy();
		newState.time = timeFrightened;
		newState.state = UnitEnemyBase.State.Frightened;
		_listEnemyState.Insert(0, newState); 
	}

	public void EndGame()
	{

	}

	void Update()
	{
		if (_enemyChangeState)
		{
			if (_listEnemyState == null || _listEnemyState.Count == 0)
			{
				if(_currentState != UnitEnemyBase.State.Chase)
				{
					_currentState = UnitEnemyBase.State.Chase;
					if (ChangeEnemyState != null)
					{
						ChangeEnemyState(UnitEnemyBase.State.Chase);
					}
				}
			}
			else
			{
				if (_currentState != _listEnemyState[0].state)
				{
					_currentState = _listEnemyState[0].state;
					if (ChangeEnemyState != null)
					{
						ChangeEnemyState(_listEnemyState[0].state);
					}
				}
				_listEnemyState[0].time -= Time.deltaTime;
				if (_listEnemyState[0].time <= 0)
				{
					_listEnemyState.Remove(_listEnemyState[0]);
				}
			}
		}
	}

	[SerializeField] LocationRotation	_rotation;
	[SerializeField] Eat				_eat;
	[SerializeField] UnitBase[]			_unit;
	[SerializeField] GameObject			_wait;
	bool								_enemyChangeState = false;
	UnitEnemyBase.State					_currentState;		
	List<StateEnemy>					_listEnemyState;
	const string						_lvlTag = "level";
	const string						_useSaveTag = "useSave";
}
