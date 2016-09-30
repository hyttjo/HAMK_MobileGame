﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelEditor))]
public class LevelEditorInspector : Editor {

    LevelEditor editor;

    private GUIContent[] objects;
    private string[] layers;

    public void OnEnable() {
        editor = (LevelEditor)target;

        if (editor.prefabs != null) {
            objects = new GUIContent[editor.prefabs.Length];

            for (int i = 0; i < editor.prefabs.Length; i++) {
                GUIContent content = new GUIContent();
                content.image = GetTextureFromObject(editor.prefabs[i]);
                content.tooltip = editor.prefabs[i].name;
                objects[i] = content;
            }
        }

        layers = new string[] { "0: Background", "1: Middleground", "2: PlayingLayer", "3: Foreground", "4: Colliders" };
    }

    public override void OnInspectorGUI() {
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Grid Width:");
        editor.width = EditorGUILayout.IntField(editor.width, GUILayout.Width(50));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Grid Height:");
        editor.height = EditorGUILayout.IntField(editor.height, GUILayout.Width(50));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.Label("Selected layer:");
        GUILayout.BeginHorizontal();
        editor.layer_index = GUILayout.SelectionGrid(editor.layer_index, layers, 1);
        GUILayout.EndHorizontal();

        if (editor.layer_index != 4) {

            editor.overwrite = GUILayout.Toggle(editor.overwrite, "Overwrite:");

            GUILayout.Space(5);

            Texture2D activeGO_texture = null;

            if (editor.activeGo != null) {
                SpriteRenderer sRenderer = editor.activeGo.GetComponentInChildren<SpriteRenderer>();

                if (sRenderer != null) {
                    Sprite activeGO_sprite = sRenderer.sprite;

                    if (activeGO_sprite != null) {
                        activeGO_texture = Misc.GetTextureFromSprite(activeGO_sprite);
                        activeGO_texture.filterMode = FilterMode.Point;
                    }
                }
            }

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Selected prefab:");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Box(activeGO_texture);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            string activeGoName = "null";
            if (editor.activeGo != null) {
                activeGoName = editor.activeGo.name.Replace("(Clone)", "");
            }
            GUILayout.Label(activeGoName);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        
            editor.activeGO_index = GUILayout.SelectionGrid(editor.activeGO_index, objects, 6);
        } else {
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                    GUILayout.Label("Collider creation:");
                    GUILayout.Space(5);
                        GUILayout.BeginHorizontal();
                        if (!editor.colliderCreation) {
                            if (GUILayout.Button("Create New Collider ")) {
                                editor.LoadColliders();
                                editor.CreateCollider();
                            }
                        } else {
                            GUILayout.Button("Creating collider...");
                        }
                        if (editor.colliderCreation) {
                            if (GUILayout.Button("Collider done")) {
                                editor.ColliderAssignPoints();
                            }
                        } else {
                            GUILayout.Button(" Waiting... ");
                        }
                        GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();  

            GUILayout.Space(40);
        }
        editor.activeGo = editor.GetActiveGameObject();

        SceneView.RepaintAll();
    }

    private Texture2D GetTextureFromObject(GameObject gameObject) {
        if (gameObject != null) {
            SpriteRenderer sRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

            if (sRenderer != null) {
                Sprite sprite = sRenderer.sprite;

                if (sprite != null) {
                    Texture2D texture = Misc.GetTextureFromSprite(sprite);
                    texture.filterMode = FilterMode.Point;
                    return texture;
                }
            }
        }
        return null;
    }
}