using UnityEngine;

public abstract class SingletonBase : MonoBehaviour
{
}

public class SingletonMonoBehaviour<T> : SingletonBase where T: class
{
	public bool destroyExcessInstances = false; // автоматически уничтожать второй инстанс, если он вдруг появится
	
	private static T _instance;
	
	public static bool isInstantiated
	{
		get { return _instance as SingletonMonoBehaviour<T> != null; }
	}
	
	public static T i
	{
		get
		{
			if ((_instance as SingletonMonoBehaviour<T>)==null)
			{
				_instance = FindInstance();
			}
			return _instance;
		}
	}
	
	public static T instance
	{
		get{ return i; }
	}
		
	public virtual bool Awake()
	{
		if( _instance as SingletonMonoBehaviour<T>!=null )
		{
			if( _instance as SingletonMonoBehaviour<T>!=this )
			{
				if( this.destroyExcessInstances )
				{
					DestroyImmediate(this.gameObject); // второй инстанс лишний на этом празднике
					Debug.Log("Second instance of '"+typeof(T)+"' deleted.");
				}
				else
					Debug.LogWarning("Second instance of '"+typeof(T)+"' detected!");
				return false;
			}
		}
		else
			_instance = FindInstance();
		
		return true;
	}
	
	static T  FindInstance()
	{
		Object[] objs = Object.FindObjectsOfType(typeof(T));
		
		if (objs.Length > 1)
		{
			Debug.LogError("Only one instance of " +
				typeof(T).FullName +
				" can be created!");
			return null;
		}
		else if (objs.Length == 0)
		{
			Debug.LogError("There's no instances of " +
				typeof(T).FullName +
				" yet. Something wrong!");
			return null;
		}
		
		return objs[0] as T;
	}
	
	public virtual void OnDestroy()
	{
		if( _instance as SingletonMonoBehaviour<T> ==this )
			_instance = null;
	}
}