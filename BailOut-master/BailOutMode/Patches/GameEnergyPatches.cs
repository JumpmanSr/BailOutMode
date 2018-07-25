using System;
using System.Reflection;
using Ryder.Lightweight;
using TMPro;
using UnityEngine;

class GameEnergyPatches
{
    private static Redirection addEnergyRedirection;
    public static TextMeshPro bailoutNotificationText;

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

        if (BailOutModePlugin.bailoutNotification) showBailoutNotification();
        Console.WriteLine("[Bailout] Lethal energy reached, bailing out!");
        BailOutModePlugin.BailedOut = true;
        

        try
        {
            typeof(GameEnergyCounter).GetField("_cannotFail", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(instance, true);

            var sceneSetup = UnityEngine.Object.FindObjectOfType<MainGameSceneSetup>();
            MainGameSceneSetupData setupData = typeof(MainGameSceneSetup).GetField("_mainGameSceneSetupData", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(sceneSetup) as MainGameSceneSetupData;
            setupData.gameplayOptions.noEnergy = true;

            UnityEngine.Object.FindObjectOfType<GameEnergyUIPanel>().EnableEnergyPanel(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        value = 0;
        addEnergyRedirection.InvokeOriginal(self, value);
    }
    //Shoutout to foolish dave for all of this.


    //Shoutout to  andruzzzhka living up to his name of UI wizard.
    static void showBailoutNotification()
    {
        bailoutNotificationText = CreateWorldText((new GameObject()).transform, "");
        bailoutNotificationText.rectTransform.anchoredPosition3D = new Vector3(9.2f, -4f, 8f);
        bailoutNotificationText.color = Color.white;
        bailoutNotificationText.fontSize = 5f;
        bailoutNotificationText.text = "Bailed Out!";
    }

    public static TextMeshPro CreateWorldText(Transform parent, string text)
    {
        TextMeshPro textMesh = new GameObject("TextMeshPro_GO").AddComponent<TextMeshPro>();
        textMesh.transform.SetParent(parent, false);
        textMesh.text = text;
        textMesh.fontSize = 5;
        textMesh.color = Color.white;
        textMesh.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");


        return textMesh;
    }


}
