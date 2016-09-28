using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelCreator))]
public class LevelCreatorInspector : Editor {

    LevelCreator level;

    private GUIContent[] objects;
    private string[] layers;

    public void OnEnable() {
        level = (LevelCreator)target;

        if (level.prefabs != null) {
            objects = new GUIContent[level.prefabs.Length];

            for (int i = 0; i < level.prefabs.Length; i++) {
                GUIContent content = new GUIContent();
                content.image = GetTextureFromObject(level.prefabs[i]);
                content.tooltip = level.prefabs[i].name;
                objects[i] = content;
            }
        }

        layers = new string[] { "Background", "Collider", "Entities", "Foreground" };
    }

    public override void OnInspectorGUI() {
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(" Grid Width ");
        level.width = EditorGUILayout.IntField(level.width, GUILayout.Width(50));
        GUILayout.FlexibleSpace();
        GUILayout.Label(" Grid Height ");
        level.height = EditorGUILayout.IntField(level.height, GUILayout.Width(50));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.Label("Selected layer:");
        GUILayout.BeginHorizontal();
        level.layer_index = GUILayout.SelectionGrid(level.layer_index, layers, 1);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        Texture2D activeGO_texture = null;

        if (level.activeGo != null) {
            SpriteRenderer sRenderer = level.activeGo.GetComponentInChildren<SpriteRenderer>();

            if (sRenderer != null) {
                Sprite activeGO_sprite = sRenderer.sprite;

                if (activeGO_sprite != null) {
                    activeGO_texture = Misc.GetTextureFromSprite(activeGO_sprite);
                    activeGO_texture.filterMode = FilterMode.Point;
                }
            }
        }

        GUILayout.Space(10);
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
        if (level.activeGo != null) {
            activeGoName = level.activeGo.name.Replace("(Clone)", "");
        }
        GUILayout.Label(activeGoName);

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        level.activeGO_index = GUILayout.SelectionGrid(level.activeGO_index, objects, 6);

        level.activeGo = level.GetActiveGameObject();

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