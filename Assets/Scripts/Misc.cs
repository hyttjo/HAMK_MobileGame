using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        if (sprite != null) {
            if (sprite.rect.width != sprite.texture.width) {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGBA32, false);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                             (int)sprite.textureRect.y,
                                                             (int)sprite.textureRect.width,
                                                             (int)sprite.textureRect.height);

                int texSize = newText.width * newText.height;

                if (newColors.Length < texSize) {
                    List<Color> colors = newColors.ToList();

                    int amount = texSize - newColors.Length;

                    for (int i = 0; i < amount; i++) {
                        colors.Add(new Color(0,0,0,0));
                    }
                    newColors = colors.ToArray();
                }
                newText.SetPixels(newColors);
                newText.Apply();

                if (newText.width < 64) {
                    TextureScale.Point(newText, 64, 64);
                }
                return newText;
            } else {
                return sprite.texture;
            }
        } else {
            return null;
        }
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

public class TextureScale
{
	public class ThreadData
	{
	    public int start;
	    public int end;
		public ThreadData (int s, int e) {
			start = s;
			end = e;
		}
	}
 
    private static Color[] texColors;
    private static Color[] newColors;
    private static int w;
    private static float ratioX;
    private static float ratioY;
    private static int w2;
    private static int finishCount;
    private static Mutex mutex;
 
	public static void Point (Texture2D tex, int newWidth, int newHeight)
    {
		ThreadedScale (tex, newWidth, newHeight, false);
	}
 
	public static void Bilinear (Texture2D tex, int newWidth, int newHeight)
    {
		ThreadedScale (tex, newWidth, newHeight, true);
	}
 
	private static void ThreadedScale (Texture2D tex, int newWidth, int newHeight, bool useBilinear)
    {
		texColors = tex.GetPixels();
		newColors = new Color[newWidth * newHeight];
		if (useBilinear)
        {
			ratioX = 1.0f / ((float)newWidth / (tex.width-1));
			ratioY = 1.0f / ((float)newHeight / (tex.height-1));
		}
		else {
			ratioX = ((float)tex.width) / newWidth;
			ratioY = ((float)tex.height) / newHeight;
		}
		w = tex.width;
		w2 = newWidth;
		var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
		var slice = newHeight/cores;
 
		finishCount = 0;
		if (mutex == null) {
			mutex = new Mutex(false);
		}
		if (cores > 1)
		{
		    int i = 0;
		    ThreadData threadData;
			for (i = 0; i < cores-1; i++) {
                threadData = new ThreadData(slice * i, slice * (i + 1));
                ParameterizedThreadStart ts = useBilinear ? new ParameterizedThreadStart(BilinearScale) : new ParameterizedThreadStart(PointScale);
			    Thread thread = new Thread(ts);
				thread.Start(threadData);
			}
			threadData = new ThreadData(slice*i, newHeight);
			if (useBilinear)
            {
				BilinearScale(threadData);
			}
			else
            {
				PointScale(threadData);
			}
			while (finishCount < cores)
            {
                Thread.Sleep(1);
            }
		}
		else
        {
			ThreadData threadData = new ThreadData(0, newHeight);
			if (useBilinear)
            {
				BilinearScale(threadData);
			}
			else
            {
				PointScale(threadData);
			}
		}
 
		tex.Resize(newWidth, newHeight);
		tex.SetPixels(newColors);
		tex.Apply();
	}
 
	public static void BilinearScale (System.Object obj)
	{
	    ThreadData threadData = (ThreadData) obj;
		for (var y = threadData.start; y < threadData.end; y++)
        {
			int yFloor = (int)Mathf.Floor(y * ratioY);
			var y1 = yFloor * w;
			var y2 = (yFloor+1) * w;
			var yw = y * w2;
 
			for (var x = 0; x < w2; x++) {
				int xFloor = (int)Mathf.Floor(x * ratioX);
				var xLerp = x * ratioX-xFloor;
				newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor+1], xLerp),
													   ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor+1], xLerp),
													   y*ratioY-yFloor);
			}
		}
 
		mutex.WaitOne();
		finishCount++;
		mutex.ReleaseMutex();
	}
 
	public static void PointScale (System.Object obj)
	{
	    ThreadData threadData = (ThreadData) obj;
		for (var y = threadData.start; y < threadData.end; y++)
        {
			var thisY = (int)(ratioY * y) * w;
			var yw = y * w2;
			for (var x = 0; x < w2; x++) {
				newColors[yw + x] = texColors[(int)(thisY + ratioX*x)];
			}
		}
 
		mutex.WaitOne();
		finishCount++;
		mutex.ReleaseMutex();
	}
 
	private static Color ColorLerpUnclamped (Color c1, Color c2, float value)
    {
        return new Color (c1.r + (c2.r - c1.r)*value, 
						  c1.g + (c2.g - c1.g)*value, 
						  c1.b + (c2.b - c1.b)*value, 
						  c1.a + (c2.a - c1.a)*value);
    }
}