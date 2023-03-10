using System;
using UnityEditor;
using UnityEngine;
using UVNF.Entities.Containers.Variables;

namespace UVNF.Editor
{
    public class VariableManagerEditor : EditorWindow
    {
        public VariableManager Variables;

        private Vector2 scrollPosition = new Vector2();
        private int selectedIndex = -1;

        [MenuItem("UVNF/Variable Manager")]
        public static void Init()
        {
            VariableManagerEditor window = GetWindow<VariableManagerEditor>();
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal("Box");
                {
                    Variables = EditorGUILayout.ObjectField("Variable Manager", Variables, typeof(VariableManager), false) as VariableManager;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    if (Variables != null)
                    {
                        EditorUtility.SetDirty(Variables);
                        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(150f));
                        {
                            for (int i = 0; i < Variables.Variables.Count; i++)
                            {
                                GUI.SetNextControlName("ButtonFocus");
                                if (GUILayout.Button(Variables.Variables[i].VariableName))
                                {
                                    selectedIndex = i;
                                    GUI.FocusControl("ButtonFocus");
                                }
                            }

                            GUILayout.BeginHorizontal();
                            {
                                if (GUILayout.Button("+"))
                                {
                                    Variables.AddVariable();
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndScrollView();
                    }
                    GUILayout.BeginVertical();
                    {
                        if (selectedIndex > -1 && selectedIndex < Variables.Variables.Count)
                        {
                            Variables.Variables[selectedIndex].VariableName = EditorGUILayout.TextField("Variable Name", Variables.Variables[selectedIndex].VariableName);
                            Variables.Variables[selectedIndex].ValueType = (VariableTypes)EditorGUILayout.EnumPopup("Variable Type", Variables.Variables[selectedIndex].ValueType);

                            switch (Variables.Variables[selectedIndex].ValueType)
                            {
                                case VariableTypes.Number:
                                    Variables.Variables[selectedIndex].NumberValue = EditorGUILayout.FloatField("Value", Variables.Variables[selectedIndex].NumberValue);
                                    break;
                                case VariableTypes.String:
                                    Variables.Variables[selectedIndex].TextValue = EditorGUILayout.TextField("Value", Variables.Variables[selectedIndex].TextValue);
                                    break;
                                case VariableTypes.Boolean:
                                    Variables.Variables[selectedIndex].BooleanValue = Convert.ToBoolean(
                                        EditorGUILayout.Popup("Value", Convert.ToInt32(Variables.Variables[selectedIndex].BooleanValue), new string[] { "False", "True" })); break;
                            }

                            if (GUILayout.Button("Remove"))
                            {
                                Variables.Variables.RemoveAt(selectedIndex);
                                selectedIndex = -1;
                            }
                        }
                        else
                        {
                            selectedIndex = -1;
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}