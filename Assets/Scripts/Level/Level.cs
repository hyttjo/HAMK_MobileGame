using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour {
    public Dictionary<Position, GameObject> objects;

    private GameObject[] layers;

    void Start() {
        layers = GetLayerGameObjects();
        objects = GetLevelData();
    }

    public GameObject GetGameObject(Vector2 vector) {
        GameObject _gameObject;

        if (objects.TryGetValue(new Position(vector), out _gameObject)) { }

        return _gameObject;
    }

    public void SetGameObject(int x, int y, GameObject gameObject) {
        objects.Add(new Position(x, y), gameObject);
    }

    public void SetGameObject(Vector2 vector, GameObject gameObject) {
        objects.Add(new Position(vector), gameObject);
    }

    public GameObject[] GetLayerGameObjects() {
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

    public Dictionary<Position, GameObject> GetLevelData() {
        Dictionary<Position, GameObject> levelData = new Dictionary<Position, GameObject>();

        for (int i = 0; i < layers.Length; i++) {
            GameObject layer = layers[i];

            GameObject[] arrayGamelevelData = Misc.GetChildren(layer);

            foreach (GameObject _gameObject in arrayGamelevelData) {
                Vector2 position = _gameObject.transform.position;
                levelData.Add(new Position(position), _gameObject);
            }
        }

        return levelData;
    }
}

public struct Position {
    public int x;
    public int y;
    public int z;

    public Position(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Position(int x, int y) {
        this.x = x;
        this.y = y;
        this.z = 0;
    }

    public Position(Vector3 vector) {
        this.x = (int)vector.x;
        this.y = (int)vector.y;
        this.z = (int)vector.z;
    }

    public Position(Vector2 vector) {
        this.x = (int)vector.x;
        this.y = (int)vector.y;
        this.z = 0;
    }

    public override string ToString() {
        return "Position (" + this.x + ", " + this.y + ", " + this.z + ")";
    }
}