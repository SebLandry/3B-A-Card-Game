using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragObject : NetworkBehaviour
{
  private Vector3 mOffset;
  private float mZCoord;
  void OnMouseDown()
  {
    if (hasAuthority)
    {
      // Store offset
      mOffset = gameObject.transform.position - GetMouseWorldPos();
      mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    }
  }

  private Vector3 GetMouseWorldPos()
  {
    // Pixel coordinates (x,y)
    Vector3 mousePoint = Input.mousePosition;
    // z coordinates of game object on screen
    mousePoint.z = mZCoord;

    var wResult = Camera.main.ScreenToWorldPoint(mousePoint);
    wResult.z = 0;

    return wResult;
  }

  void OnMouseDrag()
  {
    if (hasAuthority)
    {
      Vector3 wPos = GetMouseWorldPos() + mOffset;
      MoveCard(wPos);
    }
  }

  [Command]
  private void MoveCard(Vector3 iPos)
  {
    transform.position = iPos;
  }
}
