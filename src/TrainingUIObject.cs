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
    /// <summary>
    /// トレーニングのコース選択のUIObjectを複製し、各種パラメータを設定する。
    /// </summary>
    public class TrainingUIObject : MonoBehaviour
    {
        /// <summary>
        /// プログラム上で使用する固有のIDの指定します。ユーザーには見えません。
        /// Specify a unique ID to be used in the program. Not visible to the user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ユーザーに表示するトレーニングの名前を指定します。
        /// Specify the name of the training to be displayed to the user.
        /// </summary>
        public string TextName { get; set; }

        /// <summary>
        /// トレーニングのコスト（金額）を指定します。int型の最大値は2,147,483,647です。注意してください。
        /// Specify the cost of the training (in currency). Note that the maximum value for int is 2,147,483,647.
        /// </summary>
        public int Costs { get; set; }

        /// <summary>
        /// トレーニングの最大レベルを指定します。
        /// Specify the maximum level of the training.
        /// </summary>
        public float SkillCap { get; set; }

        /// <summary>
        /// トレーニングの効果を指定します。0:Low, 1:Medium, 2:High, 3:Very High, 4:Exceptionally High, 5:Legendary☆, 6:Legendary☆2
        /// Specify the effectiveness of the training. 0: Low, 1: Medium, 2: High, 3: Very High, 4: Exceptionally High, 5: Legendary☆, 6: Legendary☆2
        /// </summary>
        public int Efficiency { get; set; }

        /// <summary>
        /// トレーニングの所要時間を指定します。数値が高ければ高いほど、トレーニング全体の時間が長くなります。
        /// Specify the duration of the training. A higher value will result in a longer overall training time.
        /// </summary>
        public float WorkPoints { get; set; }

        /// <summary>
        /// uiObject用のユニークIDです。ユーザーには見えません。
        /// Unique ID for uiObject. Not visible to the user.
        /// </summary>
        public int MyID { get; set; }

        /// <summary>
        /// トレーニングのスキルを指定します。
        /// Specify the skill for the training.1:Game Design, 2:Programming, 3:Graphics Design, 4:Music & Sound, 5:Marketing & Support, 6:Game Testing, 7:Hardware & Engineering, 8:Research
        /// </summary>
        public int SkillIndex { get; set; }
        /// 参照するヒエラルキーの順番を指定します。Indexは、参照元のIndexの数値になるようにしてください。（Index + 1の位置に生成されるため）
        /// 指定しない場合は、9999、もしくはなにも指定しないでください。指定しない場合は、Skillに準じた場所に置かれます。
        /// Specify the order of the hierarchy to reference. Please make sure the Index matches the numerical value of the source of reference. (It will be generated at the position of Index + 1).
        /// If not specified, please specify 9999, or leave it blank. If left blank, it will be placed in a location corresponding to Skill.
        /// </summary>
        public int SiblingIndex { get; set; }

        public Sprite Sprite { get; set; }
        public GameObject SkillUI { get; set; }

        private static GameObject content;
        private static GameObject gameDesignOriginalUI;
        private static GameObject programmingOriginalUI;
        private static GameObject graphicDesignOriginalUI;
        private static GameObject musicSoundOriginalUI;
        private static GameObject marketingSupportOriginalUI;
        private static GameObject gameTestingOriginalUI;
        private static GameObject hardwareEngineeringOriginalUI;
        private static GameObject researchOriginalUI;

        private static Sprite[] originalSprites;
        private static Sprite gameDesignOriginalSprite;
        private static Sprite programmingOriginalSprite;
        private static Sprite graphicDesignOriginalSprite;
        private static Sprite musicSoundOriginalSprite;
        private static Sprite marketingSupportOriginalSprite;
        private static Sprite gameTestingOriginalSprite;
        private static Sprite hardwareEngineeringOriginalSprite;
        private static Sprite researchOriginalSprite;

        private int originalLastMyID;

        private static Menu_Training_Select mTS_;

        private static bool isInit = false;
        public static List<TrainingUIObject> instanceList = new List<TrainingUIObject>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">Specify the name of the training to be displayed to the user.</param>
        /// <param name="textName">Specify the name of the training to be displayed to the user.</param>
        /// <param name="skillIndex">Specify the skill for the training.1:Game Design, 2:Programming, 3:Graphics Design, 4:Music & Sound, 5:Marketing & Support, 6:Game Testing, 7:Hardware & Engineering, 8:Research. </param>
        /// <param name="costs">Specify the cost of the training (in currency). Note that the maximum value for int is 2,147,483,647.</param>
        /// <param name="skillCap">Specify the maximum level of the training.</param>
        /// <param name="efficiency">Specify the effectiveness of the training. 0: Low, 1: Medium, 2: High, 3: Very High, 4: Exceptionally High, 5: Legendary☆, 6: Legendary☆2</param>
        /// <param name="workPoints">Specify the duration of the training. A higher value will result in a longer overall training time.</param>
        /// <param name="siblingIndex">Specify the order of the hierarchy to reference. Please make sure the Index matches the numerical value of the source of reference. (It will be generated at the position of Index + 1) If not specified, please specify 9999, or leave it blank. If left blank, it will be placed in a location corresponding to Skill.</param>
        public TrainingUIObject(string id, string textName = "Training UIObject Name", int skillIndex = 0, int costs = 50000, float skillCap = 50, int efficiency = 0, float workPoints = 500f, int siblingIndex = 9999)
        {
            Init();
            Id = id;
            TextName = textName;
            Costs = costs;
            SkillCap = skillCap;
            Efficiency = efficiency;
            WorkPoints = workPoints;
            SiblingIndex = siblingIndex;
            SkillIndex = skillIndex;
            SetSkillInfo(skillIndex);
        }
        //引数空のコンストラクタ
        public TrainingUIObject()
        {
            Init();
            //ランダムな英数字生成する8桁のコード
            Id = System.Guid.NewGuid().ToString("N").Substring(0, 8);
            TextName = "TrainingUIObject_" + Id;
            Costs = 50000;
            SkillCap = 50f;
            Efficiency = 0;
            WorkPoints = 500f;
            SiblingIndex = 0;
            SetSkillInfo(0);
        }
        public static void Init()
        {
            //if (!isInit)
            //{
            Debug.Log("Init.mTS_");
            mTS_ = CustomTrainingMod.mTS_;
            Debug.Log("Init.GetOriginalUIObject");
            GetOriginalUIObject();
            Debug.Log("Init.GetOriginalUISprites");
            GetOriginalUISprites();
            //isInit = true;
            //}
        }

        public static void GetOriginalUIObject()
        {
            Debug.Log("GetOriginalUIObject.content");
            Debug.Log("CustomTrainingMod.mTS_.uiObjects.Length " + mTS_.uiObjects.Length);
            content = mTS_.uiObjects[0].gameObject;
            //content = GameObject.Find("CanvasInGameMenu/Menu_Training_Select/WindowMain/Scroll View /Viewport/Content");
            Debug.Log("GetOriginalUIObject.gameDesignOriginalUI");
            gameDesignOriginalUI = content.transform.Find("Training_Kurs_GD1").gameObject;              //Game Design
            Debug.Log("GetOriginalUIObject.programmingOriginalUI");
            programmingOriginalUI = content.transform.Find("Training_Kurs_P1").gameObject;              //Programming
            graphicDesignOriginalUI = content.transform.Find("Training_Kurs_GR1").gameObject;           //Graphics Design
            musicSoundOriginalUI = content.transform.Find("Training_Kurs_SO1").gameObject;              //Music & Sound
            marketingSupportOriginalUI = content.transform.Find("Training_Kurs_PR1").gameObject;        //Marketing & Support
            gameTestingOriginalUI = content.transform.Find("Training_Kurs_GT1").gameObject;             //Game Testing
            hardwareEngineeringOriginalUI = content.transform.Find("Training_Kurs_HW1").gameObject;     //Hardware & Engineering
            researchOriginalUI = content.transform.Find("Training_Kurs_RE1").gameObject;                //Research
        }

        private static void GetOriginalUISprites()
        {
            originalSprites = mTS_.trainingSprites;
            //originalSprites = GameObject.Find("CanvasInGameMenu/Menu_Training_Select").GetComponent<Menu_Training_Select>().trainingSprites;
            gameDesignOriginalSprite = originalSprites[0];              //Game Design
            programmingOriginalSprite = originalSprites[3];             //Programming
            graphicDesignOriginalSprite = originalSprites[6];           //Graphics Design
            musicSoundOriginalSprite = originalSprites[9];              //Music & Sound
            marketingSupportOriginalSprite = originalSprites[12];       //Marketing & Support
            gameTestingOriginalSprite = originalSprites[15];            //Game Testing
            hardwareEngineeringOriginalSprite = originalSprites[18];    //Hardware & Engineering
            researchOriginalSprite = originalSprites[21];               //Research
        }

        private int GetMyID()
        {
            originalLastMyID = mTS_.trainingCosts.Length - 1;
            //セレクトメニューのユニークID
            MyID = originalLastMyID + 1;
            Debug.Log("MyID : " + MyID.ToString());
            return MyID;
        }
        /// <summary>
        /// 既存のメニューUIからUIObjectを生成し、各種パラメータを設定する。
        /// インスタンス後に、SetUIObject()を実行することで、ゲーム内に新たなTraining用のUIObjectを生成する。
        /// Generate a UIObject from an existing menu UI and configure various parameters.
        /// After instantiation, executing SetUIObject() will create a new UIObject for Training in the game.
        /// </summary>
        /// <returns>The generated UIObject is returned as the return value. If configuration is needed, please assign it to a variable.</returns>
        public GameObject SetUIObject()
        {
            //UIObjectを複製する
            GameObject clone = Instantiate(SkillUI);
            clone.transform.SetParent(content.transform, false);
            clone.name = Id;
            SetTextName(clone);
            //clone.transform.SetSiblingIndex(4);
            if (SiblingIndex != 9999)
            {
                SetSiblingIndex(clone, SiblingIndex);
            }
            else
            {
                //特に何も指定しない。そのままの順番で生成する
            }
            clone.GetComponent<Item_Training_Kurs>().myID = GetMyID();
            ResizeArray();
            instanceList.Add(this);
            return clone;
        }

        private void SetTextName(GameObject clone)
        {
            clone.transform.Find("TextName").GetComponent<Text>().text = TextName;
            //mTS_.transform.GetChild(GetMyID()).Find("TextName").GetComponent<Text>().text;
        }
        //Array.Resizeを行って、配列のスタックを増やす
        /// <summary>
        /// ゲーム内のuiObject[]の配列をリサイズしてスタックを増やし、IDの場所を確保する
        /// また、IDの場所に、各種パラメータを代入する
        /// Resize the array of uiObject[] in the game to increase the stack and allocate a place for the ID.
        /// Additionally, assign various parameters to the ID location.
        /// </summary>
        private void ResizeArray()
        {
            Array.Resize(ref mTS_.trainingCosts, mTS_.trainingCosts.Length + 1);
            Array.Resize(ref mTS_.trainingMaxLearn, mTS_.trainingMaxLearn.Length + 1);
            Array.Resize(ref mTS_.trainingEffekt, mTS_.trainingEffekt.Length + 1);
            Array.Resize(ref mTS_.workPoints, mTS_.workPoints.Length + 1);
            Array.Resize(ref mTS_.trainingSprites, mTS_.trainingSprites.Length + 1);

            mTS_.trainingCosts[MyID] = Costs;
            mTS_.trainingMaxLearn[MyID] = SkillCap;
            mTS_.trainingEffekt[MyID] = Efficiency;
            mTS_.workPoints[MyID] = WorkPoints;
            mTS_.trainingSprites[MyID] = Sprite;
        }

        //ヒエラルキーの順番の確認
        private int GetOriginalSiblingIndex(string originalObjectName)
        {
            if (SiblingIndex != 9999)
            {
                return SiblingIndex;
            }
            int index = content.transform.Find(originalObjectName).gameObject.transform.GetSiblingIndex();
            Debug.Log("clone.transform.GetSiblingIndex() : " + index.ToString());
            return index;
        }

        private void SetSiblingIndex(GameObject clone, int level)
        {
            clone.transform.SetSiblingIndex(level + 1);
            SiblingIndex = level + 1;
        }
        //skillはそれぞれ、1=Game Design, 2=Programming, 3=Graphics Design, 4=Music & Sound, 5=Marketing & Support, 6=Game Testing, 7=Hardware & Engineering, 8=Research
        //スキルを設定する。
        //Sprite, SkillUIを設定する。
        public void SetSkillInfo(int skillIndex)
        {
            switch (skillIndex)
            {
                case 1:
                    Sprite = gameDesignOriginalSprite;
                    SkillUI = gameDesignOriginalUI;
                    SiblingIndex = GetOriginalSiblingIndex("Training_Kurs_GD3");
                    break;
                case 2:
                    Sprite = programmingOriginalSprite;
                    SkillUI = programmingOriginalUI;
                    SiblingIndex = GetOriginalSiblingIndex("Training_Kurs_P3");
                    break;
                case 3:
                    Sprite = graphicDesignOriginalSprite;
                    SkillUI = graphicDesignOriginalUI;
                    SiblingIndex = GetOriginalSiblingIndex("Training_Kurs_GR3");
                    break;
                case 4:
                    Sprite = musicSoundOriginalSprite;
                    SkillUI = musicSoundOriginalUI;
                    SiblingIndex = GetOriginalSiblingIndex("Training_Kurs_SO3");
                    break;
                case 5:
                    Sprite = marketingSupportOriginalSprite;
                    SkillUI = marketingSupportOriginalUI;
                    SiblingIndex = GetOriginalSiblingIndex("Training_Kurs_PR3");
                    break;
                case 6:
                    Sprite = gameTestingOriginalSprite;
                    SkillUI = gameTestingOriginalUI;
                    SiblingIndex = GetOriginalSiblingIndex("Training_Kurs_GT3");
                    break;
                case 7:
                    Sprite = hardwareEngineeringOriginalSprite;
                    SkillUI = hardwareEngineeringOriginalUI;
                    SiblingIndex = GetOriginalSiblingIndex("Training_Kurs_HW3");
                    break;
                case 8:
                    Sprite = researchOriginalSprite;
                    SkillUI = researchOriginalUI;
                    SiblingIndex = GetOriginalSiblingIndex("Training_Kurs_RE3");
                    break;
                default:
                    Sprite = gameDesignOriginalSprite;
                    SkillUI = gameDesignOriginalUI;
                    SiblingIndex = GetOriginalSiblingIndex("Training_Kurs_GD3");
                    break;
            }
        }
        /// <summary>
        /// よくわからなくなったので放置
        /// </summary>
        public void FindMyIDs()
        {
            TrainingUIObject foundInstance = instanceList.FirstOrDefault(item => item.MyID == MyID);

            if (foundInstance != null)
            {
                // インスタンスが見つかった場合、foundInstanceに格納されます
            }
        }
    }
}
