using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(FillDropboxFromSource), true)]
public class DataDropDownEditor : Editor
{

    FillDropboxFromSource dropdown;
    DataSource table;
    string field="";
    public void OnEnable()
    {
        dropdown = (FillDropboxFromSource)target;
    }

    void OnTableSelected(object table)
    {
        this.table = (DataSource)table;
        dropdown.data = this.table;
    }

    void OnFieldSelected(object field)
    {
        this.field = (string)field;
        dropdown.chosenField = this.field;
    }

    public override void OnInspectorGUI()
    {
        //detect when db or table changes and wipe list of fields/tables
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("DatabaseSource");
        dropdown.db = (DatabaseSource)EditorGUILayout.ObjectField(dropdown.db, typeof(DatabaseSource), false);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Table");
        //EditorGUILayout.DropdownButton("Button");
        string tableButtonLabel = "Select Table";
        if (table != null) { tableButtonLabel = table.name; }
        if (GUILayout.Button(tableButtonLabel))
        {
            // create the menu and add items to it
            GenericMenu menu = new GenericMenu();
            List<DataSource> datas = dropdown.db.tables.Values.ToList();
            foreach(DataSource d in datas)
            {
                menu.AddItem(new GUIContent(d.name), d.name == table?.name, OnTableSelected, d);
            }

            // display the menu
            menu.ShowAsContext();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Field");
        if (table!=null)
        {
            string label = "Select Field";
            if(field != "") { label = field; }
            if (GUILayout.Button(label))
            {
                // create the menu and add items to it
                GenericMenu menu = new GenericMenu();
                List<string> fields = table.getFieldSimple();
                foreach (string d in fields)
                {
                    menu.AddItem(new GUIContent(d), d== field, OnFieldSelected, d);
                }

                // display the menu
                menu.ShowAsContext();
            }
        }

        EditorGUILayout.EndHorizontal();

        DrawDefaultInspector();

        EditorUtility.SetDirty(dropdown);
    }

}
#endif