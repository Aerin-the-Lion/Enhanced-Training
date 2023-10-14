using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using UnityEngine.UI;

namespace EnhancedTraining
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInProcess("Mad Games Tycoon 2.exe")]
	public class Main : BaseUnityPlugin
	{
		public const string PluginGuid = "me.Aerin_the_Lion.Mad_Games_Tycoon_2.plugins.EnhancedTraining";
		public const string PluginName = "Enhanced Training";
		public const string PluginVersion = "1.0.0.0";
        // ****** Setting ******

        public static ConfigEntry<bool> CFG_IS_ENABLED { get; private set; }
        // Free Updateの開発コストの倍率
        //public static ConfigEntry<float> devCostMultiplyValue { get; private set; }
        // Free Updateの追加ポイント倍率
        //public static ConfigEntry<float> devAddPointMultiplyValue { get; private set; }
        // Free Updateの開発時間倍率
        //public static ConfigEntry<float> devTimeMultiplyValue { get; private set; }

        public void LoadConfig()
        {
            string textIsEnable = "0. MOD Settings";

            CFG_IS_ENABLED = Config.Bind<bool>(textIsEnable, "Activate the MOD", true, "If you need to disable the mod, toggle it to 'Enabled'");
            Config.SettingChanged += delegate (object sender, SettingChangedEventArgs args) { };
        }

        void Awake()
        {
            //Harmony harmony = new Harmony(PluginGuid);
            LoadConfig();
            if (!CFG_IS_ENABLED.Value) { return; }
            Harmony.CreateAndPatchAll(typeof(CustomTrainingMod));
            Harmony.CreateAndPatchAll(typeof(CharacterTraining));
        }

        //void Update()
        //{
            //base.gameObject.SetActive(false);
        //}


    }

}
