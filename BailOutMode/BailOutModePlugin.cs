
using System;
using BailOutMode.Patches;
using UnityEngine.SceneManagement;
using BailOutMode.Helpers;
using IPA.Old;

public class BailOutModePlugin : IPlugin
{
    public string Name => "BailOut Mode BS (1.6.0 compatible)";
    public string Version => "1.7A";
    public static bool shouldIBail = true;
    public static bool bailoutNotification = false; //off by default
    public static bool BailedOut = false;
    //private BoolViewController bailoutSetting;

    static BailOutModePlugin()
    {

    }

    public void OnApplicationStart()
    {
        Console.WriteLine("[BailOut] Starting Bailout");
        GameEnergyPatches.PatchMethods(); //this patches the functions necessary to stop you from failing when your energy reaches it's lowest point.
        ResultsViewControllerPatches.PatchMethods(); // jk i fixed it, but I didn't rename shit sorry :(
        PauseMenuManagerPatches.PatchMethods(); // This should prevent bailout mode from glitching out if you fail then quit.
        Console.WriteLine("[BailOut] Assembly Patched");
        Settings.loadBailoutSettings();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

    }


    public void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode sceneMode)
    {

    }


    #region Handlers
    public void OnApplicationQuit()
    {

    }

    public void OnSceneLoaded(Scene arg0, LoadSceneMode sceneMode)
    {

    }


    public void OnLevelWasLoaded(int level)
    {

    }

    public void OnLevelWasInitialized(int level)
    {

    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {

    }
    
    public void OnActiveSceneChanged(Scene arg0, Scene scene) { }

    public void OnSceneUnloaded(Scene scene) { }

    #endregion

}
