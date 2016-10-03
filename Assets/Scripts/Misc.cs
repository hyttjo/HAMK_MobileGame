using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class Misc {

    public static Vector2 GetHitDirection(Vector2 collider) {
        float angle = Vector3.Angle(collider, Vector3.up);

        if (Mathf.Approximately(angle, 0)) {
            return Vector2.up;
        } else if (Mathf.Approximately(angle, 180)) {
            return Vector2.down;
        }else if (Mathf.Approximately(angle, 90)) {
            Vector3 cross = Vector3.Cross(Vector3.forward, collider);
            if (cross.y > 0) {
                return Vector2.right;
            } else {
                return Vector2.left;
            }
        }
        return Vector2.zero;
    }

    public static Vector2[] GetConnectedVectors(int x, int y, int[][] array, List<Vector2> vectors) {
        bool canUp = (x - 1 >= 0);
        bool canDown = (x + 1 < array.Length);
        bool canRight = (y + 1 < array[0].Length);
        bool canLeft = (y - 1 >= 0);

        int value = array[x][y];
        vectors.Add(new Vector2(x, y));

        array[x][y] = 0;

        if (canUp && array[x - 1][y] == value) {
            GetConnectedVectors(x - 1, y, array, vectors);
        }
        if (canDown && array[x + 1][y] == value) {
            GetConnectedVectors(x + 1, y, array, vectors);
        }
        if (canLeft && array[x][y - 1] == value) {
            GetConnectedVectors(x, y - 1, array, vectors);
        }
        if (canRight && array[x][y + 1] == value) {
            GetConnectedVectors(x, y + 1, array, vectors);
        }

        if (canUp && canLeft && array[x - 1][y - 1] == value) {
            GetConnectedVectors(x - 1, y - 1, array, vectors);
        }
        if (canUp && canRight && array[x - 1][y + 1] == value) {
            GetConnectedVectors(x - 1, y + 1, array, vectors);
        }
        if (canDown && canRight && array[x + 1][y + 1] == value) {
            GetConnectedVectors(x + 1, y + 1, array, vectors);
        }
        if (canDown && canLeft && array[x + 1][y - 1] == value) {
            GetConnectedVectors(x + 1, y - 1, array, vectors);
        }

        return vectors.ToArray();
    }

    public static Vector2[] TrimVectorArray(Vector2[] array) {
        List<Vector2> list = new List<Vector2>();

        int prevX = 0;
        int prevY = 0;
        Direction direction = Direction.none;
        Direction prevDirection = Direction.none;

        for (int i = 0; i < array.Length; i++) {
            int x = (int)array[i].x;
            int y = (int)array[i].y;

            if (i == 0) {
                prevX = x;
                prevY = y;

                prevDirection = Dir.GetDirection(x, y, (int)array[i + 1].x, (int)array[i + 1].y);

                list.Add(array[i]);
            } else if (i == array.Length - 1) {
                list.Add(array[i]);
            } else {
                direction = Dir.GetDirection(prevX, prevY, x, y);

                if (direction != prevDirection) {
                    list.Add(array[i - 1]);

                    if (i == array.Length - 2) {
                        if (Vector2.Distance(array[i - 1], array[i]) < 1.1f) {
                            list.Add(array[i]);

                        }
                    }
                }

                prevX = x;
                prevY = y;
                prevDirection = direction;
            }
        }
        return list.ToArray();
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

    public static GameObject[] GetChildren(this GameObject go) {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform tran in go.transform) {
            children.Add(tran.gameObject);
        }
        return children.ToArray();
    }

    public static bool IsSelectionValid(Vector3 start, Vector3 end) {
        if (start.x > end.x) {
            return false;
        } else if (start.y > end.y) {
            return false;
        } else {
            return true;
        }
    }

    public static bool IsInsideGrid(Level level, Vector3 position) {
        if (position.x < 0) {
            return false;
        } else if (position.y < 0) {
            return false;
        } else if (position.x > level.width * level.tileSize) {
            return false;
        } else if (position.y > level.height * level.tileSize) {
            return false;
        } else {
            return true;
        }
    }

    public static Texture2D GetTextureFromSprite(Sprite sprite) {
        if (sprite.rect.width != sprite.texture.width) {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);

            if (newColors.Length >= newText.width * newText.height) {
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            } else {
                return null;
            }
        } else
            return sprite.texture;
    }

    public static Texture2D GetTextureFromObject(GameObject gameObject) {
        if (gameObject != null) {
            SpriteRenderer sRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

            if (sRenderer != null) {
                Sprite sprite = sRenderer.sprite;

                if (sprite != null) {
                    Texture2D texture = GetTextureFromSprite(sprite);

                    if (texture != null) {
                        texture.filterMode = FilterMode.Point;
                        return texture;
                    }
                }
            }
        }
        return null;
    }
}