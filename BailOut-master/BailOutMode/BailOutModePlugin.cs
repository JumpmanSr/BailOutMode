using IllusionPlugin;
using System;
using System.Reflection;
using BailOutMode.Patches;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BailOutModePlugin : IPlugin
{
    public string Name => "BailOut Mode BS(0.11.1 compatible)";
    public string Version => "0.0.3.420";

    public static bool BailedOut = false;

    static BailOutModePlugin()
    {

    }

    public void OnApplicationStart()
    {
        Console.WriteLine("[Bailout] Starting Bailout");
        GameEnergyPatches.PatchMethods();
        MenuMasterViewControllerPatches.PatchMethods();// disabled 4 now because fuck u beatsaber // jk i fixed it, but I didn't rename shit sorry :(
        Console.WriteLine("[Bailout] Assembly Patched");
    }

    public void OnApplicationQuit()
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

}
