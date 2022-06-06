# Slav-Menu
Slav Menu is an open source modding tool designed for GTA V modders with features that aid in development.
## Music Event Player
A comprehensive music event player that efficiently sorts and plays all 1,500+ music events found in GTA V. 
They are sorted alphabetically based on the mission they are from as well as chronologically within that mission.
The music events are categorized as follows:
- **Missions** 
- **Strangers And Freaks**
- **Random Events & Side Missions**
- **Businesses & Activities**
- **Online Content**
- **Miscellaneous**
## Ped Prop Allign Tool
A tool that can attach any prop to any ped and adjust the position and rotation of that prop relative to the ped bone it was mounted to.
This can be used for many things, for example perfectly aligning a drill into a ped's hand to make it look as if they are holding it.
When a desired position and rotation are selected one can effortlessly copy them to their clipboard and use them in their projects.
## Requirements 
Below are the requirements for Slav Menu to work:
- [Script Hook V](http://www.dev-c.com/gtav/scripthookv/)
- [Script Hook V .NET](https://github.com/crosire/scripthookvdotnet)
- [LemonUI](https://github.com/justalemon/LemonUI)
- [Json.NET](https://github.com/JamesNK/Newtonsoft.Json)
## Installation
- Download and install the latest versions of Script Hook V, Script Hook V .NET, and LemonUI  
- Download the latest version of Json.NET and place **Newtonsoft.Json.dll** as well as **Newtonsoft.Json.xml** in your scripts folder  
- Download the latest versions of **Slav Menu.dll**, **MusicEventData.json**, and **Slav_Menu_Settings.ini** from [Releases](https://github.com/slavexe/Slav-Menu/releases) and place them in your scripts folder  
- Any .pdb files are optional and are only used for giving error reports if a script were to crash
## Information
- Most music events are sourced from [GTA V Data Dumps](https://github.com/DurtyFree/gta-v-data-dumps), however some music events are not found in the music event data dump because Rockstar implemented them in a way that causes difficulty in automatically detecting them   
- Mission names, Strangers and Freaks names, DLC names, etc. can be looked up on [GTA Base](https://www.gtabase.com/)  
- Ped bone indices for use in Ped Prop Align Tool can be found on the [RAGE Multiplayer Wiki](https://wiki.rage.mp/index.php?title=Bones)  
## Contributing
Anyone is welcome to submit pull requests to refine music event names or to add new music events.
The menu will automatically create submenus and items for any additional music events, therefore all that needs to be modified is **MusicEventData.json**.
