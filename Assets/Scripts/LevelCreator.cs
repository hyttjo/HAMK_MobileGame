using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class LevelCreator : Editor {

    public GameObject tile;

    public int width = 4;
    public int height = 4;
    public float tileSize = 1;

    public void OnEnable() {
        SceneView.onSceneGUIDelegate += GridUpdate;
    }

    void GridUpdate(SceneView sceneview) {
        Event e = Event.current;
        Camera camera = Camera.current;

        if (camera != null) {
            if (e.isKey && e.character == 'a') {

                Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

                if (tile != null) {
                    GameObject obj = (GameObject)Instantiate(tile);
                    Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / tileSize) * tileSize + tileSize / 2.0f,
                                                  Mathf.Floor(mousePos.y / tileSize) * tileSize + tileSize / 2.0f, 0);
                    obj.transform.position = aligned;

                    Debug.Log(aligned + " " + mousePos);
                }
            }
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
