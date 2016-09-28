using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
//[RequireComponent(typeof(Level))]
public class LevelCreator : MonoBehaviour {

    //private Level level;
    private Dictionary<Position, GameObject> objects;
    public GameObject[] prefabs;
    public GameObject activeGo;
    public int activeGO_index = 0;

    public int width = 128;
    public int height = 32;
    public float tileSize = 1;

    public void OnEnable() {
        if (Application.isEditor) {
            objects = new Dictionary<Position, GameObject>();

            SceneView.onSceneGUIDelegate += GridUpdate;

            prefabs = LoadPrefabs();

            if (prefabs != null) {
                activeGo = GetActiveGameObject();
            }
        }
    }

    public void OnDisable() {
        SceneView.onSceneGUIDelegate -= GridUpdate;
    }

    void GridUpdate(SceneView sceneview) { 
        Event e = Event.current;
        Camera camera = Camera.current;

        if (camera != null) {
            Vector3 position = camera.ScreenToWorldPoint(new Vector3(e.mousePosition.x, -e.mousePosition.y + Screen.height - 40, 0));
            Vector3 aligned = new Vector3(Mathf.Floor(position.x / tileSize) * tileSize + tileSize / 2.0f,
                                          Mathf.Floor(position.y / tileSize) * tileSize + tileSize / 2.0f, 0);

            if (IsInsideGrid(position)) {
                if (activeGo != null) {
                    activeGo = GetActiveGameObject();
                    activeGo.transform.position = aligned;
                }

                if (e.isMouse && e.button == 1 && e.type == EventType.mouseDown) {
                    if (activeGo != null) {
                        Position pos = new Position(aligned);

                        if (!objects.ContainsKey(pos)) {
                            GameObject obj = (GameObject)Instantiate(activeGo);
                            obj.transform.position = aligned;
                            objects.Add(pos, obj);
                        }
                    }
                }
            }
        }
    }

    bool IsInsideGrid(Vector3 position) {
        if (position.x < 0) {
            return false;
        } else if (position.y < 0) {
            return false;
        } else if (position.x > width * tileSize) {
            return false;
        } else if (position.y > height * tileSize) {
            return false;
        } else {
            return true;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.black;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float x_size = Mathf.Floor(x / tileSize) * tileSize;

                Gizmos.DrawLine(new Vector3(x_size, 0, 0), new Vector3(x_size, height, 0));
            }
            float y_size = Mathf.Floor(y / tileSize) * tileSize;

            Gizmos.DrawLine(new Vector3(0, y_size, 0), new Vector3(width, y_size, 0));
        }

        Gizmos.color = Color.white;

        Gizmos.DrawLine(Vector3.zero, new Vector3(0, height * tileSize, 0));
        Gizmos.DrawLine(new Vector3(0, height * tileSize, 0), new Vector3(width * tileSize, height * tileSize, 0));
        Gizmos.DrawLine(new Vector3(width * tileSize, height * tileSize, 0), new Vector3(width * tileSize, 0, 0));
        Gizmos.DrawLine(new Vector3(width * tileSize, 0, 0), Vector3.zero);
    }

    GameObject[] LoadPrefabs() {
        GameObject[] prefabsArray = Resources.LoadAll<GameObject>("Prefabs");
        List<GameObject> prefabs = prefabsArray.ToList();

        for (int i = 0; i < prefabsArray.Length; i++) {
            SpriteRenderer sRenderer = prefabsArray[i].GetComponentInChildren<SpriteRenderer>();

            if (sRenderer == null) {
                prefabs.RemoveAt(i);
            }
        }
        return prefabs.ToArray();
    }

    public GameObject GetActiveGameObject() {
        if (prefabs != null) {
            return prefabs[activeGO_index];
        }
        return null;
    }
}