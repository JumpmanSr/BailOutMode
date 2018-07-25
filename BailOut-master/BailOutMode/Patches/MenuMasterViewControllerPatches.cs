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

            //Custom (Jumpman): My method only works if you make it to the end of the song. Could I make it better? Maybe. Will I? Eh

            MethodInfo didActivatePatch = typeof(ResultsViewControllerPatches).GetMethod(nameof(DidActivatePatch), BindingFlags.NonPublic | BindingFlags.Static);
            didActivateRedirection = new Redirection(didActivateInfo, didActivatePatch, true);
        }

        private static void DidActivatePatch(object self, bool value1, VRUI.VRUIViewController.ActivationType value2)
        {
            didActivateRedirection.InvokeOriginal(self, value1, value2);
            try
            {
                if (BailOutModePlugin.BailedOut)
                {
                    ((GameplayOptions)typeof(ResultsViewController).GetField("_gameplayOptions", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(self)).noEnergy = false;
                    BailOutModePlugin.BailedOut = false;
                    Console.WriteLine("Bailout Reset");
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
