using UnityEngine;
using System.Collections;
using System;

public class LocationRotationElement : BaseElement
{
	//возможность движения в данной клетке
	public bool left;
	public bool right;
	public bool up;
	public bool down;

	public bool Contains(LocationRotation.Rotation rotate)
	{
		if (rotate == LocationRotation.Rotation.Left)
			return left;
		else if (rotate == LocationRotation.Rotation.Right)
			return right;
		else if (rotate == LocationRotation.Rotation.Up)
			return up;
		else if (rotate == LocationRotation.Rotation.Down)
			return down;
		else
			return false;
	}
}
	