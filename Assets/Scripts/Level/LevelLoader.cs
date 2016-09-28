using UnityEngine;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(Level))]
public class LevelLoader : MonoBehaviour {

    public Texture2D level;
    public Texture2D levelCollider;
    public Color32 colliderColor = new Color(255, 255, 0, 255);
    public Color32 colliderStartColor = new Color(255, 0, 0, 255);
    public GameObject bottomPit;
    public int spriteSize = 1;
    public bool load = false;
    public LevelObject[] levelObjects;
    
	void Update () {
        if (load) {
            if (level != null) {
                if (levelObjects.Length > 0 && levelObjects != null) {
                    if (level.format == TextureFormat.RGB24) {
                        ResetLevel();
                        LoadLevel();
                        SetupPlayerCamera();

                        if (levelCollider != null) {
                            if (levelCollider.format == TextureFormat.RGB24) {
                                CreateLevelColliders();
                            } else {
                                Debug.Log("Level Collider texture fromat is incorrect! Use RGB24 format.");
                            }
                        }

                        if (bottomPit != null) {
                            CreateBottomPit();
                        }
                    } else {
                        Debug.Log("Level texture fromat is incorrect! Use RGB24 format.");
                    }
                } else {
                    Debug.Log("GameObjects dictionary is not created!");
                }
            } else {
                Debug.Log("Level Texture is not set!");
            }
        }
        load = false;
	}

    void ResetLevel() {
        Transform[] children = new Transform[transform.childCount];
        int nextElement = 0;

        foreach (Transform obj in transform) {
            children[nextElement++] = obj;
        }

        foreach (Transform obj in children) {
            if (null != obj) {
                GameObject.DestroyImmediate(obj.gameObject);
            }
        }
    }

    void LoadLevel() {
        GameObject container = new GameObject("Container");
        container.transform.parent = gameObject.transform;

        int gameObjectNumber = 0;

        for (int x = 0; x < level.width; x++) {
            for (int y = 0; y < level.height; y++) {
                Color32 pixel = level.GetPixel(x, y);

                GameObject go = LevelObject.GetValue(levelObjects, pixel);

                Transform parent = container.transform;

                if (go != null) {
                    if (go.tag == "Player") {
                        parent = gameObject.transform;
                    }

                    GameObject.Instantiate(go, new Vector3(x * spriteSize, y * spriteSize, 0), Quaternion.identity, parent);
                    gameObjectNumber++;
                } else {
                    if (!pixel.Equals(new Color32(255, 255, 255, 255))) {
                        Debug.Log("LevelObject is not set in the array! Level color was " + pixel.ToString());
                    }
                }
            }
        }
        Debug.Log("Level loaded from the texture succesfully! " + gameObjectNumber + " GameObjects created.");
    }

    void SetupPlayerCamera() {
        Camera cam = Camera.main;

        if (cam != null) {
            CameraControl camControl = cam.GetComponent<CameraControl>();

            if (camControl != null) {
                camControl.cameraBounds = new Rect(8, 7, level.width - 8, level.height - 7);
            }
        }
    }
    
    void CreateLevelColliders() {
        GameObject colliders = new GameObject("Colliders");
        colliders.transform.parent = gameObject.transform;

        int[][] levelColliderArray = new int[levelCollider.width][];
        List<Vector2> startVectors = new List<Vector2>();
        List<Vector2> colliderVectors = new List<Vector2>();

        for (int x = 0; x < levelCollider.width; x++) {
            levelColliderArray[x] = new int[levelCollider.height];

            for (int y = 0; y < levelCollider.height; y++) {
                Color32 pixel = levelCollider.GetPixel(x, y);

                if (pixel.Equals(colliderColor) || pixel.Equals(colliderStartColor)) {
                    levelColliderArray[x][y] = 1;
                } else {
                    levelColliderArray[x][y] = 0;
                }

                if (pixel.Equals(colliderStartColor)) {
                    startVectors.Add(new Vector2(x, y));
                }
            }
        }

        for (int i = 0; i < startVectors.Count; i++) {
            GameObject collider = new GameObject("Collider " + startVectors[i].ToString());
            collider.transform.parent = colliders.transform;

            colliderVectors.Clear();

            EdgeCollider2D eCollider = collider.AddComponent<EdgeCollider2D>();
            Vector2[] connectedVectors = Misc.GetConnectedVectors((int)startVectors[i].x, (int)startVectors[i].y, levelColliderArray, colliderVectors);
            eCollider.points = Misc.TrimVectorArray(connectedVectors);
            eCollider.offset = new Vector2(-0.5f, -0.5f);

            PhysicsMaterial2D physicsMaterial = (PhysicsMaterial2D)Resources.Load("Materials/Grass");

            if (physicsMaterial != null) {
                eCollider.sharedMaterial = physicsMaterial;
            }
        }
    }

    void CreateBottomPit() {
        GameObject _bottomPit = (GameObject)Instantiate(bottomPit, Vector3.zero, Quaternion.identity, transform);

        EdgeCollider2D eCollider = _bottomPit.GetComponent<EdgeCollider2D>();

        if (eCollider != null) {
            eCollider.points = new Vector2[] { new Vector2(-10, 0), new Vector2(level.width + 10, 0) };
        }
    }
}

[Serializable]
public class LevelObject {
    public Color32 color;
    public GameObject gameObject;

    static public GameObject GetValue(LevelObject[] array, Color32 key) {
        key.a = 255;

        for (int i = 0; i < array.Length; i++) {
            if (key.Equals(array[i].color)) {
                return array[i].gameObject;
            }
        }
        return null;
    }
}