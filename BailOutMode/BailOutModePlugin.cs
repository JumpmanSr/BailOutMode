﻿using IllusionPlugin;
using System;
using System.Reflection;
using BailOutMode.Patches;
using UnityEngine;
using UnityEngine.SceneManagement;
using BeatSaberTweaks;
using BailOutMode.Helpers;

public class BailOutModePlugin : IPlugin
{
    public string Name => "BailOut Mode BS (0.11.2 compatible)";
    public string Version => "0.789";
    public static bool shouldIBail = true;
    public static bool bailoutNotification = false; //off by default
    public static bool BailedOut = false;

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

        SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;


    }

    public void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
    {
        if (SettingsUI.isMenuScene(scene)) //MainScene
        {
            //God bless Taz for easy settings options <3
            Settings.loadBailoutSettings();
            var subMenu = SettingsUI.CreateSubMenu("BailOut Settings");
            var bailOutToggle = subMenu.AddBool("BailOut Mode");
            bailOutToggle.GetValue += delegate { return shouldIBail; };
            bailOutToggle.SetValue += delegate (bool value) { shouldIBail = value; Settings.saveBailoutSettings(); };

            var bailoutNotificationToggle = subMenu.AddBool("BailOut Notification");
            bailoutNotificationToggle.GetValue += delegate { return bailoutNotification; };
            bailoutNotificationToggle.SetValue += delegate (bool value2) { bailoutNotification = value2; Settings.saveBailoutSettings(); };
            //Console.WriteLine("[Bailout] Toggles added to settings"); Attempting to eliminate console spam
        }
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