using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class LevelCreator : Editor {

    public GameObject tile;
    private GameObject activeGo;

    public int width = 128;
    public int height = 32;
    public float tileSize = 1;

    public void OnEnable() {
        SceneView.onSceneGUIDelegate += GridUpdate;

        activeGo = (GameObject)Instantiate(tile);
        activeGo.transform.position = new Vector3(0.5f, 0.5f, 0);
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
                    activeGo.transform.position = aligned;
                }

                if (e.isKey && e.character == 'a') {
                    if (tile != null) {
                        GameObject obj = (GameObject)Instantiate(tile);

                        obj.transform.position = aligned;
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

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                float y_height = Mathf.Floor(y / tileSize) * tileSize;

                Gizmos.DrawLine(new Vector3(y_height, 0, 0), new Vector3(y_height, height, 0));
            }
            float x_width = Mathf.Floor(x / tileSize) * tileSize;

            Gizmos.DrawLine(new Vector3(0, x_width, 0), new Vector3(width, x_width, 0));
        }

        Gizmos.color = Color.white;

        Gizmos.DrawLine(Vector3.zero, new Vector3(0, height * tileSize, 0));
        Gizmos.DrawLine(new Vector3(0, height * tileSize, 0), new Vector3(width * tileSize, height * tileSize, 0));
        Gizmos.DrawLine(new Vector3(width * tileSize, height * tileSize, 0), new Vector3(width * tileSize, 0, 0));
        Gizmos.DrawLine(new Vector3(width * tileSize, 0, 0), Vector3.zero);
    }


}
