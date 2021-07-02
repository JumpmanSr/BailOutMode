using System;
using System.Reflection;
using Ryder.Lightweight;
//using TMPro;
using UnityEngine;
using BS_Utils;

class GameEnergyPatches
{
    private static Redirection addEnergyRedirection;

    public static void PatchMethods()
    {
        MethodInfo addEnergyInfo =
            typeof(GameEnergyCounter).GetMethod("ProcessEnergyChange", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        MethodInfo addEnergyPatch =
            typeof(GameEnergyPatches).GetMethod(nameof(AddEnergyPatch), BindingFlags.NonPublic | BindingFlags.Static);
        addEnergyRedirection = new Redirection(addEnergyInfo, addEnergyPatch, true);
    }

    private static void AddEnergyPatch(object self, float value)
    {
        //If the Bailout Toggle is off, this will just run the original code and exit this function.
        if (!BailOutModePlugin.shouldIBail)
        {
            addEnergyRedirection.InvokeOriginal(self, value);
            return;
        }

        
        GameEnergyCounter instance = (GameEnergyCounter) self;
        if (!(instance.energy + value <= 1E-05f) || BailOutModePlugin.BailedOut)
        {
            addEnergyRedirection.InvokeOriginal(self, value);
            return;
        }

        //if (BailOutModePlugin.bailoutNotification) showBailoutNotification();
        Console.WriteLine("[Bailout] Lethal energy reached, bailing out! [1/4]");
        BailOutModePlugin.BailedOut = true;

        try
        {
            
            typeof(GameEnergyCounter).GetField("<noFail>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(instance, true);
            //typeof(GameEnergyCounter).GetField("noFail", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(instance, true);
            //typeof(GameEnergyCounter).GetField("<isInitialized>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(instance, true);
            Console.WriteLine("[Bailout] Disabling score submission[2/4]");
            BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("Bailed Out");
            Console.WriteLine("[Bailout] Disabling Ingame Energy Bar [3/4]");
            UnityEngine.Object.FindObjectOfType<GameEnergyUIPanel>().gameObject.SetActive(false);
            Console.WriteLine("[Bailout] Bailed out [4/4]");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        value = 100f;
        addEnergyRedirection.InvokeOriginal(self, value);
    }
    //Shoutout to foolish dave for the original idea, this has progressed over the years.

}
