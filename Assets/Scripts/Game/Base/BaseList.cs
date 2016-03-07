using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseList : MonoBehaviour
{
	protected void Sort(bool horizontal, BaseElement[] elements)
	{
		if (elements != null)
		{
			BaseElement element;
			for (int i=0; i<elements.Length - 1; i++)
			{
				for (int j=i+1; j<elements.Length; j++)
				{
					if (elements[i].CompareTo( elements[j], horizontal) > 0)
					{
						element = elements[i];
						elements[i] = elements[j];
						elements[j] = element;
					}
				}
			}
		}
	}

	protected BaseElement[] Copy(BaseElement[] from)
	{
		if (from == null)
		{
			return null;
		}

		BaseElement[] to = new BaseElement[from.Length];
		for (int i=0; i<from.Length; i++)
		{
			to[i] = from[i];
		}
		return to;
	}

	protected BaseElement[] Copy(List<BaseElement> from)
	{
		if (from == null)
		{
			return null;
		}
		
		BaseElement[] to = new BaseElement[from.Count];
		for (int i=0; i<from.Count; i++)
		{
			to[i] = from[i];
		}
		return to;
	}

	protected void SetIndex()
	{
		if (_elementsHorizontal != null)
		{
			for (int i=0; i<_elementsHorizontal.Length; i++)
			{
				_elementsHorizontal[i].indexHorizontal = i;
			}
		}

		if (_elementsVertical != null)
		{
			for (int i=0; i<_elementsVertical.Length; i++)
			{
				_elementsVertical[i].indexVertical = i;
			}
		}
	}

	public BaseElement[] elementsHorizontal
	{
		get
		{
			return _elementsHorizontal;
		}
	}

	public BaseElement[] elementsVertical
	{
		get
		{
			return _elementsVertical;
		}
	}

	protected BaseElement[] _elementsHorizontal;
	protected BaseElement[] _elementsVertical;
}
