using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class LevelLoader : MonoBehaviour {

    public Texture2D level;
    public Texture2D levelCollider;
    public Color32 colliderColor = new Color(255, 255, 0, 255);
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
                camControl.cameraBounds = new Rect(7, 5, level.width - 7, level.height - 5);
            }
        }
    }

    void CreateLevelColliders() {
        GameObject colliders = new GameObject("Colliders");
        colliders.transform.parent = gameObject.transform;
        EdgeCollider2D eCollider = colliders.AddComponent<EdgeCollider2D>();

        List<Vector2> points = new List<Vector2>();

        Vector2[] pointNeighbours = new Vector2[] { new Vector2(0, 1),
                                                     new Vector2(1, 0),
                                                     new Vector2(0, -1),
                                                     new Vector2(-1, 0)  };

        for (int x = 0; x < levelCollider.width; x++) {
            for (int y = 0; y < levelCollider.height; y++) {
                Color32 pixel = levelCollider.GetPixel(x, y);

                if (pixel.Equals(colliderColor)) {
                    Vector2 point = new Vector2(x, y);

                    while (!points.Contains(point)) {
                        points.Add(point);

                        foreach (Vector2 pointNeighbour in pointNeighbours) {
                            int neighbourX = (int)pointNeighbour.x + (int)point.x;
                            int neighbourY = (int)pointNeighbour.y + (int)point.y;

                            Color32 pixelNeighbour = levelCollider.GetPixel(neighbourX, neighbourY);

                            if (pixelNeighbour.Equals(colliderColor)) {
                                Vector2 neighbourPoint = new Vector2(neighbourX, neighbourY);

                                if (!points.Contains(neighbourPoint)) {
                                    point = neighbourPoint;
                                }
                            }
                        }
                    }
                }
            }
        }

        Vector2 pointOffset = new Vector2(-0.5f, 0.5f);

        for (int i = 0; i < points.Count; i++) {
            Debug.Log(points.Count + ": " + points[i].ToString());
            points[i] += pointOffset;
        }

        if (points.Count > 1) {
            eCollider.points = points.ToArray();
            Debug.Log("Collider creation successful! (Total: " + points.Count + " points).");
        } else {
            Debug.Log("Too few collider points provided, can't assign them (Total: " + points.Count + ").");
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