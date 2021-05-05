using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class GetRiverCardButton : NetworkBehaviour
{
  [SerializeField]
  private int mRowNumber = 0;

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
    GameObject.FindObjectOfType<CardManager>().CmdGetLastRiverCard(mRowNumber);
  }
}
