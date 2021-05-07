using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


namespace QuickStart
{
  public class PlayerScript : NetworkBehaviour
  {
    [SyncVar]
    public bool mHasCard = false;
    public Vector3[] mPlayerHandPos;

    public override void OnStartLocalPlayer()
    {
      // Debug.Log("Setting Player Pos");
      // for (int i = 0; i < 6; i++)
      // {
      //   mPlayerHandPos[i] = transform.position + new Vector3(i, 0, 0);
      // }
    }

    void Start()
    {
      mPlayerHandPos = new Vector3[6];
      Debug.Log("Setting Player Pos");
      for (int i = 0; i < 6; i++)
      {
        mPlayerHandPos[i] = transform.position + new Vector3(i * 3.0f, 0.0f, -i * 0.2f);
      }
    }

    void Update()
    {
      if (!isLocalPlayer) { return; }

      // Camera Movement
      float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 40.0f;
      float moveY = Input.GetAxis("Vertical") * Time.deltaTime * 40.0f;
      Camera.main.transform.Translate(new Vector3(moveX, moveY, 0), Space.World);

      // Request Card
      if (!mHasCard && Input.GetKeyDown(KeyCode.R))
      {
        CmdRequestPlayerCards();
      }

      //Pige dans le lac
      if (!mHasCard && Input.GetKeyDown(KeyCode.T))
      {
        CmdRequestPigeDansLeLacCard();
      }

      CameraZoom();
    }

    [Command(requiresAuthority = false)] // needed require autho false ?
    private void CmdRequestPlayerCards()
    {
      Debug.Log("Requestion Cards from player");
      List<Card> wCards = GameObject.FindObjectOfType<CardManager>().CmdGetPlayerCards();
      Debug.Log("Got Card from Card manager");
      Debug.Log($"wCards size: {wCards.Count}");
      int wIndex = 0;
      foreach (var card in wCards)
      {
        Debug.Log($"assiging authority to: {card.name}");
        card.netIdentity.AssignClientAuthority(this.netIdentity.connectionToClient);
        card.gameObject.GetComponent<FaceSelector>().RpcSetFaceUp();
        Debug.Log($"Setting player card back. Player id: {this.connectionToClient.connectionId}");
        card.gameObject.GetComponent<FaceSelector>().RpcSetCardBack(this.connectionToClient.connectionId);
        card.transform.SetPositionAndRotation(mPlayerHandPos[wIndex], Quaternion.identity);
        wIndex++;
      }
      mHasCard = true;
    }

    [Command(requiresAuthority = false)]
    private void CmdRequestPigeDansLeLacCard()
    {
      Card wCard = GameObject.FindObjectOfType<CardManager>().GetPigeDansLeLacCard();
      // wCard.gameObject.GetComponent<FaceSelector>().RpcRevealCard();
      wCard.netIdentity.AssignClientAuthority(this.netIdentity.connectionToClient);
      wCard.gameObject.GetComponent<FaceSelector>().RpcSetFaceUp();
      wCard.gameObject.GetComponent<FaceSelector>().RpcSetCardBack(this.connectionToClient.connectionId);
      wCard.transform.SetPositionAndRotation(mPlayerHandPos[1], Quaternion.identity);

    }

    private void CameraZoom()
    {
      if (Input.GetKey(KeyCode.E) && Camera.main.orthographicSize >= 20)
      {
        Camera.main.orthographicSize -= 2;
      }
      if (Input.GetKey(KeyCode.Q))
      {
        Camera.main.orthographicSize += 2;
      }
    }
  }
}
