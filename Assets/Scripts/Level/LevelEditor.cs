using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
[RequireComponent(typeof(Level))]
public class LevelEditor : MonoBehaviour {

    private Level level;

    private Dictionary<Position, GameObject> objects;
    public GameObject[] layers;
    public GameObject[] prefabs;
    public GameObject activeGo;

    public List<GameObject> colliders;
    public List<Vector2> colliderPoints;
    public bool colliderCreation = false;

    public Dictionary<Position, GameObject> copyObjects;
    public bool selectionCopying = false;
    public Vector3 startPoint;
    public Vector3 endPoint;

    public List<AIControl> pathObjects;
    public List<Vector2> path;
    public bool pathCreation = false;

    private Vector3 point;

    

    public int activeGO_index = 0;
    public int layer_index = 0;

    public int width = 128;
    public int height = 32;
    public float tileSize = 1;
    private int gizmoSize = 1;

    public bool overwrite = false;

    private GameObject bottomPit;

    private GUIStyle info;

    public void OnEnable() {
        if (Application.isEditor) {
            level = GetComponent<Level>();

            if (level == null) {
                level = gameObject.AddComponent<Level>();
            }

            width = level.width;
            height = level.height;
            tileSize = level.tileSize;

            info = new GUIStyle { fontSize = 10, normal = new GUIStyleState { textColor = Color.white }, padding = new RectOffset(10, 10, 2, 2) };

            layers = level.GetLayerGameObjects();
            objects = level.GetLevelData();
            colliders = new List<GameObject>();
            copyObjects = new Dictionary<Position, GameObject>();

            path = new List<Vector2>();
            pathObjects = GameObject.FindObjectsOfType<AIControl>().ToList();

            if (pathObjects == null) {
                pathObjects = new List<AIControl>();
            }

            SceneView.onSceneGUIDelegate += LevelUpdate;
            
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
        SceneView.onSceneGUIDelegate += OnScene;
    }

    public void OnDisable() {
        SceneView.onSceneGUIDelegate -= LevelUpdate;
        SceneView.onSceneGUIDelegate -= OnScene;
    }

    /**************************************************/
    /****** Handle user's input and update level ******/
    /**************************************************/

    private void LevelUpdate(SceneView sceneview) {
        UpdateLevelDimensions();

        Event e = Event.current;
        Camera camera = Camera.current;

        if (camera != null) {
            Vector3 position = camera.ScreenToWorldPoint(new Vector3(e.mousePosition.x, -e.mousePosition.y + Screen.height - 40, 0));

            if (Misc.IsInsideGrid(level, position)) {
                UpdatePosition(position);

                if (e.isMouse) {
                    HandleMouseControls(e);
                } else if (e.isKey) {
                    HandleKeyboardControls(e);
                }
            }
        }   
    }

    private void UpdateLevelDimensions() {
        level.width = width;
        level.height = height;
        level.tileSize = tileSize;
    }

    private void UpdatePosition(Vector3 position) {
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
    }

    private void HandleMouseControls(Event e) {
        if (e.button == 1) { // Right mouse button
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
                        } else if (pathCreation) {
                            HandlePathCreation();
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
        }
    }

    private void HandleKeyboardControls(Event e) {
        if (e.keyCode == KeyCode.Return) {
            if (colliderCreation) {
                ColliderAssignPoints();
            }
            if (pathCreation) {
                AssignPath();
            }
        } else if (e.keyCode == KeyCode.Escape) {
            if (colliderCreation) {
                colliderCreation = false;
                colliderPoints.Clear();
            }

            if (selectionCopying) {
                selectionCopying = false;
                copyObjects.Clear();
            }

            if (pathCreation) {
                pathCreation = false;
                path.Clear();
            }
        } else if (e.keyCode == KeyCode.D) {
            DeleteGameObjects();
        }
    }

    /******************************************************/
    /****** Load prefabs and handle object placement ******/
    /******************************************************/

    private void HandleObjectPlacement() {
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

        AIControl aiControl = obj.GetComponent<AIControl>();

        if (aiControl != null) {
            pathObjects.Add(aiControl);
            pathCreation = true;
            path.Clear();
        }
    }

    private void DeleteGameObjects() {
        int deletedObjects = 0;

        for (int z = 0; z < layers.Length; z++) {
            Position pos = new Position((int)point.x, (int)point.y, z);

            GameObject obj;

            if (objects.TryGetValue(pos, out obj)) {
                Undo.DestroyObjectImmediate(obj);
                deletedObjects++;
            }
        }
        Debug.Log("Deleted " + deletedObjects + " objects at " + new Position(point).ToString());

        objects = level.GetLevelData();
    }

    public GameObject GetActiveGameObject() {
        if (prefabs != null) {
            return prefabs[activeGO_index];
        }
        return null;
    }

    private GameObject[] LoadPrefabs() {
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

    /****************************/
    /******* Area copying *******/
    /****************************/

    private void HandleAreaCopy() {
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
        objects = level.GetLevelData();
    }

    private void HandleAreaPaste() {
        selectionCopying = false;
        Position startPaste = new Position(point);

        foreach (Position pos in copyObjects.Keys) {
            GameObject _gameObject;
            GameObject target;

            float posX = pos.x - startPoint.x + startPaste.x + tileSize;
            float posY = pos.y - startPoint.y + startPaste.y + tileSize;
            Position pastePoint = new Position(new Vector3(posX, posY, pos.z));

            if (!objects.ContainsKey(pastePoint)) {
                target = copyObjects[pos];
                if (target != null) {
                    _gameObject = Instantiate(target);
                    _gameObject.transform.parent = layers[pos.z].transform;
                    _gameObject.transform.position = new Vector3(posX, posY, 0);
                    _gameObject.name = target.name.Replace(pos.ToString(), pastePoint.ToString());
                    Undo.RegisterCreatedObjectUndo(_gameObject, "Create " + _gameObject.name);
                }
            } 
        }
        objects = level.GetLevelData();              
        Debug.Log(copyObjects.Count + " objects copied to Start " + startPaste.ToString());
        copyObjects.Clear();
        Selection.activeGameObject = gameObject;
    }

    /****************************/
    /******* Path creation ******/
    /****************************/

    private void HandlePathCreation() {
        path.Add(point);
    }

    private void AssignPath() {
        if (pathObjects != null && pathObjects.Count > 0) {

            AIControl aiControl = pathObjects.Last().GetComponent<AIControl>();

            if (aiControl != null) {
                aiControl.path = path.ToArray();
            }
        }
        path.Clear();
        pathCreation = false;
    }

    /**************************************************/
    /****** Handle automatic bottom pit creation ******/
    /**************************************************/

    private GameObject GetBottomPit() {
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
        if (bottomPit != null) {
            bottomPit.transform.position = Vector3.zero;
            bottomPit.name.Replace("(Clone)", "");
            EdgeCollider2D eCollider = bottomPit.GetComponent<EdgeCollider2D>();
            eCollider.offset = new Vector2(0, -3);
            eCollider.points = new Vector2[] { new Vector2(-10, 0), new Vector2(level.width + 10, 0) };
        }
    }

    /*******************************/
    /****** Collider creation ******/
    /*******************************/

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

    /****************************/
    /****** Gizmos drawing ******/
    /****************************/

    private void OnDrawGizmos() {
        DrawGrid();

        if (Misc.IsInsideGrid(level, point)) {
            if (layer_index != 4) {
                if (selectionCopying) {
                    DrawSelectionCopy();
                } else if (pathCreation) {
                    DrawPathCreation();
                } else {
                    DrawObjectPlacement();
                }
            } else {
                DrawPointPlacement();

                if (colliderCreation) {
                    DrawColliderCreation();
                }
            }
        }
        DrawPaths();
    }

    private void DrawGrid() {
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
    }

    public void UpdateGridValues() {
        SpriteRenderer sRenderer = activeGo.GetComponentInChildren<SpriteRenderer>();

        if (sRenderer != null) {
            Sprite sprite = sRenderer.sprite;

            if (sprite != null) {
                int spriteSize = (int)sprite.rect.width * (int)sprite.rect.height;
                int pixelScaling = (int)sprite.pixelsPerUnit * (int)sprite.pixelsPerUnit;
                tileSize = spriteSize / pixelScaling;

                if (tileSize < 1) {
                    tileSize = 1;
                } else if (tileSize > 1) {
                    tileSize = (int)Mathf.Sqrt(tileSize);
                }
            }
        }
    }

    private void DrawSelectionCopy() {
        int gizmoSize = 2;

        if (tileSize > 1) {
            gizmoSize *= 2;
        }

        if (copyObjects.Count == 0) {
            if (!Misc.IsSelectionValid(startPoint, point)) {
                point = startPoint;
            }
            DrawRectangle(startPoint, point, tileSize / gizmoSize, Color.yellow);
        } else {
            foreach (KeyValuePair<Position, GameObject> pair in copyObjects) {
                Position pos = pair.Key;
                float offset = tileSize / gizmoSize;
                if (tileSize > 1) {
                    offset = gizmoSize / tileSize;
                }
                Vector3 objectPoint =  new Vector3(offset, offset, 0) + new Vector3(pos.x, pos.y, 0)  - new Vector3(startPoint.x, startPoint.y, 0) + new Vector3(point.x, point.y, 0);
                if (tileSize > 1) {
                    offset = tileSize / gizmoSize;
                }
                DrawRectangle(objectPoint, objectPoint, offset, Color.cyan);
            }
        }
    }

    private void DrawPathCreation() {
        int gizmoSize = 5;

        if (tileSize > 1) {
            gizmoSize *= 2;
        }

        DrawRectangle(point, point, tileSize / gizmoSize, Color.red);

        if (pathObjects.Count >= 0) {
            Vector3 pathObjectPosition = pathObjects[pathObjects.Count - 1].transform.position;

            Gizmos.color = Color.yellow;

            if (path.Count == 0) {
                Gizmos.DrawLine(pathObjectPosition, point);
            } else if (path.Count > 0) {
                Gizmos.DrawLine(path.Last(), point);
            }

            for (int i = 0; i < path.Count; i++) {
                if (i == 0) {
                    Gizmos.DrawLine(pathObjectPosition, path[i]);
                }
                if (i < path.Count - 1) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(path[i], path[i + 1]);
                }
                DrawRectangle(path[i], path[i], 0.1f, Color.yellow);
            }
        }
    }

    private void DrawObjectPlacement() {
        int gizmoSize = 2;

        if (tileSize > 1) {
            gizmoSize *= 2;
        }
        DrawRectangle(point, point, tileSize / gizmoSize, Color.red);
    }

    private void DrawPointPlacement() {
        int gizmoSize = 5;

        if (tileSize > 1) {
            gizmoSize *= 2;
        }
        DrawRectangle(point, point, tileSize / gizmoSize, Color.red);
    }

    private void DrawColliderCreation() {
        Gizmos.color = Color.green;

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

    private void DrawPaths() {
        for (int i = 0; i < pathObjects.Count; i++) {
            if (pathObjects[i] != null) {
                AIControl aiControl = pathObjects[i].GetComponent<AIControl>();

                if (aiControl != null) {
                    if (aiControl.path.Length > 1) {
                        for (int j = 0; j < aiControl.path.Length; j++) {
                            Vector2 pathpoint = aiControl.path[j];

                            DrawRectangle(pathpoint, pathpoint, 0.1f, Color.yellow);
                            Gizmos.color = Color.red;

                            if (j < aiControl.path.Length - 1) {
                                Gizmos.DrawLine(pathpoint, aiControl.path[j + 1]);
                            }
                        }
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

    private void OnScene(SceneView sceneView) {
        Handles.BeginGUI();
        if (Selection.activeGameObject != gameObject) {
            GUILayout.Label("Level GameObject not selected. You need to select it to use all the LevelEditor features", info);

            if (GUILayout.Button("Select Level GameObject")) {
                Selection.activeGameObject = gameObject;
            }
        }
        Handles.EndGUI();
    }
}