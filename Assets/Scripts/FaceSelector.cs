using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FaceSelector : NetworkBehaviour
{
  public Sprite mCardFace;
  public Sprite mCardBack;
  public Sprite[] mCardFaces;

  [SyncVar]
  public string mName;
  [SyncVar]
  public bool mIsRiverCard = false;

  public void SetFaceUp()
  {
    transform.GetComponent<SpriteRenderer>().sprite = mCardFace;
  }

  [ClientRpc]
  public void RpcSetIsRiverCard(bool iIsRiverCard)
  {
    mIsRiverCard = iIsRiverCard;
  }

  [ClientRpc]
  public void RpcSetFaceUp()
  {
    if (hasAuthority || mIsRiverCard)
    {
      SetFaceUp();
    }
  }

  public void SetFaceDown()
  {
    transform.GetComponent<SpriteRenderer>().sprite = mCardBack;
  }

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
    // if (hasAuthority)
    // {
    //   Debug.Log("setting Faceup");
    //   //if (mOnAuthorityFirstPass)
    //   {
    //     SetFaceUp();
    //     mOnAuthorityFirstPass = false;
    //   }
    // }
  }

  private void InitializeCardFace()
  {
    //this.GetComponentInParent<SpriteRenderer>().sprite = FindCardSprite(mName);
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
}
