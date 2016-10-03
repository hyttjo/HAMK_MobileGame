using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(LevelEditor))]
public class LevelEditorInspector : Editor {

    LevelEditor editor;

    private GUIContent[] objects;
    private string[] layers;
    private int previousActiveGO_index;

    private GUIStyle instruction;
    private GUIStyle info;

    public void OnEnable() {
        editor = (LevelEditor)target;

        Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        texture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
        texture.Apply();

        instruction = new GUIStyle { fontSize = 10, normal = new GUIStyleState { textColor = Color.red, background = texture }, padding = new RectOffset(10, 10, 2, 2) };
        info = new GUIStyle { fontSize = 10, normal = new GUIStyleState { textColor = Color.white }, padding = new RectOffset(10, 10, 2, 2) };

        if (editor.prefabs != null) {
            objects = new GUIContent[editor.prefabs.Length];

            for (int i = 0; i < editor.prefabs.Length; i++) {
                GUIContent content = new GUIContent();
                Texture2D image = Misc.GetTextureFromObject(editor.prefabs[i]);

                if (image != null) {
                    content.image = image;
                    content.tooltip = editor.prefabs[i].name;
                    objects[i] = content;
                } else {
                    objects[i] = GUIContent.none;
                }
            }
        }

        layers = new string[] { "0: Background", "1: Middleground", "2: PlayingLayer", "3: Foreground", "4: Colliders" };

        SceneView.onSceneGUIDelegate += OnScene;
    }

    public void OnDisable() {
        SceneView.onSceneGUIDelegate -= OnScene;
    }

    /***********************************/
    /****** Draw custom inspector ******/
    /***********************************/

    public override void OnInspectorGUI() {
        DrawLevelDimensions();

        DrawLayerSelection();

        if (editor.layer_index != 4) {
             DrawPrefabSelection();
        } else {
            DrawColliderCreation();
        }
        editor.activeGo = editor.GetActiveGameObject();
        editor.UpdateBottomPit();

        SceneView.RepaintAll();
    }

    void DrawLevelDimensions() {
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
    }

    void DrawLayerSelection() {
        GUILayout.Space(5);

        GUILayout.Label("Selected layer:");
        GUILayout.BeginHorizontal();
        editor.layer_index = GUILayout.SelectionGrid(editor.layer_index, layers, 1);
        GUILayout.EndHorizontal();
    }

    void DrawPrefabSelection() {
        editor.overwrite = GUILayout.Toggle(editor.overwrite, "Overwrite:");

        GUILayout.Space(5);

        Texture2D activeGO_texture = null;

        if (editor.activeGo != null) {
            SpriteRenderer sRenderer = editor.activeGo.GetComponentInChildren<SpriteRenderer>();

            if (sRenderer != null) {
                Sprite activeGO_sprite = sRenderer.sprite;
                int renderer_layer = sRenderer.sortingOrder;

                if (previousActiveGO_index != editor.activeGO_index) {
                    editor.layer_index = renderer_layer;
                }

                previousActiveGO_index = editor.activeGO_index;

                if (activeGO_sprite != null) {
                    activeGO_texture = Misc.GetTextureFromSprite(activeGO_sprite);

                    if (activeGO_texture != null) {
                        activeGO_texture.filterMode = FilterMode.Point;
                    }
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
    }

    void DrawColliderCreation() {
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

                    GUILayout.Space(10);
                        
                    for (int i = 0; i < editor.colliderPoints.Count; i++) {
                        Vector2 point = editor.colliderPoints[i];
                        GUILayout.Label("Point: " + i + ": (x " + point.x + ", y " + point.y + ")");
                    }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();  

        GUILayout.Space(40);
    }

    /************************************/
    /****** Scene istruction texts ******/
    /************************************/

    void OnScene(SceneView sceneView) {
        Handles.BeginGUI();

        if (Selection.activeGameObject == editor.gameObject) {
            if (editor.layer_index != 4) {
                if (editor.selectionCopying) {
                    if (editor.copyObjects.Count == 0) {
                        GUILayout.Label("Finish copy-area selection by releasing right mouse button", instruction);
                    } else {
                        GUILayout.Label("Paste selected area by clicking right mouse button again on a new area", instruction);
                        GUILayout.Label("Cancel area copying by pressing 'Esc'", instruction);
                    }
                } else if (editor.pathCreation) {
                    GUILayout.Label("Creating a new path...", info);
                    GUILayout.Label("Right mouse click places a new path point", instruction);
                    GUILayout.Label("When finished placing path points press 'Enter' or cancel it by pressing 'Esc'", instruction);
                    GUILayout.Label("Path points: " + editor.path.Count, info);
                } else {
                    GUILayout.Label("Right mouse click places selected prefab to the selected layer", instruction);
                    GUILayout.Label("Start Copy-Paste feature by dragging mouse while pressing right mouse button", instruction);
                    GUILayout.Label("Selected layer: " + editor.layers[editor.layer_index].name, info);
                    GUILayout.Label("Selected prefab: " + editor.activeGo.name, info);
                }
            } else {
                if (editor.colliderCreation) {
                    GUILayout.Label("Creating a new collider...", info);
                    GUILayout.Label("Right mouse click places a new collider point", instruction);
                    GUILayout.Label("Click 'Collider done' button when finished placing points or press 'Enter'", instruction);
                    GUILayout.Label("Collider points: " + editor.colliderPoints.Count, info);
                } else {
                    GUILayout.Label("Click 'Create New Collider' button to start placing points for a new collider or right click on the starting point", instruction);
                }
            }
        } else {
            GUILayout.Label("Level GameObject not selected. You need to select it to use all the LevelEditor features", instruction);
        }
        Handles.EndGUI();
    }
}