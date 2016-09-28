using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour {
    public Dictionary<Position, GameObject> gameObjects;

    void Start() {
        SetupLevelData();
    }

    void SetupLevelData() {
        gameObjects = new Dictionary<Position, GameObject>();

        GameObject[] arrayGameObjects = GameObject.FindGameObjectsWithTag("Ground");

        foreach (GameObject _gameObject in arrayGameObjects) {
            Vector2 position = _gameObject.transform.position;
            gameObjects.Add(new Position(position), _gameObject);
        }
    }

    public GameObject GetGameObject(Vector2 vector) {
        GameObject _gameObject;

        if (gameObjects.TryGetValue(new Position(vector), out _gameObject)) { }

        return _gameObject;
    }

    public void SetGameObject(int x, int y, GameObject gameObject) {
        gameObjects.Add(new Position(x, y), gameObject);
    }

    public void SetGameObject(Vector2 vector, GameObject gameObject) {
        gameObjects.Add(new Position(vector), gameObject);
    }
}

public struct Position {
    public int x;
    public int y;

    public Position(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public Position(Vector2 vector) {
        this.x = (int)vector.x;
        this.y = (int)vector.y;
    }
}

public enum Direction { none, up, down, left, right };

public static class Dir {

    public static Direction GetDirection(int prevX, int prevY, int x, int y) {
        if (x == prevX && y == prevY + 1) {
            return Direction.up;
        }
        if (x == prevX && y == prevY - 1) {
            return Direction.down;
        }
        if (x == prevX - 1 && y == prevY) {
            return Direction.left;
        }
        if (x == prevX + 1 && y == prevY) {
            return Direction.right;
        }
        return Direction.none;
    }

    public static Direction GetDirection(Vector2 prevVector, Vector2 vector) {
        return GetDirection((int)prevVector.x, (int)prevVector.y, (int)vector.x, (int)vector.y);
    }
}