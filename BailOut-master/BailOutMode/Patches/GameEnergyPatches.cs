using System;
using System.Reflection;
using Ryder.Lightweight;

class GameEnergyPatches
{
    private static Redirection addEnergyRedirection;

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
        GameEnergyCounter instance = (GameEnergyCounter) self;
        if (!(instance.energy + value <= 1E-05f) || BailOutModePlugin.BailedOut)
        {
            addEnergyRedirection.InvokeOriginal(self, value);
            return;
        }

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
}
