using UnityEngine;
using System.Collections;

public class PackMan : UnitBase 
{
	protected override void MyStart()
	{
		base.MyStart();

		_leftButton.OnPointerUpAction += ClickLeftUp;
		_leftButton.OnPointerDownAction += ClickLeftDown;

		_rightButton.OnPointerUpAction += ClickRightUp;
		_rightButton.OnPointerDownAction += ClickRightDown;

		_upButton.OnPointerUpAction += ClickUpUp;
		_upButton.OnPointerDownAction += ClickUpDown;

		_downButton.OnPointerUpAction += ClickDownUp;
		_downButton.OnPointerDownAction += ClickDownDown;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_leftButton.OnPointerUpAction -= ClickLeftUp;
		_leftButton.OnPointerDownAction -= ClickLeftDown;
		
		_rightButton.OnPointerUpAction -= ClickRightUp;
		_rightButton.OnPointerDownAction -= ClickRightDown;
		
		_upButton.OnPointerUpAction -= ClickUpUp;
		_upButton.OnPointerDownAction -= ClickUpDown;
		
		_downButton.OnPointerUpAction -= ClickDownUp;
		_downButton.OnPointerDownAction -= ClickDownDown;
	}

	protected override Transform _positionDefault
	{
		get
		{
			return rotation.transformPackMan;
		}
	}

	public override void Load(System.Action OnStop)
	{
		_left = 0;
		_right = 0;
		_up = 0;
		_down = 0;

		base.Load(OnStop);
	}

	void ClickLeftUp()
	{
		_left --;
	}

	void ClickLeftDown()
	{
		_left ++;
	}

	void ClickRightUp()
	{
		_right --;
	}

	void ClickRightDown()
	{
		_right ++;
	}

	void ClickUpUp()
	{
		_up --;
	}

	void ClickUpDown()
	{
		_up ++;
	}

	void ClickDownUp()
	{
		_down --;
	}

	void ClickDownDown()
	{
		_down ++;
	}

	bool Click(LocationRotation.Rotation click)
	{
		if (_first != null && _first.Contains(click) && _first.Same(transform, true))
		{
			direction = click;
			_second = rotation.FindIndex(click, _first, true);
			transform = _first.transform;
			return _second != null;
		}
		else if (_second != null && _second.Contains(click) && _second.Same(transform, true))
		{
			direction = click;
			_first = _second;
			transform = _first.transform;
			_second = rotation.FindIndex(click, _first, true);
			return _second != null;
		}
		else if (click == LocationRotation.GetOppositeRotation(_direction))
		{
			direction = click;
			_first = _second;
			_second = rotation.FindIndex(click, _first, true);
			return _second != null;
		}
		return false;
	}
	
	protected override void Update()
	{
		if (_canGo)
		{
			if (_left > 0 && _direction != LocationRotation.Rotation.Left && Click(LocationRotation.Rotation.Left))
			{

			}
			else if (_right > 0 && _direction != LocationRotation.Rotation.Right && Click(LocationRotation.Rotation.Right))
			{

			}
			else if (_up > 0 && _direction != LocationRotation.Rotation.Up && Click(LocationRotation.Rotation.Up))
			{

			}
			else if (_down > 0 && _direction != LocationRotation.Rotation.Down && Click(LocationRotation.Rotation.Down))
			{

			}
			else if (_second != null && _second.Same(transform, false))
			{
				_first = _second;
				_second = rotation.FindIndex(_direction, _first, true);
			}
			else
			{
				base.Update();
				GameController.i.Eat(transform);
			}
		}
	}

	public override LocationRotation.Rotation direction
	{
		protected set
		{
			Vector3 angle = Vector3.zero;
			if (value == LocationRotation.Rotation.Left)
			{
				angle = new Vector3(0, 180f, 0);
			}
			else if (value == LocationRotation.Rotation.Up)
			{
				angle = new Vector3(0, transform.eulerAngles.y, 90f);
			}
			else if (value == LocationRotation.Rotation.Down)
			{
				angle = new Vector3(0, transform.eulerAngles.y, -90f);
			}

			transform.eulerAngles = angle;

			base.direction = value;
		}
	}



	int _left;
	int _right;
	int _up;
	int _down;

	[SerializeField] ButtonOnPointerUpDown		_leftButton;
	[SerializeField] ButtonOnPointerUpDown		_rightButton;
	[SerializeField] ButtonOnPointerUpDown		_upButton;
	[SerializeField] ButtonOnPointerUpDown		_downButton;
}
