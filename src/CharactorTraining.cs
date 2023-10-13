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

namespace EnhancedTraining
{
    public class CharactorTraining : MonoBehaviour
    {
        //private static roomScript rS_;
        private static Menu_Training_Select mTS_;
        private static taskTraining taskTraining = null;
        static Traverse traverse;
        static float GetSkillCap_Skill = 0;

        static void Init(characterScript __instance, roomScript rS_)
        {
            if (!mTS_)
            {
                mTS_ = CustomTrainingMod.mTS_;
            }
            if (traverse == null)
            {
                traverse = Traverse.Create(__instance);
            }
            if (taskTraining == null)
            {
                taskTraining = rS_.GetTaskTraining();
            }
            if(GetSkillCap_Skill == 0)
            {
                GetSkillCap_Skill = traverse.Method("GetSkillCap_Skill", new Type[] { typeof(int) }).GetValue<float>(0);
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
            if(__result == 0 || __result == 1) { return; }


            Init(__instance, rS_);
            foreach (TrainingUIObject foundInstance in TrainingUIObject.instanceList)
            {
                if (foundInstance.MyID == taskTraining.slot)
                {
                    //Debug.Log("GetSkillCap_Skill: " + GetSkillCap_Skill);
                    //このままだと、gamedesignしかできないので、色々考える

                    //SkillIndexによっての職業をコメントで書いておく
                    //1=Game Design, 2=Programming, 3=Graphics Design, 4=Music & Sound, 5=Marketing & Support, 6=Game Testing, 7=Hardware & Engineering, 8=Research
                    switch (foundInstance.SkillIndex){
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


        /*
        /// <summary>
        /// トレーニングの状態を確認するためのメソッド
        /// これはアイコンとかそういうのを変更するだけで、他にトレーニング関係のものがあるらしい
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="rS_"></param>
        /// <param name="__result"></param>
        [HarmonyPostfix, HarmonyPatch(typeof(characterScript), "TrainingState")]
        static void CustomTrainingState(characterScript __instance, roomScript rS_, ref int __result)
        {
            if (!Main.CFG_IS_ENABLED.Value) { return; }
            Init(__instance);

            if (!rS_)
            {
                __result = 0;
                return;
            }
            if (rS_.typ != 13)
            {
                __result = 0;
                return;
            }
            if (!rS_.taskGameObject)
            {
                __result = 0;
                return;
            }
            if (!__instance.guiMain_)
            {
                __result = 0;
                return;
            }
            if (taskTraining == null)
            {
                taskTraining = rS_.GetTaskTraining();
            }
            if (!taskTraining)
            {
                __result = 0;
                return;
            }

            /*
            foreach (TrainingUIObject foundInstance in TrainingUIObject.instanceList)
            {
                if (foundInstance.MyID == taskTraining.slot)
                {
                    if (traverse == null)
                    {
                        traverse = Traverse.Create(__instance);
                    }
                    //このままだと、gamedesignしかできないので、色々考える
                        GetSkillCap_Skill = traverse.Method("GetSkillCap_Skill", new Type[] { typeof(int) }).GetValue<float>(0);
                    //Debug.Log("GetSkillCap_Skill: " + GetSkillCap_Skill);
                    if (__instance.s_gamedesign < mTS_.trainingMaxLearn[taskTraining.slot] && GetSkillCap_Skill > __instance.s_gamedesign)
                    {
                        __result = 1;
                    }
                }
                else
                {
                }
            }
            
            if (traverse == null)
            {
                traverse = Traverse.Create(__instance);
            }
            if (GetSkillCap_Skill == 0)
            {
                GetSkillCap_Skill = traverse.Method("GetSkillCap_Skill", new Type[] { typeof(int) }).GetValue<float>(0);
            }
            switch (taskTraining.slot)
            {
                case 24:
                    if (__instance.s_gamedesign < mTS_.trainingMaxLearn[taskTraining.slot] && GetSkillCap_Skill > __instance.s_gamedesign)
                    {
                        __result = 1;
                    }
                    break;
                case 25:
                    if (__instance.s_gamedesign < mTS_.trainingMaxLearn[taskTraining.slot] && GetSkillCap_Skill > __instance.s_gamedesign)
                    {
                        __result = 1;
                    }
                    break;
                default:
                    break;
            }
        }
        */

    }
}
