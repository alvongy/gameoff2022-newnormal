using System;
using System.Collections.Generic;

public abstract class Events
{

}
public class Events<T> : Events where T : Events<T>, new()
{
	static T _instance;
	static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new T();
			}
			return _instance;
		}
		set => _instance = value;

	}

	private static LinkedList<Action<Events>> registerList = new LinkedList<Action<Events>>();

	//protected static string noRegistLog = "Default no regist log";
	public static void Regist(Action<Events> register)
	{
		if (register == null)
		{
			return;
		}
		registerList.AddLast(register);
	}
	public static void Unregist(Action<Events> register)
	{
		if (register == null)
		{
			return;
		}
		registerList.Remove(register);
	}
	public static void Trigger()
	{
		//#if UNITY_EDITOR
		//		if (registerList.Count == 0)
		//		{
		//			Debug.Log(noRegistLog);
		//		}
		//#endif
		foreach (var item in registerList)
		{
			item?.Invoke(Instance);
		}
	}
	public static T GetEvents() => Instance;
}