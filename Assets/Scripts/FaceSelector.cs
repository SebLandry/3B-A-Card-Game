using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class FaceSelector : NetworkBehaviour
{
  public Sprite mCardFace;
  public Sprite mCardBack;
  public Sprite[] mCardFaces;

  [SyncVar]
  public string mName;
  [SyncVar]
  public bool mIsRiverCard = false;
  [SyncVar]
  public int mRow = 0;

  void Start()
  {
    mCardFaces = Resources.LoadAll<Sprite>("CardsPng");
    this.gameObject.name = mName;
    InitializeCardFace();
    if (mIsRiverCard)
    {
      SetFaceUp();
    }
    else
    {
      SetFaceDown();
    }
  }

  private bool mOnAuthorityFirstPass = true;
  void Update()
  {

  }

  private void InitializeCardFace()
  {
    mCardFace = FindCardSprite(mName);
  }
  public Sprite FindCardSprite(string iName)
  {
    foreach (Sprite wFace in mCardFaces)
    {
      if (iName == wFace.name)
      {
        return wFace;
      }
    }
    // Should not get here
    Debug.Log("Error: CardFace NOT found!!");
    return mCardBack;
  }

  [ClientRpc]
  public void RpcSetIsRiverCard(bool iIsRiverCard)
  {
    mIsRiverCard = iIsRiverCard;
  }
  public void SetFaceUp()
  {
    transform.GetComponent<SpriteRenderer>().sprite = mCardFace;
  }

  [ClientRpc]
  public void RpcSetFaceUp()
  {
    if (hasAuthority || mIsRiverCard)
    {
      SetFaceUp();
    }
  }

  [ClientRpc]
  public void RpcRevealPlayerCards()
  {
    SetFaceUp();
  }

  public void SetFaceDown()
  {
    transform.GetComponent<SpriteRenderer>().sprite = mCardBack;
  }

  private string[] mCardBackNames = new string[6] { "blue_back", "gray_back", "green_back", "red_back", "purple_back", "yellow_back" };
  [ClientRpc]
  public void RpcSetCardBack(int iPlayerId)
  {
    var name = mCardBackNames[iPlayerId];
    Debug.Log($"Setting Card Back for local card. looking for: {name}");
    foreach (Sprite cardFace in mCardFaces)
    {
      if (cardFace.name == mCardBackNames[iPlayerId])
      {
        Debug.Log("Found a card back match");
        mCardBack = cardFace;
        break;
      }
    }
    if (!hasAuthority)
    {
      SetFaceDown();
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("RowCollider"))
    {
      Debug.Log($"Collided with: {other.name}");
      mRow = Int32.Parse(other.name.Substring(other.name.Length - 1)); // Assign the row number in the object name
    }
  }
}
