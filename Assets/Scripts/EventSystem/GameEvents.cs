#region VoidEvent
public class EvtOnSceneReady : Events<EvtOnSceneReady> { }
public class EvtOnSceneFadeInFinished : Events<EvtOnSceneFadeInFinished> { }

#endregion

#region LoadEvent

public class EvtNotifyColdStartup : Events<EvtNotifyColdStartup>
{
	//#if UNITY_EDITOR
	//	static string noRegistLog = "A Scene loading was requested, but nobody picked it up.";
	//#endif
	public GameSceneSO locationToLoad;
	public bool showLoadingScreen = false;
	public bool fadeScreen = false;
}
public class EvtLoadLocation : Events<EvtLoadLocation>
{
	public GameSceneSO locationToLoad;
	public bool showLoadingScreen = false;
	public bool fadeScreen = false;
}
public class EvtLoadMenu : Events<EvtLoadMenu>
{
	public GameSceneSO locationToLoad;
	public bool showLoadingScreen = false;
	public bool fadeScreen = false;
}
#endregion
