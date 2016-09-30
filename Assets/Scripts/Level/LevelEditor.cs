using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(Level))]
public class LevelEditor : MonoBehaviour {

    private Level level;

    private Dictionary<Position, GameObject> objects;
    private GameObject[] layers;
    public GameObject[] prefabs;
    public GameObject activeGo;

    public List<GameObject> colliders;
    public List<Vector2> colliderPoints;
    public bool colliderCreation = false;

    private Vector3 point;

    public int activeGO_index = 0;
    public int layer_index = 0;

    public int width = 128;
    public int height = 32;
    public float tileSize = 1;

    public bool overwrite = false;

    public void OnEnable() {
        if (Application.isEditor) {
            level = GetComponent<Level>();

            if (level == null) {
                level = gameObject.AddComponent<Level>();
            }

            width = level.width;
            height = level.height;

            layers = level.GetLayerGameObjects();
            objects = level.GetLevelData();
            colliders = new List<GameObject>();

            SceneView.onSceneGUIDelegate += GridUpdate;
            SceneView.onSceneGUIDelegate += OnScene;
            
            prefabs = LoadPrefabs();

            if (prefabs != null) {
                activeGo = GetActiveGameObject();
            }
        }
    }

    public void OnDisable() {
        SceneView.onSceneGUIDelegate -= GridUpdate;
        SceneView.onSceneGUIDelegate -= OnScene;
    }

    void GridUpdate(SceneView sceneview) {
        level.width = width;
        level.height = height;

        Event e = Event.current;
        Camera camera = Camera.current;

        if (camera != null) {
            Vector3 position = camera.ScreenToWorldPoint(new Vector3(e.mousePosition.x, -e.mousePosition.y + Screen.height - 40, 0));

            if (layer_index != 4) {
                point = new Vector3(Mathf.Floor(position.x / tileSize) * tileSize + tileSize / 2.0f,
                                    Mathf.Floor(position.y / tileSize) * tileSize + tileSize / 2.0f, 0);

                if (IsInsideGrid(position)) {
                    if (activeGo != null) {
                        activeGo = GetActiveGameObject();
                        activeGo.transform.position = point;
                    }

                    if (e.isMouse && e.button == 1 && e.type == EventType.mouseDown) {
                        if (activeGo != null) {
                            Position pos = new Position(new Vector3(point.x, point.y, layer_index));

                            if (!objects.ContainsKey(pos)) {
                                PlaceGameObject(pos, point);
                            } else {
                                GameObject obj = objects[pos];

                                if (obj == null) {
                                    objects.Remove(pos);
                                    PlaceGameObject(pos, point);
                                } else {
                                    if (overwrite) {
                                        DestroyImmediate(obj);
                                        objects.Remove(pos);
                                        PlaceGameObject(pos, point);
                                    } else {
                                        Debug.Log(pos.ToString() + " is already taken by an object!");
                                    }
                                }
                            }
                        }
                    }
                }
            } else {
                point = new Vector3(Mathf.Round(position.x / tileSize) * tileSize,
                                    Mathf.Round(position.y / tileSize) * tileSize, 0);

                if (IsInsideGrid(position)) {
                    if (activeGo != null) {
                        activeGo = GetActiveGameObject();
                        activeGo.transform.position = point;
                    }
                }

                if (colliderCreation) {
                    if (e.isMouse && e.button == 1 && e.type == EventType.mouseDown) {
                        if (colliderPoints != null) {
                            colliderPoints.Add(point);
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

        if (IsInsideGrid(point)) {
            if (layer_index != 4) {
                Gizmos.color = Color.red;

                Gizmos.DrawLine(point + new Vector3(-0.5f * tileSize, -0.5f * tileSize, 0), point + new Vector3(-0.5f * tileSize, 0.5f * tileSize, 0));
                Gizmos.DrawLine(point + new Vector3(-0.5f * tileSize, 0.5f * tileSize, 0), point + new Vector3(0.5f * tileSize, 0.5f * tileSize, 0));
                Gizmos.DrawLine(point + new Vector3(0.5f * tileSize, 0.5f * tileSize, 0), point + new Vector3(0.5f * tileSize, -0.5f * tileSize, 0));
                Gizmos.DrawLine(point + new Vector3(0.5f * tileSize, -0.5f * tileSize, 0), point + new Vector3(-0.5f * tileSize, -0.5f * tileSize, 0));
            } else {
                Gizmos.color = Color.red;

                Gizmos.DrawLine(point + new Vector3(-0.2f * tileSize, -0.2f * tileSize, 0), point + new Vector3(-0.2f * tileSize, 0.2f * tileSize, 0));
                Gizmos.DrawLine(point + new Vector3(-0.2f * tileSize, 0.2f * tileSize, 0), point + new Vector3(0.2f * tileSize, 0.2f * tileSize, 0));
                Gizmos.DrawLine(point + new Vector3(0.2f * tileSize, 0.2f * tileSize, 0), point + new Vector3(0.2f * tileSize, -0.2f * tileSize, 0));
                Gizmos.DrawLine(point + new Vector3(0.2f * tileSize, -0.2f * tileSize, 0), point + new Vector3(-0.2f * tileSize, -0.2f * tileSize, 0));

                Gizmos.color = Color.green;

                if (colliderCreation) {
                    for (int i = 0; i < colliderPoints.Count; i++) {
                        if (i < colliderPoints.Count - 1) {
                            Gizmos.DrawLine(colliderPoints[i], colliderPoints[i + 1]);
                        }
                    }

                    Gizmos.color = Color.yellow;

                    if (colliderPoints.Count > 0) {
                        Gizmos.DrawLine(colliderPoints.Last(), point);
                    }
                }
            }
        }
    }

    void OnScene(SceneView sceneView) {
        Handles.BeginGUI();
        if (layer_index != 4) {
            GUILayout.Label("Right mouse click places selected prefab to the selected layer");
            GUILayout.Label("Selected layer: " + layers[layer_index].name);
            GUILayout.Label("Selected prefab: " + activeGo.name);
        } else {
            if (colliderCreation) {
                GUILayout.Label("Creating a new collider...");
                GUILayout.Label("Right mouse click places a new collider point");
                GUILayout.Label("Click 'Collider done' button when finished placing points");
                GUILayout.Label("Collider points: " + colliderPoints.Count);
            } else {
                GUILayout.Label("Click 'Create New Collider' button to start placing points for a new collider");
            }
        }
        Handles.EndGUI();
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

    public void LoadColliders() {
        colliders = Misc.GetChildren(layers[layer_index]).ToList();
    }

    public void CreateCollider() {
        if (!colliderCreation) {
            colliderCreation = true;

            int colliderIndex = colliders.Count;
            GameObject collider = new GameObject("Collider: " + colliderIndex);
            collider.transform.parent = layers[layer_index].transform;
            collider.AddComponent<EdgeCollider2D>();
            colliderPoints = new List<Vector2>();

            colliders.Add(collider);
        } else {
            Debug.Log("Already creating a new collider, you need to mark current one as 'done' first before creating a new one.");
        }
    }

    public void ColliderAssignPoints() {
        if (colliderCreation) {
            colliderCreation = false;

            if (colliders.Count > 0 && colliderPoints.Count > 0) {
                int collidersLength = colliders.Count - 1;
                GameObject collider = colliders[collidersLength];
                string start = "Start: (" + (int)colliderPoints.First().x + ", " + (int)colliderPoints.First().y + ")";
                string end = "End: (" + (int)colliderPoints.Last().x + ", " + (int)colliderPoints.Last().y + ")";
                collider.name = "Collider: " + collidersLength + " - " + start + " - " + end;

                EdgeCollider2D eCollider = collider.GetComponent<EdgeCollider2D>();
                if (eCollider != null) {
                    eCollider.points = colliderPoints.ToArray();
                }
            }
        } else {
            Debug.Log("Not creating currently a new collider, you need to create an active one first to assign points to it.");
        }
    }
}