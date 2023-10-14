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
using System.Security.Cryptography;
using System.Collections;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace EnhancedTraining
{
    public class CharacterTraining : MonoBehaviour
    {
        //private static roomScript rS_;
        private static Menu_Training_Select mTS_;
        private static taskTraining taskTraining = null;
        private static Traverse traverse;
        private static bool uiVisible;
        private static int ePop_Training = 0;
        private static bool[] learnSkillsSet = new bool[8];
        private static float GetSkillCap_Skill = 0;


        static void Init(characterScript __instance, roomScript rS_)
        {
            traverse = Traverse.Create(__instance);
            if (!mTS_)
            {
                mTS_ = CustomTrainingMod.mTS_;
            }
            if (taskTraining == null)
            {
                taskTraining = rS_.GetTaskTraining();
            }
            if (GetSkillCap_Skill == 0)
            {
                GetSkillCap_Skill = traverse.Method("GetSkillCap_Skill", new Type[] { typeof(int) }).GetValue<float>(0);
            }
            if (!uiVisible)
            {
                uiVisible = traverse.Field("uiVisible").GetValue<bool>();
            }
            if (ePop_Training == 0)
            {
                ePop_Training = traverse.Field("ePop_Training").GetValue<int>();
            }
        }

        /// <summary>
        /// トレーニングの状態を確認するためのメソッド
        /// これはアイコンとかそういうのを変更するだけで、他にトレーニング関係のものがあるらしい
        /// 毎フレーム行う処理のため、パフォーマンスの問題がある可能性があります。100人前後のキャラクターでテストした感じでは、数fpsぐらいしか落ちてないので、問題ないと思いますが…。
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="rS_"></param>
        /// <param name="__result"></param>
        [HarmonyPostfix, HarmonyPatch(typeof(characterScript), "TrainingState")]
        static void CustomTrainingState(characterScript __instance, roomScript rS_, ref int __result)
        {
            if (!Main.CFG_IS_ENABLED.Value) { return; }
            //__resultが0か1のときは、何もしない
            //Debug.Log("CustomTrainingState Result is : " + __result.ToString());
            if (__result == 0 || __result == 1) { return; }

            Init(__instance, rS_);
            foreach (TrainingUIObject foundInstance in TrainingUIObject.instanceList)
            {
                if (foundInstance.MyID == taskTraining.slot)
                {
                    //Debug.Log("GetSkillCap_Skill: " + GetSkillCap_Skill);
                    //このままだと、gamedesignしかできないので、色々考える

                    //SkillIndexによっての職業をコメントで書いておく
                    //1=Game Design, 2=Programming, 3=Graphics Design, 4=Music & Sound, 5=Marketing & Support, 6=Game Testing, 7=Hardware & Engineering, 8=Research
                    switch (foundInstance.SkillIndex)
                    {
                        case 1:
                            if (__instance.s_gamedesign < mTS_.trainingMaxLearn[taskTraining.slot] && GetSkillCap_Skill > __instance.s_gamedesign)
                            {
                                __result = 1;
                                return;
                            }
                            break;
                        case 2:
                            if (__instance.s_programmieren < mTS_.trainingMaxLearn[taskTraining.slot] && GetSkillCap_Skill > __instance.s_programmieren)
                            {
                                __result = 1;
                                return;
                            }
                            break;
                        case 3:
                            if (__instance.s_grafik < mTS_.trainingMaxLearn[taskTraining.slot] && GetSkillCap_Skill > __instance.s_grafik)
                            {
                                __result = 1;
                                return;
                            }
                            break;
                        case 4:
                            if (__instance.s_sound < mTS_.trainingMaxLearn[taskTraining.slot] && GetSkillCap_Skill > __instance.s_sound)
                            {
                                __result = 1;
                                return;
                            }
                            break;
                        case 5:
                            if (__instance.s_pr < mTS_.trainingMaxLearn[taskTraining.slot] && GetSkillCap_Skill > __instance.s_pr)
                            {
                                __result = 1;
                                return;
                            }
                            break;
                        case 6:
                            if (__instance.s_gametests < mTS_.trainingMaxLearn[taskTraining.slot] && GetSkillCap_Skill > __instance.s_gametests)
                            {
                                __result = 1;
                                return;
                            }
                            break;
                        case 7:
                            if (__instance.s_technik < mTS_.trainingMaxLearn[taskTraining.slot] && GetSkillCap_Skill > __instance.s_technik)
                            {
                                __result = 1;
                                return;
                            }
                            break;
                        case 8:
                            if (__instance.s_forschen < mTS_.trainingMaxLearn[taskTraining.slot] && GetSkillCap_Skill > __instance.s_forschen)
                            {
                                __result = 1;
                                return;
                            }
                            break;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        //foreach (TrainingUIObject foundInstance in TrainingUIObject.instanceList)で、foundInstance.SkillIndexを取得
        static float GetSkillLevel(characterScript __instance, int SkillIndex)
        {
            float result = 0;
            switch (SkillIndex)
            {
                case 1:
                    result = __instance.s_gamedesign;
                    if (!learnSkillsSet[0]) { learnSkillsSet[0] = true; }
                    break;
                case 2:
                    result = __instance.s_programmieren;
                    learnSkillsSet[1] = true;
                    break;
                case 3:
                    result = __instance.s_grafik;
                    learnSkillsSet[2] = true;
                    break;
                case 4:
                    result = __instance.s_sound;
                    learnSkillsSet[3] = true;
                    break;
                case 5:
                    result = __instance.s_pr;
                    learnSkillsSet[4] = true;
                    break;
                case 6:
                    result = __instance.s_gametests;
                    learnSkillsSet[5] = true;
                    break;
                case 7:
                    result = __instance.s_technik;
                    learnSkillsSet[6] = true;
                    break;
                case 8:
                    result = __instance.s_forschen;
                    learnSkillsSet[7] = true;
                    break;
            }
            return result;
        }

        static void Init_LearnSkillsSet()
        {
            learnSkillsSet[0] = false;
            learnSkillsSet[1] = false;
            learnSkillsSet[2] = false;
            learnSkillsSet[3] = false;
            learnSkillsSet[4] = false;
            learnSkillsSet[5] = false;
            learnSkillsSet[6] = false;
            learnSkillsSet[7] = false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(characterScript), "WORK_Training")]
        static void Custom_WORK_Training(characterScript __instance, roomScript rS_, ref bool __result)
        {
            if (!Main.CFG_IS_ENABLED.Value) { return; }
            //__resultが0か1のときは、何もしない
            //Debug.Log("CustomTrainingState Result is : " + __result.ToString());
            if (!__result) { return; }

            Init(__instance, rS_);
            Init_LearnSkillsSet();

            int num = __instance.objectUsingS_.qualitaet - 1;
            bool flag = false;
            int num2 = 10 + num * 2 + mTS_.trainingEffekt[taskTraining.slot] * 10;
            foreach (TrainingUIObject foundInstance in TrainingUIObject.instanceList)
            {
                for (int i = 0; i < num2; i++)
                {
                    if (foundInstance.MyID == taskTraining.slot)
                    {
                        if(GetSkillLevel(__instance, foundInstance.SkillIndex) < mTS_.trainingMaxLearn[taskTraining.slot])
                        {
                            //Debug.Log("Efficiency is : " + num2.ToString() + " SkillIndex is : " + foundInstance.SkillIndex.ToString() + " SkillLevel is : " + GetSkillLevel(__instance, foundInstance.SkillIndex).ToString() + " MaxLearn is : " + mTS_.trainingMaxLearn[taskTraining.slot].ToString());
                            var Learn = traverse.Method("Learn", new Type[] { typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(bool), typeof(bool) })
                                        .GetValue(learnSkillsSet[0], learnSkillsSet[1], learnSkillsSet[2], learnSkillsSet[3], learnSkillsSet[4], learnSkillsSet[5], learnSkillsSet[6], learnSkillsSet[7]);
                            flag = true;
                        }
                    }
                }
            }

            if (flag)
            {
                if (!__instance.settings_.disableWorkIcons && uiVisible)
                {
                    __instance.StartCoroutine(traverse.Method("CreatePopInSeconds", new Type[] { typeof(int), typeof(float), typeof(float), typeof(int) }).GetValue<IEnumerator>(ePop_Training, 1f, 0f, 13));
                }
                taskTraining.Work(1f);
            }
            __result = true;
        }
    }
}
