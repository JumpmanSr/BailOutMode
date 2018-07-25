using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ryder.Lightweight;

namespace BailOutMode.Patches
{
    class PauseMenuManagerPatches
    {
        public static Redirection didMenuButtonRedirection;
        public static Redirection didRestartButtonRedirection;

        public static void PatchMethods()
        {
            MethodInfo MenuButtonPressedInfo = typeof(PauseMenuManager).GetMethod("MenuButtonPressed", BindingFlags.Public | BindingFlags.Instance);
            MethodInfo dualPatchInfo = typeof(PauseMenuManagerPatches).GetMethod(nameof(MenuButtonPressedPatch), BindingFlags.Public | BindingFlags.Static);
            didMenuButtonRedirection = new Redirection(MenuButtonPressedInfo, dualPatchInfo, true);

            MethodInfo RestartButtonPressedInfo = typeof(PauseMenuManager).GetMethod("RestartButtonPressed", BindingFlags.Public | BindingFlags.Instance);
            MethodInfo RestartPatchInfo = typeof(PauseMenuManagerPatches).GetMethod(nameof(RestartButtonPressedPatch), BindingFlags.Public | BindingFlags.Static);
            didRestartButtonRedirection = new Redirection(RestartButtonPressedInfo, RestartPatchInfo, true);
        }

        public static void MenuButtonPressedPatch(object self)
        {
            dualPatch();
            didMenuButtonRedirection.InvokeOriginal(self);
        }

        public static void RestartButtonPressedPatch(object self)
        {
            dualPatch();
            didRestartButtonRedirection.InvokeOriginal(self);
        }

        public static void dualPatch()
        {
            try
            {
                if (BailOutModePlugin.BailedOut)
                {
                    var sceneSetup = UnityEngine.Object.FindObjectOfType<MainGameSceneSetup>();
                    MainGameSceneSetupData setupData = typeof(MainGameSceneSetup).GetField("_mainGameSceneSetupData", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(sceneSetup) as MainGameSceneSetupData;
                    setupData.gameplayOptions.noEnergy = false;
                    BailOutModePlugin.BailedOut = false;
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }
    }
}
