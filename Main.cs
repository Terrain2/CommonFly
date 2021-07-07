using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace LibCommonFly
{
    [BepInPlugin(Guid, Name, Version), BepInDependency("Terrain.MuckSettings")]
    public class Main : BaseUnityPlugin
    {
        public const string
            Name = "CommonFly",
            Author = "Terrain",
            Guid = Author + "." + Name,
            Version = "1.0.0.0";

        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;

        public static ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "fly.cfg"), true);
        public static ConfigEntry<KeyCode> up = config.Bind<KeyCode>("Fly", "fly-up", KeyCode.Space, "Ascend when flying.");
        public static ConfigEntry<KeyCode> down = config.Bind<KeyCode>("Fly", "fly-down", KeyCode.LeftControl, "Descend when flying.");

        Main()
        {
            log = Logger;
            harmony = new Harmony(Guid);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);

            config.SaveOnConfigSet = true;
            harmony.PatchAll(assembly);
        }
    }

}

public static class CommonFly
{
    public static bool flying { get; set; }

    public static bool noclip { get; set; }

    public static bool flyUp { get; internal set; }

    public static bool flyDown { get; internal set; }
}