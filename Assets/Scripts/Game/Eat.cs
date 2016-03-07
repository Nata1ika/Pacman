using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Eat : BaseList 
{
	public void Load(Action OnEnd)
	{
		if (_elementsHorizontal == null)
		{
			_listEat = new List<BaseElement>();
		
			LocationRotationElement element;
			for (int i=0; i<_rotation.elementsHorizontal.Length; i++)
			{
				element = _rotation.elementsHorizontal[i] as LocationRotationElement;
				if (element != null)
				{
					CreateEat(element, _rotation.FindIndex (LocationRotation.Rotation.Right, element, false));
					CreateEat(element, _rotation.FindIndex (LocationRotation.Rotation.Up, element, false));
				}
			}

			_elementsHorizontal = Copy(_listEat);
			Sort(true, _elementsHorizontal);
			for (int i=0; i<_elementsHorizontal.Length; i++)
			{
				_elementsHorizontal[i].indexHorizontal = i;
			}

			//_elementsVertical = Copy(_listEat);
			//Sort(false, _elementsVertical);
			//for (int i=0; i<_elementsVertical.Length; i++)
			//{
			//	_elementsVertical[i].indexVertical = i;
			//}
		}
		else
		{
			for (int i=0; i<_elementsHorizontal.Length; i++)
			{
				_elementsHorizontal[i].passive = false;
				_elementsHorizontal[i].gameObject.SetActive(true);
			}
		}

		if (OnEnd != null)
		{
			OnEnd();
		}
	}

	//построение положений еды
	void CreateEat(LocationRotationElement first, LocationRotationElement second)
	{
		if (first == null || second == null)
		{
			return;
		}

		//по вертикали
		if (first.SameVertical(second.transform)) 
		{
			float y = second.y;
			while(y - first.y > EPS + ((first.indexHorizontal != 0) ? _durationEat : 0))
			{
				if (LoadUIPrefab<BaseElement>(ref _element, _prefab, null, first.transform))
				{
					_listEat.Add(_element);
					_element.transform.localPosition = new Vector3(0, y - first.y, 0);
					y -= _durationEat;
				}
			}
		}
		else //по горизоналми
		{
			float x = second.x;
			while(x - first.x > EPS)
			{
				if (LoadUIPrefab<BaseElement>(ref _element, _prefab, null, first.transform))
				{
					_listEat.Add(_element);
					_element.gameObject.transform.localPosition = new Vector3(x - first.x, 0, 0);
					x -= _durationEat;
				}
			}
		}
	}

	bool LoadUIPrefab<T> (ref T screenPartInstance, GameObject prefab, Action<T> onInit, Transform parent) where T: MonoBehaviour
	{
		var spGO = (GameObject) GameObject.Instantiate(prefab);
		var screenPartComponent = spGO.GetComponent<T>();
			
		if (screenPartComponent != null)
		{
			screenPartInstance = screenPartComponent;
			spGO.transform.SetParent(parent, false);

			if (onInit != null)
			{
				onInit(screenPartComponent);
			}

			return true;
		}
		else
		{
			Debug.LogError (string.Format ("Prefab {0} doesn't contain {1} component", prefab, typeof(T).Name));
			return false;
		}
	}

	public void ProcessEat(Transform packman)
	{
		for (int i=0; i<_elementsHorizontal.Length; i++)
		{
			float deltaY = _elementsHorizontal[i].transform.position.y - packman.position.y;

			if (deltaY > EPS)
			{
				return;
			}

			float deltaX = _elementsHorizontal[i].transform.position.x - packman.position.x;
			if (! _elementsHorizontal[i].passive && deltaX < EPS && deltaX > -EPS && deltaY < EPS && deltaY > -EPS)
			{
				_elementsHorizontal[i].passive = true;
				_elementsHorizontal[i].gameObject.SetActive(false);
				ScoreController.Eat();
			}
		}
	}



	BaseElement							_element;
	List<BaseElement>					_listEat;
	[SerializeField] GameObject 		_prefab;
	[SerializeField] LocationRotation 	_rotation;
	const int							_durationEat = 16;
	const float							EPS = 5f;
}
