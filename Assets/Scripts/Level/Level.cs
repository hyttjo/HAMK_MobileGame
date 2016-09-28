using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour {
    public Dictionary<Position, GameObject> objects;

    void Start() {
        SetupLevelData();
    }

    void SetupLevelData() {
        objects = new Dictionary<Position, GameObject>();

        GameObject playingLayer = GameObject.Find("PlayingLayer");

        GameObject[] arrayGameObjects = Misc.GetChildren(playingLayer);

        foreach (GameObject _gameObject in arrayGameObjects) {
            Vector2 position = _gameObject.transform.position;
            objects.Add(new Position(position), _gameObject);
        }
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