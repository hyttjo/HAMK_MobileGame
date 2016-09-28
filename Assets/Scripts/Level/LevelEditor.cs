using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class LevelEditor : MonoBehaviour {

    private Dictionary<Position, GameObject> objects;

    private GameObject[] layers;

    public GameObject[] prefabs;
    public GameObject activeGo;

    public int activeGO_index = 0;
    public int layer_index = 0;

    public int width = 128;
    public int height = 32;
    public float tileSize = 1;

    public bool overwrite = false;

    public void OnEnable() {
        if (Application.isEditor) {
            objects = new Dictionary<Position, GameObject>();

            SceneView.onSceneGUIDelegate += GridUpdate;

            layers = LoadLayersGameObjects();
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
                        Position pos = new Position(new Vector3(aligned.x, aligned.y, layer_index));

                        if (!objects.ContainsKey(pos)) {
                            PlaceGameObject(pos, aligned);
                        } else {
                            GameObject obj = objects[pos];

                            if (obj == null) {
                                objects.Remove(pos);
                                PlaceGameObject(pos, aligned);
                            } else {
                                if (overwrite) {
                                    DestroyImmediate(obj);
                                    objects.Remove(pos);
                                    PlaceGameObject(pos, aligned);
                                } else {
                                    Debug.Log(pos.ToString() + " is already taken by an object!");
                                }
                            }
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

    GameObject[] LoadLayersGameObjects() {
        GameObject[] layersArray = new GameObject[4];

        GameObject background = GameObject.Find("Background");
        if (background == null) {
            background = new GameObject("Background");
            background.transform.parent = transform;
        }
        layersArray[0] = background;

        GameObject middleground = GameObject.Find("Middleground");
        if (middleground == null) {
            middleground = new GameObject("Middleground");
            middleground.transform.parent = transform;
        }
        layersArray[1] = middleground;

        GameObject playinglayer = GameObject.Find("PlayingLayer");
        if (playinglayer == null) {
            playinglayer = new GameObject("PlayingLayer");
            playinglayer.transform.parent = transform;
        }
        layersArray[2] = playinglayer;

        GameObject foreground = GameObject.Find("Foreground");
        if (foreground == null) {
            foreground = new GameObject("Foreground");
            foreground.transform.parent = transform;
        }
        layersArray[3] = foreground;

        return layersArray;
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

    private void PlaceGameObject(Position pos, Vector3 position) {
        GameObject obj = (GameObject)Instantiate(activeGo);
        obj.name = activeGo.name + " - " + pos.ToString();
        SpriteRenderer sRenderer = obj.GetComponentInChildren<SpriteRenderer>();

        if (sRenderer != null) {
            sRenderer.sortingOrder = layer_index;
        }
        obj.transform.position = position;
        obj.transform.parent = layers[layer_index].transform;
        objects.Add(pos, obj);
        Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
    }
}