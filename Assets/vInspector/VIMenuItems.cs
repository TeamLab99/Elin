// #define DISABLED    // comment to enable menu items at Tools/vInspector

#if UNITY_EDITOR && !DISABLED
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static VInspector.Libs.VUtils;
using static VInspector.Libs.VGUI;

namespace VInspector
{
    class VIMenuItems
    {
        const string menuDir = "Tools/vInspector/";


        [MenuItem(menuDir + "Script component editor")]
        static void das() => ToggleDefineDisabledInScript(typeof(VIScriptComponentEditor));
        [MenuItem(menuDir + "Script component editor", true)]
        static bool dassadc() { Menu.SetChecked(menuDir + "Script component editor", !ScriptHasDefineDisabled(typeof(VIScriptComponentEditor))); return true; }


        [MenuItem(menuDir + "Scriptable object editor")]
        static void adsasddsa() => ToggleDefineDisabledInScript(typeof(VIScriptableObjectEditor));
        [MenuItem(menuDir + "Scriptable object editor", true)]
        static bool adsasdsda() { Menu.SetChecked(menuDir + "Scriptable object editor", !ScriptHasDefineDisabled(typeof(VIScriptableObjectEditor))); return true; }


        [MenuItem(menuDir + "Script asset editor")]
        static void asdsasda() => ToggleDefineDisabledInScript(typeof(VIScriptAssetEditor));
        [MenuItem(menuDir + "Script asset editor", true)]
        static bool adsassad() { Menu.SetChecked(menuDir + "Script asset editor", !ScriptHasDefineDisabled(typeof(VIScriptAssetEditor))); return true; }


        [MenuItem(menuDir + "Resettable variables")]
        static void adsdsaads() => ToggleDefineDisabledInScript(typeof(VIResettablePropDrawer));
        [MenuItem(menuDir + "Resettable variables", true)]
        static bool sadsadads() { Menu.SetChecked(menuDir + "Resettable variables", !ScriptHasDefineDisabled(typeof(VIResettablePropDrawer))); return true; }
    }
}
#endif