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
using System.Runtime.CompilerServices;
using System.Reflection;
using System.CodeDom;
using UnityEngine.Animations;
using System.Xml.Linq;
using System.Security.Policy;
using System.Runtime.Remoting.Contexts;
using System.Reflection.Emit;

namespace EnhancedTraining
{

    // [TODO LIST] 2023.10.10
    //New Startするときに、追加したUIObjectの情報が行かない
    //Initを関数化させて、New Start時に、追加したUIObjectの情報を行かせるようにする必要がある。

    public class CustomTrainingMod : MonoBehaviour
    {
        //説明文：
        public static bool isAddedEnhancedTraining = false;
        public static int originalLastMyID;
        public static Menu_Training_Select mTS_;
        public static savegameScript gS_;

        //GameDesign
        public static TrainingUIObject ET_gameDesign01;
        public static TrainingUIObject ET_gameDesign02;
        public static TrainingUIObject ET_gameDesign03;
        public static TrainingUIObject ET_gameDesign04;
        public static TrainingUIObject ET_gameDesign05;

        //Programming
        public static TrainingUIObject ET_programming01;
        public static TrainingUIObject ET_programming02;
        public static TrainingUIObject ET_programming03;
        public static TrainingUIObject ET_programming04;
        public static TrainingUIObject ET_programming05;

        //Graphic
        public static TrainingUIObject ET_graphic01;
        public static TrainingUIObject ET_graphic02;
        public static TrainingUIObject ET_graphic03;
        public static TrainingUIObject ET_graphic04;
        public static TrainingUIObject ET_graphic05;

        //Sound
        public static TrainingUIObject ET_sound01;
        public static TrainingUIObject ET_sound02;
        public static TrainingUIObject ET_sound03;
        public static TrainingUIObject ET_sound04;
        public static TrainingUIObject ET_sound05;

        //Marketing
        public static TrainingUIObject ET_marketing01;
        public static TrainingUIObject ET_marketing02;
        public static TrainingUIObject ET_marketing03;
        public static TrainingUIObject ET_marketing04;
        public static TrainingUIObject ET_marketing05;

        //gameTesting
        public static TrainingUIObject ET_gameTesting01;
        public static TrainingUIObject ET_gameTesting02;
        public static TrainingUIObject ET_gameTesting03;
        public static TrainingUIObject ET_gameTesting04;
        public static TrainingUIObject ET_gameTesting05;

        //hardware
        public static TrainingUIObject ET_hardware01;
        public static TrainingUIObject ET_hardware02;
        public static TrainingUIObject ET_hardware03;
        public static TrainingUIObject ET_hardware04;
        public static TrainingUIObject ET_hardware05;

        //research
        public static TrainingUIObject ET_research01;
        public static TrainingUIObject ET_research02;
        public static TrainingUIObject ET_research03;
        public static TrainingUIObject ET_research04;
        public static TrainingUIObject ET_research05;


        public static float maxLevel;

        /// <summary>
        /// savegameScriptのインスタンスを取得するために存在するメソッド
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix, HarmonyPatch(typeof(savegameScript), "Start")]
        static void FindScripts(savegameScript __instance)
        {
            gS_ = __instance;
            mTS_ = Traverse.Create(gS_).Field<GUI_Main>("guiMain_").Value.uiObjects[92].GetComponent<Menu_Training_Select>();
            originalLastMyID = mTS_.trainingCosts.Length - 1;
        }

        static void FindSkillLevel()
        {
            characterScript characterScripts = new characterScript();
            characterScripts.beruf = 0;
            maxLevel = Traverse.Create(characterScripts).Method("GetSkillCap_Skill", 0).GetValue<float>();
        }

        static void SetModUIObject()
        {
            //Game Design
            ET_gameDesign01 = new TrainingUIObject("ET_Training_Kurs_GD_1", "Game design for Legend☆I", 1, 100000, maxLevel, 3, 600f, 9999);
            ET_gameDesign01.SetUIObject();
            ET_gameDesign02 = new TrainingUIObject("ET_Training_Kurs_GD_2", "Game design for Legend☆II", 1, 250000, maxLevel, 4, 600f, ET_gameDesign01.SiblingIndex);
            ET_gameDesign02.SetUIObject();
            ET_gameDesign03 = new TrainingUIObject("ET_Training_Kurs_GD_3", "Game design for Legend☆III", 1, 650000, maxLevel, 5, 600f, ET_gameDesign02.SiblingIndex);
            ET_gameDesign03.SetUIObject();
            ET_gameDesign04 = new TrainingUIObject("ET_Training_Kurs_GD_4", "Game design for Legend☆IV", 1, 1600000, maxLevel, 6, 600f, ET_gameDesign03.SiblingIndex);
            ET_gameDesign04.SetUIObject();
            ET_gameDesign05 = new TrainingUIObject("ET_Training_Kurs_GD_5", "Game design for Legend☆V", 1, 5500000, maxLevel, 7, 600f, ET_gameDesign04.SiblingIndex);
            ET_gameDesign05.SetUIObject();

            //Programming
            ET_programming01 = new TrainingUIObject("ET_Training_Kurs_Programming_1", "Programming for Legend☆I", 2, 100000, maxLevel, 3, 600f, 9999);
            ET_programming01.SetUIObject();
            ET_programming02 = new TrainingUIObject("ET_Training_Kurs_Programming_2", "Programming for Legend☆II", 2, 250000, maxLevel, 4, 600f, ET_programming01.SiblingIndex);
            ET_programming02.SetUIObject();
            ET_programming03 = new TrainingUIObject("ET_Training_Kurs_Programming_3", "Programming for Legend☆III", 2, 650000, maxLevel, 5, 600f, ET_programming02.SiblingIndex);
            ET_programming03.SetUIObject();
            ET_programming04 = new TrainingUIObject("ET_Training_Kurs_Programming_4", "Programming for Legend☆IV", 2, 1600000, maxLevel, 6, 600f, ET_programming03.SiblingIndex);
            ET_programming04.SetUIObject();
            ET_programming05 = new TrainingUIObject("ET_Training_Kurs_Programming_5", "Programming for Legend☆V", 2, 5500000, maxLevel, 7, 600f, ET_programming04.SiblingIndex);
            ET_programming05.SetUIObject();

            //Graphic
            ET_graphic01 = new TrainingUIObject("ET_Training_Kurs_Graphic_1", "Graphic for Legend☆I", 3, 100000, maxLevel, 3, 600f, 9999);
            ET_graphic01.SetUIObject();
            ET_graphic02 = new TrainingUIObject("ET_Training_Kurs_Graphic_2", "Graphic for Legend☆II", 3, 250000, maxLevel, 4, 600f, ET_graphic01.SiblingIndex);
            ET_graphic02.SetUIObject();
            ET_graphic03 = new TrainingUIObject("ET_Training_Kurs_Graphic_3", "Graphic for Legend☆III", 3, 650000, maxLevel, 5, 600f, ET_graphic02.SiblingIndex);
            ET_graphic03.SetUIObject();
            ET_graphic04 = new TrainingUIObject("ET_Training_Kurs_Graphic_4", "Graphic for Legend☆IV", 3, 1600000, maxLevel, 6, 600f, ET_graphic03.SiblingIndex);
            ET_graphic04.SetUIObject();
            ET_graphic05 = new TrainingUIObject("ET_Training_Kurs_Graphic_5", "Graphic for Legend☆V", 3, 5500000, maxLevel, 7, 600f, ET_graphic04.SiblingIndex);
            ET_graphic05.SetUIObject();

            //Sound
            ET_sound01 = new TrainingUIObject("ET_Training_Kurs_Sound_1", "Sound for Legend☆I", 4, 100000, maxLevel, 3, 600f, 9999);
            ET_sound01.SetUIObject();
            ET_sound02 = new TrainingUIObject("ET_Training_Kurs_Sound_2", "Sound for Legend☆II", 4, 250000, maxLevel, 4, 600f, ET_sound01.SiblingIndex);
            ET_sound02.SetUIObject();
            ET_sound03 = new TrainingUIObject("ET_Training_Kurs_Sound_3", "Sound for Legend☆III", 4, 650000, maxLevel, 5, 600f, ET_sound02.SiblingIndex);
            ET_sound03.SetUIObject();
            ET_sound04 = new TrainingUIObject("ET_Training_Kurs_Sound_4", "Sound for Legend☆IV", 4, 1600000, maxLevel, 6, 600f, ET_sound03.SiblingIndex);
            ET_sound04.SetUIObject();
            ET_sound05 = new TrainingUIObject("ET_Training_Kurs_Sound_5", "Sound for Legend☆V", 4, 5500000, maxLevel, 7, 600f, ET_sound04.SiblingIndex);
            ET_sound05.SetUIObject();

            //Marketing
            ET_marketing01 = new TrainingUIObject("ET_Training_Kurs_Marketing_1", "Marketing for Legend☆I", 5, 100000, maxLevel, 3, 600f, 9999);
            ET_marketing01.SetUIObject();
            ET_marketing02 = new TrainingUIObject("ET_Training_Kurs_Marketing_2", "Marketing for Legend☆II", 5, 250000, maxLevel, 4, 600f, ET_marketing01.SiblingIndex);
            ET_marketing02.SetUIObject();
            ET_marketing03 = new TrainingUIObject("ET_Training_Kurs_Marketing_3", "Marketing for Legend☆III", 5, 650000, maxLevel, 5, 600f, ET_marketing02.SiblingIndex);
            ET_marketing03.SetUIObject();
            ET_marketing04 = new TrainingUIObject("ET_Training_Kurs_Marketing_4", "Marketing for Legend☆IV", 5, 1600000, maxLevel, 6, 600f, ET_marketing03.SiblingIndex);
            ET_marketing04.SetUIObject();
            ET_marketing05 = new TrainingUIObject("ET_Training_Kurs_Marketing_5", "Marketing for Legend☆V", 5, 5500000, maxLevel, 7, 600f, ET_marketing04.SiblingIndex);
            ET_marketing05.SetUIObject();

            //Game Testing
            ET_gameTesting01 = new TrainingUIObject("ET_Training_Kurs_GameTesting_1", "Game Testing for Legend☆I", 6, 100000, maxLevel, 3, 600f, 9999);
            ET_gameTesting01.SetUIObject();
            ET_gameTesting02 = new TrainingUIObject("ET_Training_Kurs_GameTesting_2", "Game Testing for Legend☆II", 6, 250000, maxLevel, 4, 600f, ET_gameTesting01.SiblingIndex);
            ET_gameTesting02.SetUIObject();
            ET_gameTesting03 = new TrainingUIObject("ET_Training_Kurs_GameTesting_3", "Game Testing for Legend☆III", 6, 650000, maxLevel, 5, 600f, ET_gameTesting02.SiblingIndex);
            ET_gameTesting03.SetUIObject();
            ET_gameTesting04 = new TrainingUIObject("ET_Training_Kurs_GameTesting_4", "Game Testing for Legend☆IV", 6, 1600000, maxLevel, 6, 600f, ET_gameTesting03.SiblingIndex);
            ET_gameTesting04.SetUIObject();
            ET_gameTesting05 = new TrainingUIObject("ET_Training_Kurs_GameTesting_5", "Game Testing for Legend☆V", 6, 5500000, maxLevel, 7, 600f, ET_gameTesting04.SiblingIndex);
            ET_gameTesting05.SetUIObject();

            //Hardware
            ET_hardware01 = new TrainingUIObject("ET_Training_Kurs_Hardware_1", "Hardware for Legend☆I", 7, 100000, maxLevel, 3, 600f, 9999);
            ET_hardware01.SetUIObject();
            ET_hardware02 = new TrainingUIObject("ET_Training_Kurs_Hardware_2", "Hardware for Legend☆II", 7, 250000, maxLevel, 4, 600f, ET_hardware01.SiblingIndex);
            ET_hardware02.SetUIObject();
            ET_hardware03 = new TrainingUIObject("ET_Training_Kurs_Hardware_3", "Hardware for Legend☆III", 7, 650000, maxLevel, 5, 600f, ET_hardware02.SiblingIndex);
            ET_hardware03.SetUIObject();
            ET_hardware04 = new TrainingUIObject("ET_Training_Kurs_Hardware_4", "Hardware for Legend☆IV", 7, 1600000, maxLevel, 6, 600f, ET_hardware03.SiblingIndex);
            ET_hardware04.SetUIObject();
            ET_hardware05 = new TrainingUIObject("ET_Training_Kurs_Hardware_5", "Hardware for Legend☆V", 7, 5500000, maxLevel, 7, 600f, ET_hardware04.SiblingIndex);
            ET_hardware05.SetUIObject();

            //Research
            ET_research01 = new TrainingUIObject("ET_Training_Kurs_Research_1", "Research for Legend☆I", 8, 100000, maxLevel, 3, 600f, 9999);
            ET_research01.SetUIObject();
            ET_research02 = new TrainingUIObject("ET_Training_Kurs_Research_2", "Research for Legend☆II", 8, 250000, maxLevel, 4, 600f, ET_research01.SiblingIndex);
            ET_research02.SetUIObject();
            ET_research03 = new TrainingUIObject("ET_Training_Kurs_Research_3", "Research for Legend☆III", 8, 650000, maxLevel, 5, 600f, ET_research02.SiblingIndex);
            ET_research03.SetUIObject();
            ET_research04 = new TrainingUIObject("ET_Training_Kurs_Research_4", "Research for Legend☆IV", 8, 1600000, maxLevel, 6, 600f, ET_research03.SiblingIndex);
            ET_research04.SetUIObject();
            ET_research05 = new TrainingUIObject("ET_Training_Kurs_Research_5", "Research for Legend☆V", 8, 5500000, maxLevel, 7, 600f, ET_research04.SiblingIndex);
            ET_research05.SetUIObject();

        }
        static void Init(savegameScript gS_)
        {
            FindScripts(gS_);
            FindSkillLevel();
            if (mTS_.trainingCosts.Length > 24) { return; } //追加処理をすると、mTS_.trainingCosts.Lengthは24以上になるため、さらなる処理を防ぐ
            
            Debug.Log("--- Enhanced.Training.Menu_Training_Select...etc : AddedCustomUI ---");

            //追加するUIObjectの情報を全てここに記述する
            SetModUIObject();
        }

        /// <summary>
        /// セーブデータロード時に、追加したUIObjectの情報を行かせるために存在するメソッド
        /// </summary>
        [HarmonyPrefix, HarmonyPatch(typeof(savegameScript), "LoadTasks")]
        static void AfterInject_SavegameScript_LoadTasks()
        {
            Init(gS_);
        }

        /// <summary>
        /// New Start時に、追加したUIObjectの情報を行かせるために存在するメソッド
        /// </summary>
        [HarmonyPostfix, HarmonyPatch(typeof(Menu_NewGame), "OnEnable")]
        static void AfterInject_Menu_NewGame_OnEnable()
        {
            Init(gS_);
        }

        //Pink with RGBA
        static Color pink = new Color(1f, 0.5f, 0.5f, 1f);
        //Light Red
        static Color lightRed = new Color(1f, 0.5f, 0.5f, 1f);
        static void SetCustomColor()
        {

        }
        /// <summary>
        /// Trainingのコース選択画面のUIObjectを一つずつ設定するために存在するメソッド
        /// この情報をもとに、taskTrainingがStartされ、RoomのUIに情報が行く。
        /// 裏を返せば、ゲーム起動時では、taskTrainingを介さないため、何らかの方法でRoomのUIに情報を行かせる必要がある……
        /// そうしないと、TextNameに"N"という謎の文字列が入ったままになる。
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix, HarmonyPatch(typeof(Item_Training_Kurs), "SetData")]
        static void CustomSetData(Item_Training_Kurs __instance)
        {
            if (__instance.myID > originalLastMyID)
            {
                //TrainingUIObjectのリストを回して、TextNameを設定する
                foreach (TrainingUIObject foundObject in TrainingUIObject.instanceList)
                {
                    if (foundObject.MyID == __instance.myID)
                    {
                        __instance.uiObjects[0].GetComponent<UnityEngine.UI.Text>().text = foundObject.TextName;
                    }
                    else
                    {
                    }
                }
                
                string textEffect;
                switch (mTS_.trainingEffekt[__instance.myID])
                {
                    case 0:
                        textEffect = "Effectiveness: <color=blue>Low</color>";
                        break;
                    case 1:
                        textEffect = "Effectiveness: <color=blue>Medium</color>";
                        break;
                    case 2:
                        textEffect = "Effectiveness: <color=blue>High</color>";
                        break;
                    case 4:
                        textEffect = "Effectiveness: <color=#cc00ff>Very High</color>";
                        break;
                    case 8:
                        textEffect = "Effectiveness: <color=#ff00dd>Exceptionally High</color>";
                        break;
                    case 16:
                        textEffect = "Effectiveness: <color=#fff200>Legendary</color>";
                        break;
                    case 32:
                        textEffect = "Effectiveness: <color=orange>Wizard Hacker</color>";
                        break;
                    case 64:
                        textEffect = "Effectiveness: <color=#ff0000>Mad Games Tycoon 2's Developer</color>";
                        break;
                    default:
                        textEffect = "Effectiveness: <color=blue>Low</color>";
                        break;
                }
                __instance.uiObjects[4].GetComponent<UnityEngine.UI.Text>().text = textEffect;

                //this.uiObjects[1].GetComponent<Image>().sprite = this.menuTraining_.trainingSprites[this.myID];
                //string text = this.tS_.GetText(562);
                //text = text.Replace("<NUM>", this.mS_.Round(this.menuTraining_.trainingMaxLearn[this.myID], 2).ToString());
                //this.uiObjects[3].GetComponent<Text>().text = text;
            }
        }

        private static taskTraining taskTraining_;
        private static roomButtonScript rbS_;
        private static GUI_Main guiMain_;
        private static textScript tS_;
        /// <summary>
        /// RoomのUIに情報を行かせるために存在するメソッド、Updateのため1フレームごとに呼び出される
        /// Update関数のためか、Transpilerでうまくいかないため、Postfixで対応する
        /// セーブ・ロードした際に、追加UIの情報が行かないため、追加の処理を入れる。
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix, HarmonyPatch(typeof(roomScript), "UpdateWindowTraining")]
        static void AddCustomInfoToUpdateWindowTraining(roomScript __instance)
        {
            if (__instance.taskGameObject)
            {
                //if(taskTraining_ == null) { taskTraining_ = __instance.GetTaskTraining(); }
                //if(guiMain_ == null) { guiMain_ = Traverse.Create(__instance).Field<GUI_Main>("guiMain_").Value; }
                //if(rbS_ == null) { rbS_ = Traverse.Create(__instance).Field<roomButtonScript>("rbS_").Value; }
                //if(tS_ == null) { tS_ = Traverse.Create(__instance).Field<textScript>("tS_").Value; }
                taskTraining_ = __instance.GetTaskTraining();
                guiMain_ = Traverse.Create(__instance).Field<GUI_Main>("guiMain_").Value;
                rbS_ = Traverse.Create(__instance).Field<roomButtonScript>("rbS_").Value;
                tS_ = Traverse.Create(__instance).Field<textScript>("tS_").Value;

                Transform mTS_Transform = guiMain_.uiObjects[92].GetComponent<Menu_Training_Select>().uiObjects[0].transform;

                //mTS_Transformをforeachして、slotと同じIDのものを探す
                string text = "";
                int slot = taskTraining_.slot;
                foreach (Transform child in mTS_Transform)
                {
                    if(child.gameObject.GetComponent<Item_Training_Kurs>() == null) { continue; }
                    if (child.gameObject.GetComponent<Item_Training_Kurs>().myID == slot)
                    {
                        if(slot > originalLastMyID)
                        {
                            foreach (TrainingUIObject foundObject in TrainingUIObject.instanceList) //インスタンス化されたTrainingUIObjectのリストを回す
                            {
                                if (foundObject.MyID == slot)
                                {
                                    text = foundObject.TextName;
                                }
                                else
                                {
                                }
                            }
                        }
                        else
                        {
                            text = tS_.GetText(538 + slot); //オリジナルのUIオブジェクト用
                        }
                    }
                }
                rbS_.uiWindows[5].GetComponent<roomWindow>().uiObjects[0].GetComponent<Text>().text = text;
                //Debug.Log("text " + text);
            }
        }

    }
}