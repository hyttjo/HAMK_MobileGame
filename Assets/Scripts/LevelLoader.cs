using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class LevelLoader : MonoBehaviour {

    public Texture2D level;
    public LevelObject[] levelObjects;
    public int spriteSize = 1;
    public bool load = false;

	void Update () {
        if (load) {
            if (level != null) {
                if (levelObjects.Length > 0 && levelObjects != null) {
                    if (level.format == TextureFormat.RGB24) {
                        LoadLevel();
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

    void LoadLevel() {
        int gameObjectNumber = 0;

        for (int x = 0; x < level.width; x++) {
            for (int y = 0; y < level.height; y++) {
                Color32 pixel = level.GetPixel(x, y);

                GameObject go = LevelObject.GetValue(levelObjects, pixel);

                if (go != null) {
                    GameObject.Instantiate(go, new Vector3(x * spriteSize, y * spriteSize, 0), Quaternion.identity, gameObject.transform);
                    gameObjectNumber++;
                } else {
                    if (!pixel.Equals(new Color32(255, 255, 255, 255))) {
                        Debug.Log(pixel.ToString());
                    }
                }
            }
        }
        Debug.Log("level loaded from the texture succesfully! " + gameObjectNumber + " GameObjects created.");
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