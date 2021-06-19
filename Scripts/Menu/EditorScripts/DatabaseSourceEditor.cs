using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(DatabaseSource), true)]
public class DatabaseSourceEditor : Editor
{

    DatabaseSource dbsource;
    static bool showTileEditor = false;

    public void OnEnable()
    {
        dbsource = (DatabaseSource)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //MAP DEFAULT INFORMATION
        //dbsource.name = EditorGUILayout.TextField("Test", dbsource.name);
        List<string> tables = dbsource.getTables();
        string tableList = "";
        int numTables = tables?.Count ?? 0;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("#Tables", numTables.ToString());
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Status: ",dbsource.loadStatus);
        foreach (string t in tables)
        {
            tableList += t + ",";
        }
        tableList = tableList.TrimEnd(',');
        EditorGUILayout.LabelField("Tables:" ,tableList);
        //WIDTH - HEIGHT
        //int width = EditorGUILayout.IntField("Map Sprite Width", comp.mapSprites.GetLength(0));
        //int height = EditorGUILayout.IntField("Map Sprite Height", comp.mapSprites.GetLength(1));

        /*if (width != comp.mapSprites.GetLength(0) || height != comp.mapSprites.GetLength(1))
        {
            comp.mapSprites = new Sprite[width, height];
        }*/

        /*showTileEditor = EditorGUILayout.Foldout(showTileEditor, "Tile Editor");

        if (showTileEditor)
        {
            for (int h = 0; h < height; h++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int w = 0; w < width; w++)
                {
                    comp.mapSprites[w, h] = (Sprite)EditorGUILayout.ObjectField(comp.mapSprites[w, h], typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f));
                }
                EditorGUILayout.EndHorizontal();
            }
        }*/
        if (GUILayout.Button("Load"))
        {
            dbsource.LoadData();
        }
        EditorUtility.SetDirty(dbsource);
    }

}
#endif