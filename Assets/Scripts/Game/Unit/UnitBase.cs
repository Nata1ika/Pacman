using UnityEngine;
using System.Collections;

public class UnitBase : MonoBehaviour 
{
	public string				posx;//данные для сохранения положения
	public string				posy;
	public float 				baseSpeed;
	public LocationRotation		rotation;

	public Transform transform
	{
		get
		{
			if (_transform == null)
			{
				_transform = gameObject.transform;
			}
			return _transform;
		}
		set
		{
			transform.position = value.position;
		}
	}

	protected virtual void MyStart()
	{
		_wasStart = true;
	}

	protected virtual void OnDestroy()
	{
		if (GameController.isInstantiated)
		{
			GameController.i.PauseGame -= SetPauseGame;
		}
	}

	protected virtual void Update()
	{
		if (_canGo && _second != null)
		{
			float x = transform.localPosition.x;
			float y = transform.localPosition.y;
			if (_direction == LocationRotation.Rotation.Left)
			{
				x -= speed;
			}
			else if (_direction == LocationRotation.Rotation.Right)
			{
				x += speed;
			}
			else if (_direction == LocationRotation.Rotation.Down)
			{
				y -= speed;
			}
			else if (_direction == LocationRotation.Rotation.Up)
			{
				y += speed;
			}
			
			transform.localPosition = new Vector3(x,y,0);

			PlayerPrefs.SetFloat(posx, x);
			PlayerPrefs.SetFloat(posy, y);
		}
	}

	protected virtual float speed
	{
		get
		{
			return baseSpeed;
		}
		set
		{

		}
	}

	public virtual void Load(System.Action OnStop)
	{
		if (! _wasStart)
		{
			MyStart();
		}
		GameController.i.PauseGame += SetPauseGame;
		direction = LocationRotation.Rotation.None;
		if (GameController.useSave && PlayerPrefs.HasKey(posx) && PlayerPrefs.HasKey(posy))
		{
			transform.position = new Vector3(PlayerPrefs.GetFloat(posx), PlayerPrefs.GetFloat(posy), 0);
			_first = rotation.GetElementByPos(transform.position.x, transform.position.y);
			_second = null;

			if (_first != null)
			{
				if (OnStop != null)
				{
					OnStop();
				}
				return;
			}
		}

		transform.position = _positionDefault.position;
		_first = _positionDefault.gameObject.GetComponent<LocationRotationElement>();
		_second = null;

		if (OnStop != null)
		{
			OnStop();
		}
	}

	protected virtual Transform _positionDefault
	{
		get
		{
			return rotation.elementsHorizontal[0].transform;
		}
	}

	void SetPauseGame(bool pause)
	{
		_canGo = ! pause;
	}

	public virtual LocationRotation.Rotation direction
	{
		get
		{
			return _direction;
		}
		protected set
		{
			_direction = value;
		}
	}

	Transform							_transform;
	protected LocationRotationElement	_first;
	protected LocationRotationElement	_second;
	protected LocationRotation.Rotation _direction;
	protected bool						_canGo = true;
	bool								_wasStart = false;
}
