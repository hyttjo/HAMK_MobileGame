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

    private Dictionary<Position, GameObject> copyObjects;
    public bool selectionCopying = false;
    public Vector3 startPoint;
    public Vector3 endPoint;

    private Vector3 point;

    public int activeGO_index = 0;
    public int layer_index = 0;

    public int width = 128;
    public int height = 32;
    public float tileSize = 1;

    public bool overwrite = false;

    private GameObject bottomPit;

    private GUIStyle instruction;
    private GUIStyle info;

    public void OnEnable() {
        if (Application.isEditor) {
            Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
            texture.Apply();
            instruction = new GUIStyle { fontSize = 10, normal = new GUIStyleState { textColor = Color.red, background = texture }, padding = new RectOffset(10, 10, 2, 2) };
            info = new GUIStyle { fontSize = 10, normal = new GUIStyleState { textColor = Color.white }, padding = new RectOffset(10, 10, 2, 2) };

            level = GetComponent<Level>();

            if (level == null) {
                level = gameObject.AddComponent<Level>();
            }

            width = level.width;
            height = level.height;
            tileSize = level.tileSize;

            layers = level.GetLayerGameObjects();
            objects = level.GetLevelData();
            colliders = new List<GameObject>();
            copyObjects = new Dictionary<Position, GameObject>();

            SceneView.onSceneGUIDelegate += LevelUpdate;
            SceneView.onSceneGUIDelegate += OnScene;
            
            prefabs = LoadPrefabs();

            if (prefabs != null) {
                activeGo = GetActiveGameObject();
                bottomPit = GameObject.FindGameObjectWithTag("DamageTypePit");

                if (bottomPit == null) {
                    bottomPit = GetBottomPit();
                    Instantiate(bottomPit);
                    UpdateBottomPit();
                } else { 
                    UpdateBottomPit();
                }
            }
        }
    }

    public void OnDisable() {
        SceneView.onSceneGUIDelegate -= LevelUpdate;
        SceneView.onSceneGUIDelegate -= OnScene;
    }

    void LevelUpdate(SceneView sceneview) {
        level.width = width;
        level.height = height;
        level.tileSize = tileSize;

        Event e = Event.current;
        Camera camera = Camera.current;

        if (camera != null) {
            Vector3 position = camera.ScreenToWorldPoint(new Vector3(e.mousePosition.x, -e.mousePosition.y + Screen.height - 40, 0));

            if (Misc.IsInsideGrid(level, position)) {
                if (activeGo != null) {
                    activeGo = GetActiveGameObject();
                    activeGo.transform.position = point;
                }

                if (layer_index != 4) {
                    point = new Vector3(Mathf.Floor(position.x / tileSize) * tileSize + tileSize / 2.0f,
                                        Mathf.Floor(position.y / tileSize) * tileSize + tileSize / 2.0f, 0);
                } else {
                    point = new Vector3(Mathf.Round(position.x / tileSize) * tileSize,
                                        Mathf.Round(position.y / tileSize) * tileSize, 0);
                }

                if (e.isMouse && e.button == 1) {
                    int controlID = GUIUtility.GetControlID (FocusType.Passive);

                    switch (e.GetTypeForControl (controlID)) {
                        case EventType.MouseDown:
                            GUIUtility.hotControl = controlID;
                            if (!selectionCopying) {
                                startPoint = point;
                                copyObjects.Clear();
                            }
                            e.Use();
                        break;

                        case EventType.MouseUp:
                            GUIUtility.hotControl = 0;
                            if (layer_index == 4) {
                                if (colliderCreation) {
                                    if (colliderPoints != null) {
                                        colliderPoints.Add(point);
                                    }
                                } else {
                                    CreateCollider();
                                    colliderPoints.Add(point);
                                }
                            } else {
                                if (selectionCopying) {
                                    if (copyObjects.Count == 0) {
                                        HandleAreaCopy();
                                    } else {
                                        HandleAreaPaste();
                                    }
                                } else {
                                    HandleObjectPlacement();
                                }
                            }
                            e.Use();
                        break;

                        case EventType.MouseDrag:
                            GUIUtility.hotControl = controlID;
                            selectionCopying = true;
                            e.Use();
                        break;
                    }
                } else if (e.isKey && e.keyCode == KeyCode.Return) {
                    if (colliderCreation) {
                         ColliderAssignPoints();
                    }
                }
            }
        }   
    }

    void HandleObjectPlacement() {
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

    void HandleAreaCopy() {
        if (startPoint == point || !Misc.IsSelectionValid(startPoint, point)) {
            endPoint = startPoint;
        } else {
            endPoint = point;
        }

        Position start = new Position(startPoint);
        Position end = new Position(endPoint);

        for (int x = 0; x < end.x + 1 - start.x; x++) {
            for (int y = 0; y < end.y + 1 - start.y; y++) {
                for (int z = 0; z < 4; z++) {
                    Position pos = new Position(start.x + x, start.y + y, z);

                    GameObject obj;
                    if (objects.TryGetValue(pos, out obj)) {
                        copyObjects.Add(pos, obj);
                    }
                }
            }
        }
        if (copyObjects.Count == 0) {
            copyObjects.Clear();
            selectionCopying = false;
            Debug.Log("Copy-area was empty, copying cancelled!");
        }
    }

    void HandleAreaPaste() {
        selectionCopying = false;
        Position startPaste = new Position(point);

        foreach (Position pos in copyObjects.Keys) {
            GameObject _gameObject;
            GameObject target;

            float posX = pos.x - startPoint.x + startPaste.x + 1;
            float posY = pos.y - startPoint.y + startPaste.y + 1;
            Position pastePoint = new Position(new Vector3(posX, posY, pos.z));

            if (!objects.ContainsKey(pastePoint)) {
                target = copyObjects[pos];
                if (target != null) {
                    _gameObject = Instantiate(target);
                    _gameObject.transform.parent = layers[pos.z].transform;
                    _gameObject.transform.position = new Vector3(posX, posY, 0);
                    _gameObject.name = target.name.Replace(pos.ToString(), pastePoint.ToString());
                }
            } 
        }
        objects = level.GetLevelData();              
        Debug.Log(copyObjects.Count + " objects copied to Start " + startPaste.ToString());
        copyObjects.Clear();
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

        DrawRectangle(Vector3.zero, new Vector3(width, height, 0), 0, Color.white);

        if (Misc.IsInsideGrid(level, point)) {
            if (layer_index != 4) {
                if (selectionCopying) {
                    if (copyObjects.Count == 0) {
                        if (!Misc.IsSelectionValid(startPoint, point)) {
                            point = startPoint;
                        }
                        DrawRectangle(startPoint, point, tileSize / 2, Color.yellow);
                    } else {
                        foreach (KeyValuePair<Position, GameObject> pair in copyObjects) {
                            Position pos = pair.Key;
                            Vector3 objectPoint =  new Vector3(0.5f, 0.5f, 0) + new Vector3(pos.x, pos.y, 0)  - new Vector3(startPoint.x, startPoint.y, 0) + new Vector3(point.x, point.y, 0);
                            DrawRectangle(objectPoint, objectPoint, tileSize / 2, Color.cyan);
                        }
                    }
                } else {
                    DrawRectangle(point, point, tileSize / 2, Color.red);
                }
            } else {
                DrawRectangle(point, point, tileSize / 5, Color.red);

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

    private void DrawRectangle(Vector3 start, Vector3 end, float offset, Color color) {
        Gizmos.color = color;
        Gizmos.DrawLine(start + new Vector3(-offset * tileSize, -offset * tileSize, 0), new Vector3(end.x, start.y, 0) + new Vector3(offset * tileSize, -offset * tileSize, 0));
        Gizmos.DrawLine(new Vector3(end.x, start.y, 0) + new Vector3(offset * tileSize, -offset * tileSize, 0), end + new Vector3(offset * tileSize, offset * tileSize, 0));
        Gizmos.DrawLine(end + new Vector3(offset * tileSize, offset * tileSize, 0), new Vector3(start.x, end.y, 0) + new Vector3(-offset * tileSize, offset * tileSize, 0));
        Gizmos.DrawLine(new Vector3(start.x, end.y, 0) + new Vector3(-offset * tileSize, offset * tileSize, 0), start + new Vector3(-offset * tileSize, -offset * tileSize, 0));
    }

    void OnScene(SceneView sceneView) {
        Handles.BeginGUI();

        if (Selection.activeGameObject == gameObject) {
            if (layer_index != 4) {
                if (selectionCopying) {
                    if (copyObjects.Count == 0) {
                        GUILayout.Label("Finish copy-area selection by releasing right mouse button", instruction);
                    } else {
                        GUILayout.Label("Paste selected area by clicking right mouse button again on a new area", instruction);
                    }
                } else {
                    GUILayout.Label("Right mouse click places selected prefab to the selected layer", instruction);
                    GUILayout.Label("Start Copy-Paste feature by dragging mouse while pressing right mouse button", instruction);
                    GUILayout.Label("Selected layer: " + layers[layer_index].name, info);
                    GUILayout.Label("Selected prefab: " + activeGo.name, info);
                }
            } else {
                if (colliderCreation) {
                    GUILayout.Label("Creating a new collider...", info);
                    GUILayout.Label("Right mouse click places a new collider point", instruction);
                    GUILayout.Label("Click 'Collider done' button when finished placing points or press 'Enter'", instruction);
                    GUILayout.Label("Collider points: " + colliderPoints.Count, info);
                } else {
                    GUILayout.Label("Click 'Create New Collider' button to start placing points for a new collider or right click on the starting point", instruction);
                }
            }
        } else {
            GUILayout.Label("Level GameObject not selected. You need to select it to use all the LevelEditor features", instruction);
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

    GameObject GetBottomPit() {
        GameObject bottomPit = null;

        if (prefabs != null) {
            foreach (GameObject _gameObject in prefabs) {
                if (_gameObject.name == "BottomDamage") {
                    bottomPit = _gameObject;
                }
            }
        }
        return bottomPit;
    }

    public void UpdateBottomPit() {
        bottomPit.transform.position = Vector3.zero;
        bottomPit.name.Replace("(Clone)", "");
        EdgeCollider2D eCollider = bottomPit.GetComponent<EdgeCollider2D>();
        eCollider.offset = new Vector2(0, -3);
        eCollider.points = new Vector2[] { new Vector2(-10, 0), new Vector2(level.width + 10, 0) };
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
                    colliderPoints.Clear();

                    PhysicsMaterial2D physicsMaterial = (PhysicsMaterial2D)Resources.Load("Materials/Grass");

                    if (physicsMaterial != null) {
                        eCollider.sharedMaterial = physicsMaterial;
                    }
                }
            }
        } else {
            Debug.Log("Not creating currently a new collider, you need to create an active one first to assign points to it.");
        }
    }
}