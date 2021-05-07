using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
public class GetRiverCardButton : NetworkBehaviour
{
  [SerializeField]
  private int mRowNumber = 0;

  [SyncVar]
  private int mClickCount = 0;

  void Start()
  {
    Button btn = gameObject.GetComponent<Button>();
    btn.onClick.AddListener(TaskOnClick);
  }

  void TaskOnClick()
  {
    Debug.Log("You have clicked the Get River Cards button!");
    CmdGetLastRiverCard();
  }

  [Command(requiresAuthority = false)]
  public void CmdGetLastRiverCard()
  {
    if (mClickCount < 1)
    {
      Debug.Log("Revealing river");
      GameObject.FindObjectOfType<CardManager>().RevealRiverHiddenCards(mRowNumber);
      mClickCount++;
      RpcUpdateButtonText("Get a Card");
    }
    else if (mClickCount == 1)
    {
      Debug.Log("Getting first card");
      GameObject.FindObjectOfType<CardManager>().CmdGetLastRiverCard(mRowNumber);
      mClickCount++;
      RpcUpdateButtonText("Get another Card");
    }
    else if (mClickCount > 1)
    {
      Debug.Log("Getting second card");
      GameObject.FindObjectOfType<CardManager>().CmdGetLastRiverCard(mRowNumber);
      // NetworkServer.Destroy(this.gameObject);
      RpcUpdateButtonText("");
      mClickCount++;
    }
  }

  [ClientRpc]
  public void RpcUpdateButtonText(string iText)
  {
    gameObject.GetComponentInChildren<TMP_Text>().text = iText;
  }

  [ClientRpc]
  public void RpcResetButon()
  {
    mClickCount = 0;
    gameObject.GetComponentInChildren<TMP_Text>().text = "Reveal river";
  }
}
