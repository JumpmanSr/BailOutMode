using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BailOutMode.Helpers
{
    class Settings
    {
        public static void loadBailoutSettings()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "BailOutSettings.cfg")))
            {
                string[] settings = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "BailOutSettings.cfg"));
                if (settings.Length != 2) //There can only be two settings, if this is wrong generate a new settings file.
                {
                    saveBailoutSettings();
                    loadBailoutSettings();
                    return; //return here so we dont take out empty settings and get index exceptions
                    //wow recurssion :O
                }

                if(settings[0].Split('=')[1].ToLower() == "true") { BailOutModePlugin.shouldIBail = true; } //setting1
                else { BailOutModePlugin.shouldIBail = false; }
                if (settings[1].Split('=')[1].ToLower() == "true") {  BailOutModePlugin.bailoutNotification = true; } //setting2
                else { BailOutModePlugin.bailoutNotification = false; } //if its not true its false ;)
                Console.WriteLine("[BailOut] Settings loaded from file");
            }
            else
            {
                Console.WriteLine("[BailOut] Settings file not found, making new one.");
                saveBailoutSettings();
                loadBailoutSettings();
                //wow recurssion :O
            }
        }

        //This will also make a "default" file because the default settings are in memory.
        public static void saveBailoutSettings()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "BailOutSettings.cfg")))
            {
                File.Delete(Path.Combine(Environment.CurrentDirectory, "BailOutSettings.cfg"));
                //Console.WriteLine("[BailOut] Old settings file deleted"); trying to eliminate console spam
            }
            string[] settings = { "BailOut=" + BailOutModePlugin.shouldIBail.ToString().ToLower(), "BailOutNotification=" + BailOutModePlugin.bailoutNotification.ToString().ToLower() };
            File.WriteAllLines(Path.Combine(Environment.CurrentDirectory, "BailOutSettings.cfg"), settings);
            Console.WriteLine("[BailOut] Settings file saved");
        }
    }
}
