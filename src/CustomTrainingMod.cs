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
        public static TrainingUIObject testMenuObject;
        public static TrainingUIObject testMenuObject2;
        public static TrainingUIObject testMenuObject3;
        public static TrainingUIObject testMenuObject4;
        public static TrainingUIObject testMenuObject5;
        public static TrainingUIObject testMenuObject6;
        public static TrainingUIObject testMenuObject7;

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
        static void Init(savegameScript gS_)
        {
            FindScripts(gS_);
            FindSkillLevel();
            if (mTS_.trainingCosts.Length > 24) { return; } //追加処理をすると、mTS_.trainingCosts.Lengthは24以上になるため、さらなる処理を防ぐ
            
            Debug.Log("--- Enhanced.Training.Menu_Training_Select...etc : AddedCustomUI ---");

            //追加するUIObjectの情報を全てここに記述する
            testMenuObject = new TrainingUIObject("Test999", "Hogeee for Veterans", 1, 999999, maxLevel, 0, 1000f, 9999);
            testMenuObject.SetUIObject();
            testMenuObject2 = new TrainingUIObject("Test9992", "Hogeee for Veterans2", 2, 999999, maxLevel, 1, 1000f, testMenuObject.SiblingIndex);
            testMenuObject2.SetUIObject();
            testMenuObject3 = new TrainingUIObject("Test9993", "Hogeee for Veterans3", 3, 999999, maxLevel, 2, 1000f, testMenuObject.SiblingIndex);
            testMenuObject3.SetUIObject();
            testMenuObject4 = new TrainingUIObject("Test9994", "Hogeee for Veterans4", 4, 999999, maxLevel, 3, 1000f, testMenuObject.SiblingIndex);
            testMenuObject4.SetUIObject();
            testMenuObject5 = new TrainingUIObject("Test9995", "Hogeee for Veterans5", 5, 999999, maxLevel, 4, 1000f, testMenuObject.SiblingIndex);
            testMenuObject5.SetUIObject();
            testMenuObject6 = new TrainingUIObject("Test9996", "Hogeee for Veterans6", 6, 999999, maxLevel, 5, 1000f, testMenuObject.SiblingIndex);
            testMenuObject6.SetUIObject();
            testMenuObject7 = new TrainingUIObject("Test9997", "Hogeee for Veterans7", 7, 999999, maxLevel, 6, 1000f, testMenuObject.SiblingIndex);
            testMenuObject7.SetUIObject();
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
                        textEffect = "Effectiveness: <color=fuchsia>Very High</color>";
                        break;
                    case 8:
                        textEffect = "Effectiveness: <color=yellow>Exceptionally High</color>";
                        break;
                    case 16:
                        textEffect = "Effectiveness: <color=orange>Legendary☆</color>";
                        break;
                    case 32:
                        textEffect = "Effectiveness: <color=orange>Legendary☆2</color>";
                        break;
                    case 64:
                        textEffect = "Effectiveness: <color=orange>Legendary☆3</color>";
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