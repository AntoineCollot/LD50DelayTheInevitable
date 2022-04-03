using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour
{
    public Rect playRect;
    public static PlayArea Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public static Vector2 ClampToPlayArea(Vector2 pos)
    {
        pos.x = Mathf.Clamp(pos.x, Instance.playRect.xMin, Instance.playRect.xMax);
        pos.y = Mathf.Clamp(pos.y, Instance.playRect.yMin, Instance.playRect.yMax);
        return pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(playRect.center, playRect.size);
    }
}
