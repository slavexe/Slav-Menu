using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using LemonUI;
using LemonUI.Elements;
using LemonUI.Extensions;
using LemonUI.Menus;
using LemonUI.Scaleform;
using LemonUI.TimerBars;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace Slav_Menu
{
    public class Menu : Script
    {
        ObjectPool menuPool = new ObjectPool();

        NativeMenu mainMenu = new NativeMenu("Slav Menu", "Main Menu");
        NativeMenu pedPropMenu = new NativeMenu("Slav Menu", "Ped Prop Allign Tool");
        NativeMenu musicEventMenu = new NativeMenu("Slav Menu", "Music Event Player");

        ScriptSettings slavSettings;

        #region PedPropAllignDeclarations

        bool loadModel = false;
        bool loadProp = false;
        bool softPinning = false;
        bool collision = false;
        bool isPed = false;
        bool fixedRot = true;
        bool propLoaded = false;
        bool xInvert = false;
        bool yInvert = false;
        bool zInvert = false;
        string propInput = "";
        string boneInput = "";
        string vertexInput = "";
        string clipboardText = "";
        int boneIndex = 90;
        int vertex = 2;
        float propXpos = 0f;
        float propYpos = 0f;
        float propZpos = 0f;
        float propXrot = 0f;
        float propYrot = 0f;
        float propZrot = 0f;

        Model propModel;
        Prop? prop;

        NativeMenu adjustmentMenu = new NativeMenu("Ped Prop Align Tool", "Adjust Position and Rotation")
        {
            Width = 500,
        };
        NativeItem propItem = new NativeItem("Input Prop Name");
        NativeItem boneItem = new NativeItem("Input Bone Index");
        NativeItem vertexItem = new NativeItem("Input Vertex");
        NativeCheckboxItem softPinningItem = new NativeCheckboxItem("Enable Soft Pinning")
        {
            Checked = false,
        };
        NativeCheckboxItem collisionItem = new NativeCheckboxItem("Enable Collision")
        {
            Checked = false,
        };
        NativeCheckboxItem isPedItem = new NativeCheckboxItem("Enable Is Ped")
        {
            Checked = false,
        };
        NativeCheckboxItem fixedRotItem = new NativeCheckboxItem("Enable Fixed Rotation")
        {
            Checked = true,
        };
        NativeItem addItem = new NativeItem("Add Prop");
        NativeItem removeItem = new NativeItem("Remove Prop");
        NativeCheckboxItem xinvertItem = new NativeCheckboxItem("Invert X Position")
        {
            Checked = false,
        };
        NativeSliderItem xPosSlider = new NativeSliderItem("X Position")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem xPosSliderTe = new NativeSliderItem("X Position Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem xPosSliderHu = new NativeSliderItem("X Position Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem xPosSliderTo = new NativeSliderItem("X Position Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSeparatorItem xySeperator = new NativeSeparatorItem();
        NativeCheckboxItem yinvertItem = new NativeCheckboxItem("Invert Y Position")
        {
            Checked = false,
        };
        NativeSliderItem yPosSlider = new NativeSliderItem("Y Position")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem yPosSliderTe = new NativeSliderItem("Y Position Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem yPosSliderHu = new NativeSliderItem("Y Position Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem yPosSliderTo = new NativeSliderItem("Y Position Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSeparatorItem yzSeperator = new NativeSeparatorItem();
        NativeCheckboxItem zinvertItem = new NativeCheckboxItem("Invert Z Position")
        {
            Checked = false,
        };
        NativeSliderItem zPosSlider = new NativeSliderItem("Z Position")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem zPosSliderTe = new NativeSliderItem("Z Position Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem zPosSliderHu = new NativeSliderItem("Z Position Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem zPosSliderTo = new NativeSliderItem("Z Position Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSeparatorItem zxrSeperator = new NativeSeparatorItem();
        NativeSliderItem xRotSlider = new NativeSliderItem("X Rotation")
        {
            Maximum = 360,
            Value = 0,
        };
        NativeSliderItem xRotSliderTe = new NativeSliderItem("X Rotation Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem xRotSliderHu = new NativeSliderItem("X Rotation Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem xRotSliderTo = new NativeSliderItem("X Rotation Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSeparatorItem xyrSeperator = new NativeSeparatorItem();
        NativeSliderItem yRotSlider = new NativeSliderItem("Y Rotation")
        {
            Maximum = 360,
            Value = 0,
        };
        NativeSliderItem yRotSliderTe = new NativeSliderItem("Y Rotation Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem yRotSliderHu = new NativeSliderItem("Y Rotation Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem yRotSliderTo = new NativeSliderItem("Y Rotation Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSeparatorItem yzrSeperator = new NativeSeparatorItem();
        NativeSliderItem zRotSlider = new NativeSliderItem("Z Rotation")
        {
            Maximum = 360,
            Value = 0,
        };
        NativeSliderItem zRotSliderTe = new NativeSliderItem("Z Rotation Tenth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem zRotSliderHu = new NativeSliderItem("Z Rotation Hundredth")
        {
            Maximum = 9,
            Value = 0,
        };
        NativeSliderItem zRotSliderTo = new NativeSliderItem("Z Rotation Thousandth")
        {
            Maximum = 9,
            Value = 0,
        };

        #endregion

        #region MusicEventDeclarations

        bool notifEnabled = true;
        bool currentMenuLoaded = false;

        string currentMenuName;
        string playingMusicEvent;

        Keys menuOpenKey;
        Keys clipboardKey;
        Keys stopMusicKey;

        MusicEventStorage jsonOutput;

        NativeMenu missionMenu = new NativeMenu("Music Event Player", "Missions");
        NativeMenu strangersAndFreaksMenu = new NativeMenu("Music Event Player", "Strangers And Freaks");
        NativeMenu randomEventsMenu = new NativeMenu("Music Event Player", "Random Events & Side Missions");
        NativeMenu activitiesMenu = new NativeMenu("Music Event Player", "Businesses & Activities");
        NativeMenu onlineContentMenu = new NativeMenu("Music Event Player", "Online Content");
        NativeMenu miscellaneousMenu = new NativeMenu("Music Event Player", "Miscellaneous");
        NativeMenu currentMenu = new NativeMenu("PlaceHolder");

        List<NativeMenu> missionMenuList = new List<NativeMenu>();
        List<NativeMenu> strangersAndFreaksMenuList = new List<NativeMenu>();
        List<NativeMenu> randomEventsMenuList = new List<NativeMenu>();
        List<NativeMenu> activitiesMenuList = new List<NativeMenu>();
        List<NativeMenu> onlineContentMenuList = new List<NativeMenu>();
        List<NativeMenu> miscellaneousMenuList = new List<NativeMenu>();

        NativeItem searchItem = new NativeItem("Search Music Events");
        NativeItem playingItem = new NativeItem("");

        NativeCheckboxItem disableAmbientItem = new NativeCheckboxItem("Disable Ambience");

        #endregion

        public Menu()
        {
            Tick += MenuTick;
            KeyDown += OnKeyDown;

            slavSettings = ScriptSettings.Load("scripts\\Slav_Menu_Settings.ini");

            menuPool.Add(mainMenu);
            menuPool.Add(pedPropMenu);
            menuPool.Add(musicEventMenu);

            mainMenu.AddSubMenu(pedPropMenu);
            mainMenu.AddSubMenu(musicEventMenu);

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

            xPosSlider.ValueChanged += InputXPosSlider;
            xPosSliderTe.ValueChanged += InputXPosTeSlider;
            xPosSliderHu.ValueChanged += InputXPosHuSlider;
            xPosSliderTo.ValueChanged += InputXPosToSlider;
            yPosSlider.ValueChanged += InputYPosSlider;
            yPosSliderTe.ValueChanged += InputYPosTeSlider;
            yPosSliderHu.ValueChanged += InputYPosHuSlider;
            yPosSliderTo.ValueChanged += InputYPosToSlider;
            zPosSlider.ValueChanged += InputZPosSlider;
            zPosSliderTe.ValueChanged += InputZPosTeSlider;
            zPosSliderHu.ValueChanged += InputZPosHuSlider;
            zPosSliderTo.ValueChanged += InputZPosToSlider;
            xRotSlider.ValueChanged += InputXRotSlider;
            xRotSliderTe.ValueChanged += InputXRotTeSlider;
            xRotSliderHu.ValueChanged += InputXRotHuSlider;
            xRotSliderTo.ValueChanged += InputXRotToSlider;
            yRotSlider.ValueChanged += InputYRotSlider;
            yRotSliderTe.ValueChanged += InputYRotTeSlider;
            yRotSliderHu.ValueChanged += InputYRotHuSlider;
            yRotSliderTo.ValueChanged += InputYRotToSlider;
            zRotSlider.ValueChanged += InputZRotSlider;
            zRotSliderTe.ValueChanged += InputZRotTeSlider;
            zRotSliderHu.ValueChanged += InputZRotHuSlider;
            zRotSliderTo.ValueChanged += InputZRotToSlider;

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

            xPosSlider.Title = "X Position: " + xPosSlider.Value;
            xPosSliderTe.Title = "X Position Tenth: " + xPosSliderTe.Value;
            xPosSliderHu.Title = "X Position Hundredth: " + xPosSliderHu.Value;
            xPosSliderTo.Title = "X Position Thousandth: " + xPosSliderTo.Value;
            yPosSlider.Title = "Y Position: " + yPosSlider.Value;
            yPosSliderTe.Title = "Y Position Tenth: " + yPosSliderTe.Value;
            yPosSliderHu.Title = "Y Position Hundredth: " + yPosSliderHu.Value;
            yPosSliderTo.Title = "Y Position Thousandth: " + yPosSliderTo.Value;
            zPosSlider.Title = "Z Position: " + zPosSlider.Value;
            zPosSliderTe.Title = "Z Position Tenth: " + zPosSliderTe.Value;
            zPosSliderHu.Title = "Z Position Hundredth: " + zPosSliderHu.Value;
            zPosSliderTo.Title = "Z Position Thousandth: " + zPosSliderTo.Value;
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

            clipboardText = "X Pos: " + propXpos + ", Prop Y Pos: " + propYpos + ", Prop Z Pos: " + propZpos + ", Prop X Rot: " + propXrot + ", Prop Y Rot: " + propYrot + ", Prop Z Rot: " + propZrot;
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
                NativeMenu currentMission = new NativeMenu("Music_Event_Player", mission.MissionName);
                missionMenuList.Add(currentMission);
            }
            foreach (var mission in missionMenuList)
            {
                menuPool.Add(mission);
                missionMenu.AddSubMenu(mission);
            }
            foreach (var strangerAndFreak in jsonOutput.StrangersAndFreaks)
            {
                NativeMenu currentMission = new NativeMenu("Music_Event_Player", strangerAndFreak.StrangerAndFreakName);
                strangersAndFreaksMenuList.Add(currentMission);
            }
            foreach (var strangerAndFreak in strangersAndFreaksMenuList)
            {
                menuPool.Add(strangerAndFreak);
                strangersAndFreaksMenu.AddSubMenu(strangerAndFreak);
            }
            foreach (var randomEvent in jsonOutput.RandomEvents)
            {
                NativeMenu currentRandomEvent = new NativeMenu("Music_Event_Player", randomEvent.RandomEventName);
                randomEventsMenuList.Add(currentRandomEvent);
            }
            foreach (var randomEvent in randomEventsMenuList)
            {
                menuPool.Add(randomEvent);
                randomEventsMenu.AddSubMenu(randomEvent);
            }
            foreach (var activity in jsonOutput.Activities)
            {
                NativeMenu currentActivity = new NativeMenu("Music_Event_Player", activity.ActivityName);
                activitiesMenuList.Add(currentActivity);
            }
            foreach (var activity in activitiesMenuList)
            {
                menuPool.Add(activity);
                activitiesMenu.AddSubMenu(activity);
            }
            foreach (var content in jsonOutput.OnlineContent)
            {
                NativeMenu currentContent = new NativeMenu("Music_Event_Player", content.ContentName);
                onlineContentMenuList.Add(currentContent);
            }
            foreach (var content in onlineContentMenuList)
            {
                menuPool.Add(content);
                onlineContentMenu.AddSubMenu(content);
            }
            foreach (var other in jsonOutput.Miscellaneous)
            {
                NativeMenu currentOther = new NativeMenu("Music_Event_Player", other.OtherName);
                miscellaneousMenuList.Add(currentOther);
            }
            foreach (var other in miscellaneousMenuList)
            {
                menuPool.Add(other);
                miscellaneousMenu.AddSubMenu(other);
            }
            notifEnabled = slavSettings.GetValue("General", "Notifications", true);
            menuOpenKey = (Keys)Enum.Parse(typeof(Keys), slavSettings.GetValue("General", "OpenMenuKey", "F3"), true);
            clipboardKey = (Keys)Enum.Parse(typeof(Keys), slavSettings.GetValue("General", "CopyToClipboardKey", "NumPad0"), true);
            stopMusicKey = (Keys)Enum.Parse(typeof(Keys), slavSettings.GetValue("General", "StopMusicKey", "Subtract"), true);

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
                    UpdateProp();
                }
                else
                {
                    if (loadModel)
                    {
                        while (!propModel.IsLoaded) Wait(50);
                        prop = World.CreateProp(propModel, Game.Player.Character.Position, false, false);
                        propLoaded = true;
                        loadProp = false;
                        UpdateProp();
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
                            UpdateProp();
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
            playingItem.Title = "Playing: ~g~" + playingMusicEvent;

            #endregion
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == menuOpenKey)
            {
                if (!mainMenu.Visible && !pedPropMenu.Visible && !adjustmentMenu.Visible && !musicEventMenu.Visible && !missionMenu.Visible && !strangersAndFreaksMenu.Visible && !randomEventsMenu.Visible && !activitiesMenu.Visible && !onlineContentMenu.Visible && !miscellaneousMenu.Visible)
                {
                    mainMenu.Visible = true;
                } else if (mainMenu.Visible || pedPropMenu.Visible || adjustmentMenu.Visible || musicEventMenu.Visible || missionMenu.Visible || strangersAndFreaksMenu.Visible || randomEventsMenu.Visible || activitiesMenu.Visible && onlineContentMenu.Visible || miscellaneousMenu.Visible)
                {
                    if (mainMenu.Visible)
                    {
                        mainMenu.Visible = false;
                    } else if (pedPropMenu.Visible)
                    {
                        pedPropMenu.Visible = false;
                    } else if (adjustmentMenu.Visible)
                    {
                        adjustmentMenu.Visible = false;
                    } else if (musicEventMenu.Visible)
                    {
                        musicEventMenu.Visible = false;
                    } else if (missionMenu.Visible)
                    {
                        missionMenu.Visible = false;
                    } else if (strangersAndFreaksMenu.Visible)
                    {
                        strangersAndFreaksMenu.Visible = false;
                    } else if (randomEventsMenu.Visible)
                    {
                        randomEventsMenu.Visible = false;
                    } else if (activitiesMenu.Visible)
                    {
                        activitiesMenu.Visible = false;
                    } else if (onlineContentMenu.Visible)
                    {
                        onlineContentMenu.Visible = false;
                    } else if (miscellaneousMenu.Visible)
                    {
                        miscellaneousMenu.Visible = false;
                    }
                }
            }
            if (e.KeyCode == clipboardKey)
            {
                if (clipboardText != null)
                {
                    Clipboard.setText(clipboardText);
                    if (notifEnabled)
                    {
                        Notification.Show("~g~Position and Rotation copied to clipboard");
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
            UpdateProp();
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
            UpdateProp();
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
            UpdateProp();
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
            UpdateProp();
            UpdateNames();
        }
        private void InputYInvert(object sender, EventArgs e)
        {
            yInvert = !yInvert;
            UpdateProp();
            UpdateNames();
        }
        private void InputZInvert(object sender, EventArgs e)
        {
            zInvert = !zInvert;
            UpdateProp();
            UpdateNames();
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
        private void InputXPosSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputYPosSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputZPosSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputXPosTeSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputYPosTeSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputZPosTeSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputXPosHuSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputYPosHuSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputZPosHuSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputXPosToSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputYPosToSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputZPosToSlider(object sender, EventArgs e)
        {
            UpdateProp();
            UpdateNames();
        }
        private void InputXRotSlider(object sender, EventArgs e)
        {
            UpdateProp();
            xRotSlider.Title = "X Rotation: " + xRotSlider.Value;
        }
        private void InputYRotSlider(object sender, EventArgs e)
        {
            UpdateProp();
            yRotSlider.Title = "Y Rotation: " + yRotSlider.Value;
        }
        private void InputZRotSlider(object sender, EventArgs e)
        {
            UpdateProp();
            zRotSlider.Title = "Z Rotation: " + zRotSlider.Value;
        }
        private void InputXRotTeSlider(object sender, EventArgs e)
        {
            UpdateProp();
            xRotSliderTe.Title = "X Rotation Tenth: " + xRotSliderTe.Value;
        }
        private void InputYRotTeSlider(object sender, EventArgs e)
        {
            UpdateProp();
            yRotSliderTe.Title = "Y Rotation Tenth: " + yRotSliderTe.Value;
        }
        private void InputZRotTeSlider(object sender, EventArgs e)
        {
            UpdateProp();
            zRotSliderTe.Title = "Z Rotation Tenth: " + zRotSliderTe.Value;
        }
        private void InputXRotHuSlider(object sender, EventArgs e)
        {
            UpdateProp();
            xRotSliderHu.Title = "X Rotation Hundredth: " + xRotSliderHu.Value;
        }
        private void InputYRotHuSlider(object sender, EventArgs e)
        {
            UpdateProp();
            yRotSliderHu.Title = "Y Rotation Hundredth: " + yRotSliderHu.Value;
        }
        private void InputZRotHuSlider(object sender, EventArgs e)
        {
            UpdateProp();
            zRotSliderHu.Title = "Z Rotation Hundredth: " + zRotSliderHu.Value;
        }
        private void InputXRotToSlider(object sender, EventArgs e)
        {
            UpdateProp();
            xRotSliderTo.Title = "X Rotation Thousandth: " + xRotSliderTo.Value;
        }
        private void InputYRotToSlider(object sender, EventArgs e)
        {
            UpdateProp();
            yRotSliderTo.Title = "Y Rotation Thousandth: " + yRotSliderTo.Value;
        }
        private void InputZRotToSlider(object sender, EventArgs e)
        {
            UpdateProp();
            zRotSliderTo.Title = "Z Rotation Thousandth: " + zRotSliderTo.Value;
        }
        void UpdateNames()
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
        }
        void UpdateProp()
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
            clipboardText = "X Pos: " + propXpos + ", Prop Y Pos: " + propYpos + ", Prop Z Pos: " + propZpos + ", Prop X Rot: " + propXrot + ", Prop Y Rot: " + propYrot + ", Prop Z Rot: " + propZrot;
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
    }
}