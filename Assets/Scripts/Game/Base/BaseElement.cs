using UnityEngine;
using System.Collections;

public class BaseElement : MonoBehaviour 
{
	public int indexHorizontal;
	public int indexVertical;
	public bool passive;

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
	}

	public float x
	{
		get
		{
			return transform.position.x;
		}
	}

	public float y
	{
		get
		{
			return transform.position.y;
		}
	}

	public bool SameHorizontal(Transform other) //находятся на одной горизонали
	{
		if (y - other.position.y < EPS && y - other.position.y > -EPS)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool SameVertical(Transform other) //находятся на одной вертикали
	{
		if (x - other.position.x < EPS && x - other.position.x > -EPS)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool Same(Transform pos, bool big)
	{
		if (big && transform.position.x - pos.position.x < BIGEPS && transform.position.x - pos.position.x > -BIGEPS && 
		    transform.position.y - pos.position.y < BIGEPS && transform.position.y - pos.position.y > -BIGEPS)
		{
			return true;
		}
		else if (! big && transform.position.x - pos.position.x < EPS && transform.position.x - pos.position.x > -EPS && 
		         transform.position.y - pos.position.y < EPS && transform.position.y - pos.position.y > -EPS)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public int CompareTo(object obj, bool horizontal)
	{
		if (obj == null)
		{
			Debug.Log("ERROR1111111");
			return 1;
		}
		
		BaseElement otherElement = obj as BaseElement;
		if (otherElement == null)
		{
			Debug.Log("ERROR2222");
			return 1;
		}

		float x1, x2, y1, y2;

		if (horizontal)
		{
			x1 = x;
			y1 = y;

			x2 = otherElement.x;
			y2 = otherElement.y;
		}
		else
		{
			x1 = y;
			y1 = x;
			
			x2 = otherElement.y;
			y2 = otherElement.x;
		}

		if (y1 - y2 > EPS)
		{
			return 1;
		}
		else if (y1 - y2 < - EPS)
		{
			return -1;
		}
		else
		{
			return (x1 - x2 > 0) ? 1 : -1;
		}
	}
	
	const float EPS = 1f;
	const float BIGEPS = 10f;

	Transform _transform;
}
