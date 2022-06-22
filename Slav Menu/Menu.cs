using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Menus;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Slav_Menu
{
    public class Menu : Script
    {
        private ObjectPool menuPool = new ObjectPool();

        private NativeMenu mainMenu = new NativeMenu("Slav Menu", "Main Menu");
        private NativeMenu pedPropMenu = new NativeMenu("Slav Menu", "Ped Prop Allign Tool");
        private NativeMenu musicEventMenu = new NativeMenu("Slav Menu", "Music Event Player");
        private NativeMenu uiPosMenu = new NativeMenu("Slav Menu", "UI Position Tool");

        private ScriptSettings slavSettings;

        private Keys menuOpenKey;
        private Keys clipboardKey;

        private int gameTimeOffset;

        private string clipboardText = "";

        #region PedPropAllignDeclarations

        private bool loadModel = false;
        private bool loadProp = false;
        private bool softPinning = false;
        private bool collision = false;
        private bool isPed = false;
        private bool fixedRot = true;
        private bool propLoaded = false;
        private bool xInvert = false;
        private bool yInvert = false;
        private bool zInvert = false;
        private int boneIndex = 90;
        private int vertex = 2;
        private float propXpos = 0f;
        private float propYpos = 0f;
        private float propZpos = 0f;
        private float propXrot = 0f;
        private float propYrot = 0f;
        private float propZrot = 0f;
        private string propInput = "";
        private string boneInput = "";
        private string vertexInput = "";

        private Model propModel;

        private Prop? prop;

        private NativeMenu adjustmentMenu = new NativeMenu("Ped Prop Align Tool", "Adjust Position and Rotation")
        {
            Width = 500,
        };
        private NativeItem propItem = new NativeItem("Input Prop Name");
        private NativeItem boneItem = new NativeItem("Input Bone Index");
        private NativeItem vertexItem = new NativeItem("Input Vertex");
        private NativeCheckboxItem softPinningItem = new NativeCheckboxItem("Enable Soft Pinning")
        {
            Checked = false,
        };
        private NativeCheckboxItem collisionItem = new NativeCheckboxItem("Enable Collision")
        {
            Checked = false,
        };
        private NativeCheckboxItem isPedItem = new NativeCheckboxItem("Enable Is Ped")
        {
            Checked = false,
        };
        private NativeCheckboxItem fixedRotItem = new NativeCheckboxItem("Enable Fixed Rotation")
        {
            Checked = true,
        };
        private NativeItem addItem = new NativeItem("Add Prop");
        private NativeItem removeItem = new NativeItem("Remove Prop");
        private NativeCheckboxItem xinvertItem = new NativeCheckboxItem("Invert X Position")
        {
            Checked = false,
        };
        private NativeSliderItem xPosSlider = new NativeSliderItem("X Position")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem xPosSliderTe = new NativeSliderItem("X Position Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem xPosSliderHu = new NativeSliderItem("X Position Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem xPosSliderTo = new NativeSliderItem("X Position Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSeparatorItem xySeperator = new NativeSeparatorItem();
        private NativeCheckboxItem yinvertItem = new NativeCheckboxItem("Invert Y Position")
        {
            Checked = false,
        };
        private NativeSliderItem yPosSlider = new NativeSliderItem("Y Position")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem yPosSliderTe = new NativeSliderItem("Y Position Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem yPosSliderHu = new NativeSliderItem("Y Position Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem yPosSliderTo = new NativeSliderItem("Y Position Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSeparatorItem yzSeperator = new NativeSeparatorItem();
        private NativeCheckboxItem zinvertItem = new NativeCheckboxItem("Invert Z Position")
        {
            Checked = false,
        };
        private NativeSliderItem zPosSlider = new NativeSliderItem("Z Position")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem zPosSliderTe = new NativeSliderItem("Z Position Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem zPosSliderHu = new NativeSliderItem("Z Position Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem zPosSliderTo = new NativeSliderItem("Z Position Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSeparatorItem zxrSeperator = new NativeSeparatorItem();
        private NativeSliderItem xRotSlider = new NativeSliderItem("X Rotation")
        {
            Maximum = 360,
            Value = 0,
        };
        private NativeSliderItem xRotSliderTe = new NativeSliderItem("X Rotation Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem xRotSliderHu = new NativeSliderItem("X Rotation Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem xRotSliderTo = new NativeSliderItem("X Rotation Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSeparatorItem xyrSeperator = new NativeSeparatorItem();
        private NativeSliderItem yRotSlider = new NativeSliderItem("Y Rotation")
        {
            Maximum = 360,
            Value = 0,
        };
        private NativeSliderItem yRotSliderTe = new NativeSliderItem("Y Rotation Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem yRotSliderHu = new NativeSliderItem("Y Rotation Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem yRotSliderTo = new NativeSliderItem("Y Rotation Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSeparatorItem yzrSeperator = new NativeSeparatorItem();
        private NativeSliderItem zRotSlider = new NativeSliderItem("Z Rotation")
        {
            Maximum = 360,
            Value = 0,
        };
        private NativeSliderItem zRotSliderTe = new NativeSliderItem("Z Rotation Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem zRotSliderHu = new NativeSliderItem("Z Rotation Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        private NativeSliderItem zRotSliderTo = new NativeSliderItem("Z Rotation Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };

        #endregion

        #region MusicEventDeclarations

        private bool notifEnabled = true;
        private bool currentMenuLoaded = false;

        private string currentMenuName;
        private string playingMusicEvent;

        private Keys stopMusicKey;

        private MusicEventStorage jsonOutput;

        private NativeMenu missionMenu = new NativeMenu("Music Event Player", "Missions");
        private NativeMenu strangersAndFreaksMenu = new NativeMenu("Music Event Player", "Strangers And Freaks");
        private NativeMenu randomEventsMenu = new NativeMenu("Music Event Player", "Random Events & Side Missions");
        private NativeMenu activitiesMenu = new NativeMenu("Music Event Player", "Businesses & Activities");
        private NativeMenu onlineContentMenu = new NativeMenu("Music Event Player", "Online Content");
        private NativeMenu miscellaneousMenu = new NativeMenu("Music Event Player", "Miscellaneous");
        private NativeMenu currentMenu = new NativeMenu("PlaceHolder");

        private List<NativeMenu> missionMenuList = new List<NativeMenu>();
        private List<NativeMenu> strangersAndFreaksMenuList = new List<NativeMenu>();
        private List<NativeMenu> randomEventsMenuList = new List<NativeMenu>();
        private List<NativeMenu> activitiesMenuList = new List<NativeMenu>();
        private List<NativeMenu> onlineContentMenuList = new List<NativeMenu>();
        private List<NativeMenu> miscellaneousMenuList = new List<NativeMenu>();

        private NativeItem searchItem = new NativeItem("Search Music Events");
        private NativeItem playingItem = new NativeItem("");

        private NativeCheckboxItem disableAmbientItem = new NativeCheckboxItem("Disable Ambience");

        #endregion

        #region UIPositionDeclarations

        private static readonly string[] shapes = { "Rectangle", "Sprite" };
        private bool renderShape = false;
        private bool shapeMenuUpdated = false;
        private bool hideHudSetting = false;
        private bool hideHud = false;
        private int shapeType = 0;
        private float shapeWidth = 1f;
        private float shapeLength = 1f;
        private float shapeXpos = 0f;
        private float shapeYpos = 0f;
        private string uiTextureDict = "";
        private string uiTextureName = "";

        private NativeMenu shapeMenu = new NativeMenu("UI Position Tool", "Shape Selection");
        private NativeListItem<string> shapeTypes = new NativeListItem<string>("Shapes", shapes);
        private NativeItem uiTextureDictItem = new NativeItem("Texture Dictionary: ");
        private NativeItem uiTextureNameItem = new NativeItem("Texture Name: ");
        private NativeMenu uiAdjustmentMenu = new NativeMenu("UI Position Tool", "Adjust Position & Size")
        {
            Width = 500f
        };
        private NativeCheckboxItem uiRenderItem = new NativeCheckboxItem("Render Shape");
        private NativeSliderItem uiWidthSlider = new NativeSliderItem("Width")
        {
            Maximum = 2,
            Value = 0
        };
        private NativeSliderItem uiWidthSliderTe = new NativeSliderItem("Width Tenth")
        {
            Maximum = 9,
            Value = 1
        };
        private NativeSliderItem uiWidthSliderHu = new NativeSliderItem("Width Hundredth")
        {
            Maximum = 9,
            Value = 0
        };
        private NativeSliderItem uiWidthSliderTo = new NativeSliderItem("Width Thousandth")
        {
            Maximum = 9,
            Value = 0
        };
        private NativeSliderItem uiLengthSlider = new NativeSliderItem("Length")
        {
            Maximum = 2,
            Value = 0
        };
        private NativeSliderItem uiLengthSliderTe = new NativeSliderItem("Length Tenth")
        {
            Maximum = 9,
            Value = 1
        };
        private NativeSliderItem uiLengthSliderHu = new NativeSliderItem("Length Hundredth")
        {
            Maximum = 9,
            Value = 0
        };
        private NativeSliderItem uiLengthSliderTo = new NativeSliderItem("Length Thousandth")
        {
            Maximum = 9,
            Value = 0
        };
        private NativeSliderItem uiXposSlider = new NativeSliderItem("X Position")
        {
            Maximum = 1,
            Value = 0
        };
        private NativeSliderItem uiXposSliderTe = new NativeSliderItem("X Position Tenth")
        {
            Maximum = 9,
            Value = 0
        };
        private NativeSliderItem uiXposSliderHu = new NativeSliderItem("X Position Hundredth")
        {
            Maximum = 9,
            Value = 0
        };
        private NativeSliderItem uiXposSliderTo = new NativeSliderItem("X Position Thousandth")
        {
            Maximum = 9,
            Value = 0
        };
        private NativeSliderItem uiYposSlider = new NativeSliderItem("Y Position")
        {
            Maximum = 1,
            Value = 0
        };
        private NativeSliderItem uiYposSliderTe = new NativeSliderItem("Y Position Tenth")
        {
            Maximum = 9,
            Value = 0
        };
        private NativeSliderItem uiYposSliderHu = new NativeSliderItem("Y Position Hundredth")
        {
            Maximum = 9,
            Value = 0
        };
        private NativeSliderItem uiYposSliderTo = new NativeSliderItem("Y Position Thousandth")
        {
            Maximum = 9,
            Value = 0
        };

        #endregion

        public Menu()
        {
            Tick += MenuTick;
            KeyDown += OnKeyDown;

            slavSettings = ScriptSettings.Load("scripts\\Slav_Menu_Settings.ini");

            notifEnabled = slavSettings.GetValue("General", "Notifications", true);
            hideHudSetting = slavSettings.GetValue("General", "HideHUD", true);

            menuOpenKey = (Keys)Enum.Parse(typeof(Keys), slavSettings.GetValue("Keybinds", "OpenMenuKey", "F3"), true);
            clipboardKey = (Keys)Enum.Parse(typeof(Keys), slavSettings.GetValue("Keybinds", "CopyToClipboardKey", "NumPad0"), true);
            stopMusicKey = (Keys)Enum.Parse(typeof(Keys), slavSettings.GetValue("Keybinds", "StopMusicKey", "Subtract"), true);

            menuPool.Add(mainMenu);
            menuPool.Add(pedPropMenu);
            menuPool.Add(musicEventMenu);
            menuPool.Add(uiPosMenu);

            mainMenu.AddSubMenu(pedPropMenu);
            mainMenu.AddSubMenu(musicEventMenu);
            mainMenu.AddSubMenu(uiPosMenu);

            #region PedPropAllignInit

            propItem.Activated += InputProp;
            boneItem.Activated += InputBone;
            vertexItem.Activated += InputVertex;
            addItem.Activated += InputAdd;
            removeItem.Activated += InputRemove;

            softPinningItem.CheckboxChanged += InputSoftPinning;
            collisionItem.CheckboxChanged += InputCollision;
            isPedItem.CheckboxChanged += InputIsPed;
            fixedRotItem.CheckboxChanged += InputFixedRot;

            xinvertItem.CheckboxChanged += InputXInvert;
            yinvertItem.CheckboxChanged += InputYInvert;
            zinvertItem.CheckboxChanged += InputZInvert;

            xPosSlider.ValueChanged += UpdatePedPropEvent;
            xPosSliderTe.ValueChanged += UpdatePedPropEvent;
            xPosSliderHu.ValueChanged += UpdatePedPropEvent;
            xPosSliderTo.ValueChanged += UpdatePedPropEvent;
            yPosSlider.ValueChanged += UpdatePedPropEvent;
            yPosSliderTe.ValueChanged += UpdatePedPropEvent;
            yPosSliderHu.ValueChanged += UpdatePedPropEvent;
            yPosSliderTo.ValueChanged += UpdatePedPropEvent;
            zPosSlider.ValueChanged += UpdatePedPropEvent;
            zPosSliderTe.ValueChanged += UpdatePedPropEvent;
            zPosSliderHu.ValueChanged += UpdatePedPropEvent;
            zPosSliderTo.ValueChanged += UpdatePedPropEvent;
            xRotSlider.ValueChanged += UpdatePedPropEvent;
            xRotSliderTe.ValueChanged += UpdatePedPropEvent;
            xRotSliderHu.ValueChanged += UpdatePedPropEvent;
            xRotSliderTo.ValueChanged += UpdatePedPropEvent;
            yRotSlider.ValueChanged += UpdatePedPropEvent;
            yRotSliderTe.ValueChanged += UpdatePedPropEvent;
            yRotSliderHu.ValueChanged += UpdatePedPropEvent;
            yRotSliderTo.ValueChanged += UpdatePedPropEvent;
            zRotSlider.ValueChanged += UpdatePedPropEvent;
            zRotSliderTe.ValueChanged += UpdatePedPropEvent;
            zRotSliderHu.ValueChanged += UpdatePedPropEvent;
            zRotSliderTo.ValueChanged += UpdatePedPropEvent;

            menuPool.Add(adjustmentMenu);

            pedPropMenu.AddSubMenu(adjustmentMenu);
            pedPropMenu.Add(propItem);
            pedPropMenu.Add(boneItem);
            pedPropMenu.Add(vertexItem);
            pedPropMenu.Add(softPinningItem);
            pedPropMenu.Add(collisionItem);
            pedPropMenu.Add(isPedItem);
            pedPropMenu.Add(fixedRotItem);
            pedPropMenu.Add(addItem);
            pedPropMenu.Add(removeItem);

            adjustmentMenu.Add(xinvertItem);
            adjustmentMenu.Add(xPosSlider);
            adjustmentMenu.Add(xPosSliderTe);
            adjustmentMenu.Add(xPosSliderHu);
            adjustmentMenu.Add(xPosSliderTo);
            adjustmentMenu.Add(xySeperator);
            adjustmentMenu.Add(yinvertItem);
            adjustmentMenu.Add(yPosSlider);
            adjustmentMenu.Add(yPosSliderTe);
            adjustmentMenu.Add(yPosSliderHu);
            adjustmentMenu.Add(yPosSliderTo);
            adjustmentMenu.Add(yzSeperator);
            adjustmentMenu.Add(zinvertItem);
            adjustmentMenu.Add(zPosSlider);
            adjustmentMenu.Add(zPosSliderTe);
            adjustmentMenu.Add(zPosSliderHu);
            adjustmentMenu.Add(zPosSliderTo);
            adjustmentMenu.Add(zxrSeperator);
            adjustmentMenu.Add(xRotSlider);
            adjustmentMenu.Add(xRotSliderTe);
            adjustmentMenu.Add(xRotSliderHu);
            adjustmentMenu.Add(xRotSliderTo);
            adjustmentMenu.Add(xyrSeperator);
            adjustmentMenu.Add(yRotSlider);
            adjustmentMenu.Add(yRotSliderTe);
            adjustmentMenu.Add(yRotSliderHu);
            adjustmentMenu.Add(yRotSliderTo);
            adjustmentMenu.Add(yzrSeperator);
            adjustmentMenu.Add(zRotSlider);
            adjustmentMenu.Add(zRotSliderTe);
            adjustmentMenu.Add(zRotSliderHu);
            adjustmentMenu.Add(zRotSliderTo);

            UpdatePedProp();
            UpdatePedPropNames();
            
            #endregion

            #region MusicEventInit

            menuPool.Add(missionMenu);
            menuPool.Add(strangersAndFreaksMenu);
            menuPool.Add(randomEventsMenu);
            menuPool.Add(activitiesMenu);
            menuPool.Add(onlineContentMenu);
            menuPool.Add(miscellaneousMenu);

            musicEventMenu.AddSubMenu(missionMenu);
            musicEventMenu.AddSubMenu(strangersAndFreaksMenu);
            musicEventMenu.AddSubMenu(randomEventsMenu);
            musicEventMenu.AddSubMenu(activitiesMenu);
            musicEventMenu.AddSubMenu(onlineContentMenu);
            musicEventMenu.AddSubMenu(miscellaneousMenu);
            musicEventMenu.Add(searchItem);
            musicEventMenu.Add(disableAmbientItem);
            musicEventMenu.Add(playingItem);

            searchItem.Activated += SearchMusicEvent;
            disableAmbientItem.Activated += DisableAmbientEvent;

            using (StreamReader jsonReader = new StreamReader("scripts\\MusicEventData.json"))
            {
                string rawJson = jsonReader.ReadToEnd();
                jsonOutput = JsonConvert.DeserializeObject<MusicEventStorage>(rawJson);
            }
            foreach (var mission in jsonOutput.Missions)
            {
                NativeMenu currentMission = new NativeMenu("Music Event Player", mission.MissionName);
                missionMenuList.Add(currentMission);
            }
            foreach (var mission in missionMenuList)
            {
                menuPool.Add(mission);
                missionMenu.AddSubMenu(mission);
            }
            foreach (var strangerAndFreak in jsonOutput.StrangersAndFreaks)
            {
                NativeMenu currentMission = new NativeMenu("Music Event Player", strangerAndFreak.StrangerAndFreakName);
                strangersAndFreaksMenuList.Add(currentMission);
            }
            foreach (var strangerAndFreak in strangersAndFreaksMenuList)
            {
                menuPool.Add(strangerAndFreak);
                strangersAndFreaksMenu.AddSubMenu(strangerAndFreak);
            }
            foreach (var randomEvent in jsonOutput.RandomEvents)
            {
                NativeMenu currentRandomEvent = new NativeMenu("Music Event Player", randomEvent.RandomEventName);
                randomEventsMenuList.Add(currentRandomEvent);
            }
            foreach (var randomEvent in randomEventsMenuList)
            {
                menuPool.Add(randomEvent);
                randomEventsMenu.AddSubMenu(randomEvent);
            }
            foreach (var activity in jsonOutput.Activities)
            {
                NativeMenu currentActivity = new NativeMenu("Music Event Player", activity.ActivityName);
                activitiesMenuList.Add(currentActivity);
            }
            foreach (var activity in activitiesMenuList)
            {
                menuPool.Add(activity);
                activitiesMenu.AddSubMenu(activity);
            }
            foreach (var content in jsonOutput.OnlineContent)
            {
                NativeMenu currentContent = new NativeMenu("Music Event Player", content.ContentName);
                onlineContentMenuList.Add(currentContent);
            }
            foreach (var content in onlineContentMenuList)
            {
                menuPool.Add(content);
                onlineContentMenu.AddSubMenu(content);
            }
            foreach (var other in jsonOutput.Miscellaneous)
            {
                NativeMenu currentOther = new NativeMenu("Music Event Player", other.OtherName);
                miscellaneousMenuList.Add(currentOther);
            }
            foreach (var other in miscellaneousMenuList)
            {
                menuPool.Add(other);
                miscellaneousMenu.AddSubMenu(other);
            }

            #endregion

            #region UIPositionInit

            menuPool.Add(shapeMenu);
            menuPool.Add(uiAdjustmentMenu);

            uiPosMenu.AddSubMenu(shapeMenu);
            uiPosMenu.AddSubMenu(uiAdjustmentMenu);
            uiPosMenu.Add(uiRenderItem);

            shapeMenu.Add(shapeTypes);
            shapeMenu.Add(uiTextureDictItem);
            shapeMenu.Add(uiTextureNameItem);

            uiAdjustmentMenu.Add(uiWidthSlider);
            uiAdjustmentMenu.Add(uiWidthSliderTe);
            uiAdjustmentMenu.Add(uiWidthSliderHu);
            uiAdjustmentMenu.Add(uiWidthSliderTo);
            uiAdjustmentMenu.Add(uiLengthSlider);
            uiAdjustmentMenu.Add(uiLengthSliderTe);
            uiAdjustmentMenu.Add(uiLengthSliderHu);
            uiAdjustmentMenu.Add(uiLengthSliderTo);
            uiAdjustmentMenu.Add(uiXposSlider);
            uiAdjustmentMenu.Add(uiXposSliderTe);
            uiAdjustmentMenu.Add(uiXposSliderHu);
            uiAdjustmentMenu.Add(uiXposSliderTo);
            uiAdjustmentMenu.Add(uiYposSlider);
            uiAdjustmentMenu.Add(uiYposSliderTe);
            uiAdjustmentMenu.Add(uiYposSliderHu);
            uiAdjustmentMenu.Add(uiYposSliderTo);

            uiRenderItem.CheckboxChanged += RenderUIShape;
            shapeTypes.ItemChanged += UpdateShape;
            uiTextureDictItem.Activated += UpdateUITextureDict;
            uiTextureNameItem.Activated += UpdateUITextureName;

            uiWidthSlider.ValueChanged += UpdateUIEvent;
            uiWidthSliderTe.ValueChanged += UpdateUIEvent;
            uiWidthSliderHu.ValueChanged += UpdateUIEvent;
            uiWidthSliderTo.ValueChanged += UpdateUIEvent;
            uiLengthSlider.ValueChanged += UpdateUIEvent;
            uiLengthSliderTe.ValueChanged += UpdateUIEvent;
            uiLengthSliderHu.ValueChanged += UpdateUIEvent;
            uiLengthSliderTo.ValueChanged += UpdateUIEvent;
            uiXposSlider.ValueChanged += UpdateUIEvent;
            uiXposSliderTe.ValueChanged += UpdateUIEvent;
            uiXposSliderHu.ValueChanged += UpdateUIEvent;
            uiXposSliderTo.ValueChanged += UpdateUIEvent;
            uiYposSlider.ValueChanged += UpdateUIEvent;
            uiYposSliderTe.ValueChanged += UpdateUIEvent;
            uiYposSliderHu.ValueChanged += UpdateUIEvent;
            uiYposSliderTo.ValueChanged += UpdateUIEvent;

            UpdateUI();
            UpdateUINames();

            #endregion
        }
        private void MenuTick(object sender, EventArgs e)
        {
            menuPool.Process();

            #region PedPropAllignTick

            if (loadModel)
            {
                propModel = new Model(propInput);
                propModel.Request(5000);
                if (propModel.IsValid && propModel.IsInCdImage)
                {
                    while (!propModel.IsLoaded) Wait(50);
                    Notification.Show("~g~Successfully loaded prop model");
                }
                else
                {
                    Notification.Show("~r~Could not load prop model. Check spelling and validity of model.");
                }
                loadModel = false;
            }
            if (loadProp)
            {
                if (propModel.IsLoaded)
                {
                    prop = World.CreateProp(propModel, Game.Player.Character.Position, false, false);
                    propLoaded = true;
                    loadProp = false;
                    UpdatePedProp();
                }
                else
                {
                    if (loadModel)
                    {
                        while (!propModel.IsLoaded) Wait(50);
                        prop = World.CreateProp(propModel, Game.Player.Character.Position, false, false);
                        propLoaded = true;
                        loadProp = false;
                        UpdatePedProp();
                    }
                    else
                    {
                        if (propInput != "")
                        {
                            loadModel = true;
                            while (!propModel.IsLoaded) Wait(50);
                            prop = World.CreateProp(propModel, Game.Player.Character.Position, false, false);
                            propLoaded = true;
                            loadProp = false;
                            UpdatePedProp();
                        }
                        else
                        {
                            Notification.Show("~o~Please choose a prop before attempting to load it");
                            loadProp = false;
                        }
                    }
                }
            }
            #endregion

            #region MusicEventTick

            foreach (NativeMenu menu in missionMenuList)
            {
                if (menu.Visible)
                {
                    if (currentMenuName != null)
                    {
                        if (currentMenuName == menu.Subtitle)
                        {
                            break;
                        } else
                        {
                            currentMenuName = menu.Subtitle;
                            currentMenu = menu;
                            currentMenuLoaded = false;
                            break;
                        }
                    } else
                    {
                        currentMenuName = menu.Subtitle;
                        currentMenu = menu;
                        currentMenuLoaded = false;
                        break;
                    }
                }
            }
            foreach (NativeMenu menu in strangersAndFreaksMenuList)
            {
                if (menu.Visible)
                {
                    if (currentMenuName != null)
                    {
                        if (currentMenuName == menu.Subtitle)
                        {
                            break;
                        }
                        else
                        {
                            currentMenuName = menu.Subtitle;
                            currentMenu = menu;
                            currentMenuLoaded = false;
                            break;
                        }
                    }
                    else
                    {
                        currentMenuName = menu.Subtitle;
                        currentMenu = menu;
                        currentMenuLoaded = false;
                        break;
                    }
                }
            }
            foreach (NativeMenu menu in randomEventsMenuList)
            {
                if (menu.Visible)
                {
                    if (currentMenuName != null)
                    {
                        if (currentMenuName == menu.Subtitle)
                        {
                            break;
                        }
                        else
                        {
                            currentMenuName = menu.Subtitle;
                            currentMenu = menu;
                            currentMenuLoaded = false;
                            break;
                        }
                    }
                    else
                    {
                        currentMenuName = menu.Subtitle;
                        currentMenu = menu;
                        currentMenuLoaded = false;
                        break;
                    }
                }
            }
            foreach (NativeMenu menu in activitiesMenuList)
            {
                if (menu.Visible)
                {
                    if (currentMenuName != null)
                    {
                        if (currentMenuName == menu.Subtitle)
                        {
                            break;
                        }
                        else
                        {
                            currentMenuName = menu.Subtitle;
                            currentMenu = menu;
                            currentMenuLoaded = false;
                            break;
                        }
                    }
                    else
                    {
                        currentMenuName = menu.Subtitle;
                        currentMenu = menu;
                        currentMenuLoaded = false;
                        break;
                    }
                }
            }
            foreach (NativeMenu menu in onlineContentMenuList)
            {
                if (menu.Visible)
                {
                    if (currentMenuName != null)
                    {
                        if (currentMenuName == menu.Subtitle)
                        {
                            break;
                        }
                        else
                        {
                            currentMenuName = menu.Subtitle;
                            currentMenu = menu;
                            currentMenuLoaded = false;
                            break;
                        }
                    }
                    else
                    {
                        currentMenuName = menu.Subtitle;
                        currentMenu = menu;
                        currentMenuLoaded = false;
                        break;
                    }
                }
            }
            foreach (NativeMenu menu in miscellaneousMenuList)
            {
                if (menu.Visible)
                {
                    if (currentMenuName != null)
                    {
                        if (currentMenuName == menu.Subtitle)
                        {
                            break;
                        }
                        else
                        {
                            currentMenuName = menu.Subtitle;
                            currentMenu = menu;
                            currentMenuLoaded = false;
                            break;
                        }
                    }
                    else
                    {
                        currentMenuName = menu.Subtitle;
                        currentMenu = menu;
                        currentMenuLoaded = false;
                        break;
                    }
                }
            }
            if (!currentMenuLoaded)
            {
                if (currentMenu != null)
                {
                    currentMenu.ItemActivated += new ItemActivatedEventHandler(EventActivated);
                    foreach (var mission in jsonOutput.Missions)
                    {
                        if (currentMenu.Subtitle == mission.MissionName)
                        {
                            foreach (var musicEvent in mission.MissionEvents)
                            {
                                bool musicEventPresent = false;
                                NativeItem currentMusicEvent = new NativeItem(musicEvent.EventName);
                                foreach (NativeItem item in currentMenu.Items)
                                {
                                    if (item.Title == currentMusicEvent.Title)
                                    {
                                        musicEventPresent = true;
                                    }
                                }
                                if (!musicEventPresent)
                                {
                                    currentMenu.Add(currentMusicEvent);
                                }
                            }
                            currentMenuLoaded = true;
                        }
                    }
                    foreach (var strangerAndFreak in jsonOutput.StrangersAndFreaks)
                    {
                        if (currentMenu.Subtitle == strangerAndFreak.StrangerAndFreakName)
                        {
                            foreach (var musicEvent in strangerAndFreak.StrangerAndFreakEvents)
                            {
                                bool musicEventPresent = false;
                                NativeItem currentMusicEvent = new NativeItem(musicEvent.EventName);
                                foreach (NativeItem item in currentMenu.Items)
                                {
                                    if (item.Title == currentMusicEvent.Title)
                                    {
                                        musicEventPresent = true;
                                    }
                                }
                                if (!musicEventPresent)
                                {
                                    currentMenu.Add(currentMusicEvent);
                                }
                            }
                            currentMenuLoaded = true;
                        }
                    }
                    foreach (var randomEvent in jsonOutput.RandomEvents)
                    {
                        if (currentMenu.Subtitle == randomEvent.RandomEventName)
                        {
                            foreach (var musicEvent in randomEvent.RandomMusicEvents)
                            {
                                bool musicEventPresent = false;
                                NativeItem currentMusicEvent = new NativeItem(musicEvent.EventName);
                                foreach (NativeItem item in currentMenu.Items)
                                {
                                    if (item.Title == currentMusicEvent.Title)
                                    {
                                        musicEventPresent = true;
                                    }
                                }
                                if (!musicEventPresent)
                                {
                                    currentMenu.Add(currentMusicEvent);
                                }
                            }
                            currentMenuLoaded = true;
                        }
                    }
                    foreach (var activity in jsonOutput.Activities)
                    {
                        if (currentMenu.Subtitle == activity.ActivityName)
                        {
                            foreach (var musicEvent in activity.ActivityEvents)
                            {
                                bool musicEventPresent = false;
                                NativeItem currentMusicEvent = new NativeItem(musicEvent.EventName);
                                foreach (NativeItem item in currentMenu.Items)
                                {
                                    if (item.Title == currentMusicEvent.Title)
                                    {
                                        musicEventPresent = true;
                                    }
                                }
                                if (!musicEventPresent)
                                {
                                    currentMenu.Add(currentMusicEvent);
                                }
                            }
                            currentMenuLoaded = true;
                        }
                    }
                    foreach (var content in jsonOutput.OnlineContent)
                    {
                        if (currentMenu.Subtitle == content.ContentName)
                        {
                            foreach (var contentEvent in content.ContentEvents)
                            {
                                bool contentEventPresent = false;
                                NativeItem currentContentEvent = new NativeItem(contentEvent.EventName);
                                foreach (NativeItem item in currentMenu.Items)
                                {
                                    if (item.Title == currentContentEvent.Title)
                                    {
                                        contentEventPresent = true;
                                    }
                                }
                                if (!contentEventPresent)
                                {
                                    currentMenu.Add(currentContentEvent);
                                }
                            }
                            currentMenuLoaded = true;
                        }
                    }
                    foreach (var other in jsonOutput.Miscellaneous)
                    {
                        if (currentMenu.Subtitle == other.OtherName)
                        {
                            foreach (var otherEvent in other.OtherEvents)
                            {
                                bool otherEventPresent = false;
                                NativeItem currentOtherEvent = new NativeItem(otherEvent.EventName);
                                foreach (NativeItem item in currentMenu.Items)
                                {
                                    if (item.Title == currentOtherEvent.Title)
                                    {
                                        otherEventPresent = true;
                                    }
                                }
                                if (!otherEventPresent)
                                {
                                    currentMenu.Add(currentOtherEvent);
                                }
                            }
                            currentMenuLoaded = true;
                        }
                    }
                }
            }
            if (musicEventMenu.Visible)
            {
                playingItem.Title = "Playing: ~g~" + playingMusicEvent;
            }

            #endregion

            #region UIPositionTick

            if (!shapeMenuUpdated)
            {
                switch (shapeType)
                {
                    case 0:
                        {
                            uiTextureDictItem.Enabled = false;
                            uiTextureNameItem.Enabled = false;
                        }
                        break;
                    case 1:
                        {
                            uiTextureDictItem.Enabled = true;
                            uiTextureNameItem.Enabled = true;
                        }
                        break;
                }
                shapeMenuUpdated = true;
            }
            else if (renderShape)
            {
                switch (shapeType)
                {
                    case 0:
                        {
                            Function.Call(Hash.DRAW_RECT, shapeXpos, shapeYpos, shapeWidth, shapeLength, 255, 0, 0, 255, false);
                        }
                        break;
                    case 1:
                        {
                            Function.Call(Hash.DRAW_SPRITE, uiTextureDict, uiTextureName, shapeXpos, shapeYpos, shapeWidth, shapeLength, 0f, 255, 0, 0, 255, false, 0);
                        }
                        break;
                }
            }
            if (hideHud)
            {
                Function.Call(Hash.DISPLAY_RADAR, false);
                Function.Call(Hash.DISPLAY_HUD, false);
            }

            #endregion
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == menuOpenKey)
            {
                if (!mainMenu.Visible &&
                    !pedPropMenu.Visible &&
                    !adjustmentMenu.Visible &&
                    !musicEventMenu.Visible &&
                    !missionMenu.Visible &&
                    !strangersAndFreaksMenu.Visible &&
                    !randomEventsMenu.Visible &&
                    !activitiesMenu.Visible &&
                    !onlineContentMenu.Visible &&
                    !miscellaneousMenu.Visible &&
                    !uiPosMenu.Visible &&
                    !shapeMenu.Visible &&
                    !uiAdjustmentMenu.Visible)
                {
                    mainMenu.Visible = true;
                } 
                else if (mainMenu.Visible ||
                    pedPropMenu.Visible ||
                    adjustmentMenu.Visible ||
                    musicEventMenu.Visible ||
                    missionMenu.Visible ||
                    strangersAndFreaksMenu.Visible ||
                    randomEventsMenu.Visible ||
                    activitiesMenu.Visible ||
                    onlineContentMenu.Visible ||
                    miscellaneousMenu.Visible ||
                    uiPosMenu.Visible ||
                    shapeMenu.Visible ||
                    uiAdjustmentMenu.Visible)
                {
                    if (mainMenu.Visible) mainMenu.Visible = false;
                    else if (pedPropMenu.Visible) pedPropMenu.Visible = false;
                    else if (adjustmentMenu.Visible) adjustmentMenu.Visible = false;
                    else if (musicEventMenu.Visible) musicEventMenu.Visible = false;
                    else if (missionMenu.Visible) missionMenu.Visible = false;
                    else if (strangersAndFreaksMenu.Visible) strangersAndFreaksMenu.Visible = false;
                    else if (randomEventsMenu.Visible) randomEventsMenu.Visible = false;
                    else if (activitiesMenu.Visible) activitiesMenu.Visible = false;
                    else if (onlineContentMenu.Visible) onlineContentMenu.Visible = false;
                    else if (miscellaneousMenu.Visible) miscellaneousMenu.Visible = false;
                    else if (uiPosMenu.Visible) uiPosMenu.Visible = false;
                    else if (shapeMenu.Visible) shapeMenu.Visible = false;
                    else if (uiAdjustmentMenu.Visible) uiAdjustmentMenu.Visible = false;
                }
            }
            if (e.KeyCode == clipboardKey)
            {
                if (clipboardText != null)
                {
                    Clipboard.setText(clipboardText);
                    if (notifEnabled)
                    {
                        Notification.Show("~g~Values copied to clipboard");
                    }
                }
                else
                {
                    if (notifEnabled)
                    {
                        Notification.Show("~r~Clipboard text does not exist");
                    }
                }
            }
            if (e.KeyCode == stopMusicKey)
            {
                if (notifEnabled)
                {
                    Function.Call(Hash.TRIGGER_MUSIC_EVENT, "GLOBAL_KILL_MUSIC");
                    Notification.Show("Stopping \"~g~" + playingMusicEvent + "~s~\"");
                    playingMusicEvent = "";
                }
            }
        }

        #region PedPropAllignFunc

        private void InputProp(object sender, EventArgs e)
        {
            propInput = Game.GetUserInput();
            if (propInput.Contains("\""))
            {
                for (int i = 0; i < propInput.Length; i++)
                {
                    if (propInput[i] == '"')
                    {
                        propInput.Remove(i);
                    }
                }
            }
            Notification.Show("Prop ~g~" + propInput + " ~s~Selected");
            loadModel = true;
            UpdatePedProp();
        }
        private void InputBone(object sender, EventArgs e)
        {
            boneInput = Game.GetUserInput();
            if (int.TryParse(boneInput, out boneIndex))
            {
                Notification.Show("Bone Index ~g~" + boneIndex + " ~s~Selected");
            }
            else
            {
                Notification.Show("~r~Only integer values allowed");
            }
            UpdatePedProp();
        }
        private void InputVertex(object sender, EventArgs e)
        {
            vertexInput = Game.GetUserInput();
            if (int.TryParse(vertexInput, out vertex))
            {
                Notification.Show("Vertex ~g~" + vertex + " ~s~Selected");
            }
            else
            {
                Notification.Show("~r~Only integer values allowed");
            }
            UpdatePedProp();
        }
        private void InputSoftPinning(object sender, EventArgs e)
        {
            softPinning = softPinningItem.Checked;
        }
        private void InputCollision(object sender, EventArgs e)
        {
            collision = collisionItem.Checked;
        }
        private void InputIsPed(object sender, EventArgs e)
        {
            isPed = isPedItem.Checked;
        }
        private void InputFixedRot(object sender, EventArgs e)
        {
            fixedRot = fixedRotItem.Checked;
        }
        private void InputXInvert(object sender, EventArgs e)
        {
            xInvert = !xInvert;
            UpdatePedProp();
            UpdatePedPropNames();
        }
        private void InputYInvert(object sender, EventArgs e)
        {
            yInvert = !yInvert;
            UpdatePedProp();
            UpdatePedPropNames();
        }
        private void InputZInvert(object sender, EventArgs e)
        {
            zInvert = !zInvert;
            UpdatePedProp();
            UpdatePedPropNames();
        }
        private void InputAdd(object sender, EventArgs e)
        {
            if (prop == null)
            {
                loadProp = true;
            }
            else if (prop != null && !propLoaded)
            {
                loadProp = true;
            }
            else
            {
                Notification.Show("~o~Prop already loaded");
            }
        }
        private void InputRemove(object sender, EventArgs e)
        {
            if (prop != null)
            {
                prop.Detach();
                prop.Delete();
                propLoaded = false;
            }
            else
            {
                Notification.Show("~o~Prop not loaded, nothing to delete");
            }
        }
        private void UpdatePedPropEvent(object sender, EventArgs e)
        {
            UpdatePedProp();
            UpdatePedPropNames();
        }
        private void UpdatePedPropNames()
        {
            if (xInvert)
            {
                xPosSlider.Title = "X Position: " + -xPosSlider.Value;
                xPosSliderTe.Title = "X Position Tenth: " + -xPosSliderTe.Value;
                xPosSliderHu.Title = "X Position Hundredth: " + -xPosSliderHu.Value;
                xPosSliderTo.Title = "X Position Thousandth: " + -xPosSliderTo.Value;
            }
            else
            {
                xPosSlider.Title = "X Position: " + xPosSlider.Value;
                xPosSliderTe.Title = "X Position Tenth: " + xPosSliderTe.Value;
                xPosSliderHu.Title = "X Position Hundredth: " + xPosSliderHu.Value;
                xPosSliderTo.Title = "X Position Thousandth: " + xPosSliderTo.Value;
            }
            if (yInvert)
            {
                yPosSlider.Title = "Y Position: " + -yPosSlider.Value;
                yPosSliderTe.Title = "Y Position Tenth: " + -yPosSliderTe.Value;
                yPosSliderHu.Title = "Y Position Hundredth: " + -yPosSliderHu.Value;
                yPosSliderTo.Title = "Y Position Thousandth: " + -yPosSliderTo.Value;
            }
            else
            {
                yPosSlider.Title = "Y Position: " + yPosSlider.Value;
                yPosSliderTe.Title = "Y Position Tenth: " + yPosSliderTe.Value;
                yPosSliderHu.Title = "Y Position Hundredth: " + yPosSliderHu.Value;
                yPosSliderTo.Title = "Y Position Thousandth: " + yPosSliderTo.Value;
            }
            if (zInvert)
            {
                zPosSlider.Title = "Z Position: " + -zPosSlider.Value;
                zPosSliderTe.Title = "Z Position Tenth: " + -zPosSliderTe.Value;
                zPosSliderHu.Title = "Z Position Hundredth: " + -zPosSliderHu.Value;
                zPosSliderTo.Title = "Z Position Thousandth: " + -zPosSliderTo.Value;
            }
            else
            {
                zPosSlider.Title = "Z Position: " + zPosSlider.Value;
                zPosSliderTe.Title = "Z Position Tenth: " + zPosSliderTe.Value;
                zPosSliderHu.Title = "Z Position Hundredth: " + zPosSliderHu.Value;
                zPosSliderTo.Title = "Z Position Thousandth: " + zPosSliderTo.Value;
            }
            xRotSlider.Title = "X Rotation: " + xRotSlider.Value;
            xRotSliderTe.Title = "X Rotation Tenth: " + xRotSliderTe.Value;
            xRotSliderHu.Title = "X Rotation Hundredth: " + xRotSliderHu.Value;
            xRotSliderTo.Title = "X Rotation Thousandth: " + xRotSliderTo.Value;
            yRotSlider.Title = "Y Rotation: " + yRotSlider.Value;
            yRotSliderTe.Title = "Y Rotation Tenth: " + yRotSliderTe.Value;
            yRotSliderHu.Title = "Y Rotation Hundredth: " + yRotSliderHu.Value;
            yRotSliderTo.Title = "Y Rotation Thousandth: " + yRotSliderTo.Value;
            zRotSlider.Title = "Z Rotation: " + zRotSlider.Value;
            zRotSliderTe.Title = "Z Rotation Tenth: " + zRotSliderTe.Value;
            zRotSliderHu.Title = "Z Rotation Hundredth: " + zRotSliderHu.Value;
            zRotSliderTo.Title = "Z Rotation Thousandth: " + zRotSliderTo.Value;
        }
        private void UpdatePedProp()
        {
            propXpos = xPosSlider.Value + (xPosSliderTe.Value * 0.1f) + (xPosSliderHu.Value * 0.01f) + (xPosSliderTo.Value * 0.001f);
            propYpos = yPosSlider.Value + (yPosSliderTe.Value * 0.1f) + (yPosSliderHu.Value * 0.01f) + (yPosSliderTo.Value * 0.001f);
            propZpos = zPosSlider.Value + (zPosSliderTe.Value * 0.1f) + (zPosSliderHu.Value * 0.01f) + (zPosSliderTo.Value * 0.001f);
            propXrot = xRotSlider.Value + (xRotSliderTe.Value * 0.1f) + (xRotSliderHu.Value * 0.01f) + (xRotSliderTo.Value * 0.001f);
            propYrot = yRotSlider.Value + (yRotSliderTe.Value * 0.1f) + (yRotSliderHu.Value * 0.01f) + (yRotSliderTo.Value * 0.001f);
            propZrot = zRotSlider.Value + (zRotSliderTe.Value * 0.1f) + (zRotSliderHu.Value * 0.01f) + (zRotSliderTo.Value * 0.001f);
            if (xInvert)
            {
                propXpos = -propXpos;
            }
            if (yInvert)
            {
                propYpos = -propYpos;
            }
            if (zInvert)
            {
                propZpos = -propZpos;
            }
            clipboardText = "Prop X Pos: " + propXpos + ", Prop Y Pos: " + propYpos + ", Prop Z Pos: " + propZpos + ", Prop X Rot: " + propXrot + ", Prop Y Rot: " + propYrot + ", Prop Z Rot: " + propZrot;
            if (propLoaded)
            {
                Function.Call(Hash.ATTACH_ENTITY_TO_ENTITY, prop, Game.Player.Character, boneIndex, propXpos, propYpos, propZpos, propXrot, propYrot, propZrot, 0, softPinning, collision, isPed, vertex, fixedRot);
            }
        }

        #endregion

        #region MusicEventFunc
       
        private void EventActivated(object sender, ItemActivatedArgs e)
        {
            foreach (var mission in jsonOutput.Missions)
            {
                if (mission.MissionName == currentMenuName)
                {
                    foreach (var musicEvent in mission.MissionEvents)
                    {
                        if (e.Item.Title == musicEvent.EventName)
                        {
                            PlayMusicEvent(mission.MissionName, musicEvent.EventName, musicEvent.EventHash);
                        }
                    }
                    break;
                }
            }
            foreach (var strangerAndFreak in jsonOutput.StrangersAndFreaks)
            {
                if (strangerAndFreak.StrangerAndFreakName == currentMenuName)
                {
                    foreach (var musicEvent in strangerAndFreak.StrangerAndFreakEvents)
                    {
                        if (e.Item.Title == musicEvent.EventName)
                        {
                            PlayMusicEvent(strangerAndFreak.StrangerAndFreakName, musicEvent.EventName, musicEvent.EventHash);
                        }
                    }
                    break;
                }
            }
            foreach (var randomEvent in jsonOutput.RandomEvents)
            {
                if (randomEvent.RandomEventName == currentMenuName)
                {
                    foreach (var musicEvent in randomEvent.RandomMusicEvents)
                    {
                        if (e.Item.Title == musicEvent.EventName)
                        {
                            PlayMusicEvent(randomEvent.RandomEventName, musicEvent.EventName, musicEvent.EventHash);
                        }
                    }
                    break;
                }
            }
            foreach (var activity in jsonOutput.Activities)
            {
                if (activity.ActivityName == currentMenuName)
                {
                    foreach (var musicEvent in activity.ActivityEvents)
                    {
                        if (e.Item.Title == musicEvent.EventName)
                        {
                            PlayMusicEvent(activity.ActivityName, musicEvent.EventName, musicEvent.EventHash);
                        }
                    }
                    break;
                }
            }
            foreach (var content in jsonOutput.OnlineContent)
            {
                if (content.ContentName == currentMenuName)
                {
                    foreach (var musicEvent in content.ContentEvents)
                    {
                        if (e.Item.Title == musicEvent.EventName)
                        {
                            PlayMusicEvent(content.ContentName, musicEvent.EventName, musicEvent.EventHash);
                        }
                    }
                    break;
                }
            }
            foreach (var other in jsonOutput.Miscellaneous)
            {
                if (other.OtherName == currentMenuName)
                {
                    foreach (var musicEvent in other.OtherEvents)
                    {
                        if (e.Item.Title == musicEvent.EventName)
                        {
                            PlayMusicEvent(other.OtherName, musicEvent.EventName, musicEvent.EventHash);
                        }
                    }
                    break;
                }
            }
        }
        private void PlayMusicEvent(string missionName, string eventName, string eventHash)
        {
            if (notifEnabled)
            {
                Notification.Show("Playing ~g~" + eventName + " ~s~(~y~" + eventHash + "~s~) from ~o~" + missionName + "~s~.");
            }
            playingMusicEvent = eventHash;
            Function.Call(Hash.TRIGGER_MUSIC_EVENT, eventHash);
        }
        private void SearchMusicEvent(object sender, EventArgs e)
        {
            string userInput = Game.GetUserInput();
            bool musicEventFound = false;
            foreach (var mission in jsonOutput.Missions)
            {
                foreach (var musicEvent in mission.MissionEvents)
                {
                    if (musicEvent.EventHash == userInput)
                    {
                        PlayMusicEvent(mission.MissionName, musicEvent.EventName, musicEvent.EventHash);
                        musicEventFound = true;
                        break;
                    }
                }
            }
            foreach (var strangerAndFreak in jsonOutput.StrangersAndFreaks)
            {
                foreach (var musicEvent in strangerAndFreak.StrangerAndFreakEvents)
                {
                    if (musicEvent.EventHash == userInput)
                    {
                        PlayMusicEvent(strangerAndFreak.StrangerAndFreakName, musicEvent.EventName, musicEvent.EventHash);
                        musicEventFound = true;
                        break;
                    }
                }
            }
            foreach (var randomEvent in jsonOutput.RandomEvents)
            {
                foreach (var musicEvent in randomEvent.RandomMusicEvents)
                {
                    if (musicEvent.EventHash == userInput)
                    {
                        PlayMusicEvent(randomEvent.RandomEventName, musicEvent.EventName, musicEvent.EventHash);
                        musicEventFound = true;
                        break;
                    }
                }
            }
            foreach (var activity in jsonOutput.Activities)
            {
                foreach (var musicEvent in activity.ActivityEvents)
                {
                    if (musicEvent.EventHash == userInput)
                    {
                        PlayMusicEvent(activity.ActivityName, musicEvent.EventName, musicEvent.EventHash);
                        musicEventFound = true;
                        break;
                    }
                }
            }
            foreach (var content in jsonOutput.OnlineContent)
            {
                foreach (var musicEvent in content.ContentEvents)
                {
                    if (musicEvent.EventHash == userInput)
                    {
                        PlayMusicEvent(content.ContentName, musicEvent.EventName, musicEvent.EventHash);
                        musicEventFound = true;
                        break;
                    }
                }
            }
            foreach (var other in jsonOutput.Miscellaneous)
            {
                foreach (var musicEvent in other.OtherEvents)
                {
                    if (musicEvent.EventHash == userInput)
                    {
                        PlayMusicEvent(other.OtherName, musicEvent.EventName, musicEvent.EventHash);
                        musicEventFound = true;
                        break;
                    }
                }
            }
            if (!musicEventFound)
            {
                if (notifEnabled)
                {
                    Notification.Show("No music event with the hash \"~g~" + userInput + "~s~\" found");
                }
            }
        }
        private void DisableAmbientEvent(object sender, EventArgs e)
        {
            if (disableAmbientItem.Checked)
            {
                Function.Call(Hash.START_AUDIO_SCENE, "END_CREDITS_SCENE");
            }
            else
            {
                Function.Call(Hash.STOP_AUDIO_SCENE, "END_CREDITS_SCENE");
            }
        }

        #endregion

        #region UIPositionFunc

        private void RenderUIShape(object sender, EventArgs e)
        {
            renderShape = uiRenderItem.Checked;
            if (hideHudSetting)
            {
                if (uiRenderItem.Checked)
                {
                    hideHud = true;
                }
                else
                {
                    hideHud = false;
                    Function.Call(Hash.DISPLAY_RADAR, true);
                    Function.Call(Hash.DISPLAY_HUD, true);
                }
            }
        }
        private void UpdateShape(object sender, ItemChangedEventArgs<string> e)
        {
            if (shapeType != e.Index)
            {
                shapeType = e.Index;
                shapeMenuUpdated = false;
            }
        }
        private void UpdateUIEvent(object sender, EventArgs e)
        {
            UpdateUI();
            UpdateUINames();
        }
        private void UpdateUITextureDict(object sender, EventArgs e)
        {
            if ((uiTextureDict != null) && (uiTextureDict != ""))
            {
                Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, uiTextureDict);
            }
            uiTextureDict = Game.GetUserInput();
            Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, uiTextureDict, false);
            if (notifEnabled)
            {
                gameTimeOffset = Game.GameTime + 5000;
                while (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, uiTextureDict))
                {
                    if (Game.GameTime > gameTimeOffset)
                    {
                        uiTextureDictItem.Title = "Texture Dictionary: ";
                        Notification.Show("~r~" + uiTextureDict + " ~s~could not be loaded. It may be invalid.");
                        break;
                    }
                    Wait(10);
                }
                if (Game.GameTime < gameTimeOffset)
                {
                    uiTextureDictItem.Title = "Texture Dictionary: " + uiTextureDict;
                    Notification.Show("~g~" + uiTextureDict + " ~s~was successfully loaded.");
                }
            }
        }
        private void UpdateUITextureName(object sender, EventArgs e)
        {
            uiTextureName = Game.GetUserInput();
        }
        private void UpdateUINames()
        {
            uiWidthSlider.Title = "Width: " + uiWidthSlider.Value;
            uiWidthSliderTe.Title = "Width Tenth: " + uiWidthSliderTe.Value;
            uiWidthSliderHu.Title = "Width Hundredth: " + uiWidthSliderHu.Value;
            uiWidthSliderTo.Title = "Width Thousandth: " + uiWidthSliderTo.Value;
            uiLengthSlider.Title = "Length: " + uiLengthSlider.Value;
            uiLengthSliderTe.Title = "Length Tenth: " + uiLengthSliderTe.Value;
            uiLengthSliderHu.Title = "Length Hundredth: " + uiLengthSliderHu.Value;
            uiLengthSliderTo.Title = "Length Thousandth: " + uiLengthSliderTo.Value;
            uiXposSlider.Title = "X Position: " + uiXposSlider.Value;
            uiXposSliderTe.Title = "X Position Tenth: " + uiXposSliderTe.Value;
            uiXposSliderHu.Title = "X Position Hundredth: " + uiXposSliderHu.Value;
            uiXposSliderTo.Title = "X Position Thousandth: " + uiXposSliderTo.Value;
            uiYposSlider.Title = "Y Position: " + uiYposSlider.Value;
            uiYposSliderTe.Title = "Y Position Tenth: " + uiYposSliderTe.Value;
            uiYposSliderHu.Title = "Y Position Hundredth: " + uiYposSliderHu.Value;
            uiYposSliderTo.Title = "Y Position Thousandth: " + uiYposSliderTo.Value;
        }
        private void UpdateUI()
        {
            shapeXpos = uiXposSlider.Value + (uiXposSliderTe.Value * 0.1f) + (uiXposSliderHu.Value * 0.01f) + (uiXposSliderTo.Value * 0.001f);
            shapeYpos = uiYposSlider.Value + (uiYposSliderTe.Value * 0.1f) + (uiYposSliderHu.Value * 0.01f) + (uiYposSliderTo.Value * 0.001f);
            shapeWidth = uiWidthSlider.Value + (uiWidthSliderTe.Value * 0.1f) + (uiWidthSliderHu.Value * 0.01f) + (uiWidthSliderTo.Value * 0.001f);
            shapeLength = uiLengthSlider.Value + (uiLengthSliderTe.Value * 0.1f) + (uiLengthSliderHu.Value * 0.01f) + (uiLengthSliderTo.Value * 0.001f);
            clipboardText = "UI Width: " + shapeWidth + ", UI Length: " + shapeLength + ", UI X Pos: " + shapeXpos + ", UI Y Pos: " + shapeYpos;
        }

        #endregion
    }
}