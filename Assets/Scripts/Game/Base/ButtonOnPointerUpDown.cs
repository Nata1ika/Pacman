using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonOnPointerUpDown : MonoBehaviour, IPointerUpHandler, IPointerDownHandler 
{
	public System.Action OnPointerDownAction;
	public System.Action OnPointerUpAction;

	public void OnPointerDown(PointerEventData data)
	{
		if (OnPointerDownAction != null)
		{
			OnPointerDownAction();
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		if (OnPointerUpAction != null)
		{
			OnPointerUpAction();
		}
	}

}
