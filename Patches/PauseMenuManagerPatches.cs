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

        //We do the same thing in both cases so we can just call a singular patch function.
        public static void dualPatch()
        {
            try
            {
                if (BailOutModePlugin.BailedOut)
                {
                    //BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.gameplayModifiers.noFail = false;
                    BailOutModePlugin.BailedOut = false;
                    Console.WriteLine("[BailOut] Reset No Fail");
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }
    }
}
