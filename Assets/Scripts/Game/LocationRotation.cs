using UnityEngine;
using System;
using System.Collections;

public class LocationRotation : BaseList
{
	public Transform		transformPackMan;
	public Transform		transformRed;
	public Transform		transformBlue;
	public Transform		transformPink;
	public Transform		transformOrange;

	public Transform		targetScatterRed;
	public Transform		targetScatterBlue;
	public Transform		targetScatterPink;
	public Transform		targetScatterOrange;

	public enum Rotation
	{
		Left,
		Right,
		Up,
		Down,
		None
	};

	public void Load(Action RotationLoad)
	{
		_elementsHorizontal = gameObject.GetComponentsInChildren<BaseElement>();

		if (_elementsHorizontal != null)
		{
			_elementsVertical = Copy(_elementsHorizontal);
			Sort(true, _elementsHorizontal);
			Sort(false, _elementsVertical);

			SetIndex();

			if (RotationLoad != null)
			{
				RotationLoad();
			}
		}
	}

	public static Rotation GetOppositeRotation(Rotation rotate)
	{
		if (rotate == Rotation.Left)
		{
			return Rotation.Right;
		}
		else if (rotate == Rotation.Right)
		{
			return Rotation.Left;
		}
		else if (rotate == Rotation.Up)
		{
			return Rotation.Down;
		}
		else if (rotate == Rotation.Down)
		{
			return Rotation.Up;
		}
		else
		{
			return Rotation.None;
		}
	}

	public LocationRotationElement FindIndex(Rotation rotate, LocationRotationElement current, bool usePassive)
	{
		if (! usePassive && current.passive)
		{
			return null;
		}

		if (current.indexHorizontal >= 0 && current.indexHorizontal < _elementsHorizontal.Length)
		{
			if (rotate == Rotation.Left && current.left && current.indexHorizontal - 1 >= 0 && 
			    current.SameHorizontal(_elementsHorizontal[current.indexHorizontal - 1].transform))
			{
				if (! usePassive && _elementsHorizontal[current.indexHorizontal - 1].passive)
				{
					return FindIndex(rotate, _elementsHorizontal[current.indexHorizontal - 1] as LocationRotationElement, true);
				}
				return _elementsHorizontal[current.indexHorizontal - 1] as LocationRotationElement;
			}
			else if (rotate == Rotation.Right && current.right && current.indexHorizontal + 1 < _elementsHorizontal.Length &&
			         current.SameHorizontal(_elementsHorizontal[current.indexHorizontal + 1].transform))
			{
				if (! usePassive && _elementsHorizontal[current.indexHorizontal + 1].passive)
				{
					return FindIndex(rotate, _elementsHorizontal[current.indexHorizontal + 1] as LocationRotationElement, true);
				}
				return _elementsHorizontal[current.indexHorizontal + 1] as LocationRotationElement;
			}
		}

		if (current.indexVertical >= 0 && current.indexVertical < _elementsVertical.Length)
		{
			if (rotate == Rotation.Down && current.down && current.indexVertical - 1 >= 0 &&
			    current.SameVertical(_elementsVertical[current.indexVertical - 1].transform))
			{
				if (! usePassive && _elementsVertical[current.indexVertical - 1].passive)
				{
					return FindIndex(rotate, _elementsVertical[current.indexVertical - 1] as LocationRotationElement, true);
				}
				return _elementsVertical[current.indexVertical - 1] as LocationRotationElement;
			}
			else if (rotate == Rotation.Up && current.up && current.indexVertical + 1 < _elementsVertical.Length &&
				current.SameVertical(_elementsVertical[current.indexVertical + 1].transform))
			{
				if (! usePassive && _elementsVertical[current.indexVertical + 1].passive)
				{
					return FindIndex(rotate, _elementsVertical[current.indexVertical + 1] as LocationRotationElement, true);
				}
				return _elementsVertical[current.indexVertical + 1] as LocationRotationElement;
			}
		}
		return null;
	}

	public LocationRotationElement GetElementByPos(float x, float y)
	{
		return _elementsHorizontal[0] as LocationRotationElement;
	}
}
