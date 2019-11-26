using System;
using System.Reflection;
using Ryder.Lightweight;
//using TMPro;
using UnityEngine;
using BS_Utils;

class GameEnergyPatches
{
    private static Redirection addEnergyRedirection;
    //public static TextMeshPro bailoutNotificationText;

    public static void PatchMethods()
    {
        MethodInfo addEnergyInfo =
            typeof(GameEnergyCounter).GetMethod("AddEnergy", BindingFlags.Public | BindingFlags.Instance);
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
        Console.WriteLine("[Bailout] Lethal energy reached, bailing out! [1/2]");
        BailOutModePlugin.BailedOut = true;

        try
        {
            
            typeof(GameEnergyCounter).GetField("<noFail>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(instance, true);
            //typeof(GameEnergyCounter).GetField("<isInitialized>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(instance, true);
            BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("Bailed Out");
            
            //typeof(GameEnergyUIPanel).Get("_energyBar", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(uIPanel, blankpic); // -----------------V maybe use self if really stuck
            ((UnityEngine.UI.Image)typeof(GameEnergyUIPanel).GetField("_energyBar", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(UnityEngine.Object.FindObjectOfType<GameEnergyUIPanel>())).enabled = false;
            Console.WriteLine("[Bailout] Lethal energy reached, bailing out! [2/2]");
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


    //Shoutout to  andruzzzhka living up to his name of UI wizard.
    //static void showBailoutNotification()
    //{
    //    bailoutNotificationText = CreateWorldText((new GameObject()).transform, "");
    //    bailoutNotificationText.rectTransform.anchoredPosition3D = new Vector3(9.2f, -6f, 8f);
    //    bailoutNotificationText.color = Color.white;
    //    bailoutNotificationText.fontSize = 5f;
    //    bailoutNotificationText.text = "Bailed Out!";
    //}

    //public static TextMeshPro CreateWorldText(Transform parent, string text)
    //{
    //    TextMeshPro textMesh = new GameObject("TextMeshPro_GO").AddComponent<TextMeshPro>();
    //    textMesh.transform.SetParent(parent, false);
    //    textMesh.text = text;
    //    textMesh.fontSize = 5;
    //    textMesh.color = Color.white;
    //    textMesh.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");


    //    return textMesh;
    //}


}
