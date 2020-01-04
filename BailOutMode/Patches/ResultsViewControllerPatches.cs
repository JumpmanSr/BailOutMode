using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ryder.Lightweight;


namespace BailOutMode.Patches
{
    class ResultsViewControllerPatches
    {
        private static Redirection didActivateRedirection;

        public static void PatchMethods()
        {
            MethodInfo didActivateInfo = typeof(ResultsViewController).GetMethod("DidActivate", BindingFlags.NonPublic | BindingFlags.Instance);

            //Custom (Jumpman): I made it better by adding more patches yay

            MethodInfo didActivatePatch = typeof(ResultsViewControllerPatches).GetMethod(nameof(DidActivatePatch), BindingFlags.NonPublic | BindingFlags.Static);
            didActivateRedirection = new Redirection(didActivateInfo, didActivatePatch, true);
        }

        private static void DidActivatePatch(object self, bool value1, HMUI.ViewController.ActivationType value2)
        {
            didActivateRedirection.InvokeOriginal(self, value1, value2);
            try
            {
                if (BailOutModePlugin.BailedOut)
                {
                    //((GameplayOptions)typeof(ResultsViewController).GetField("_gameplayOptions", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self)).noEnergy = false;
                    //BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.gameplayModifiers.noFail = false;
                    BailOutModePlugin.BailedOut = false;
                    Console.WriteLine("[BailOut] Reset No Fail");
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            BailOutModePlugin.BailedOut = false;
        }
    }
}
