using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class StartNewRoundButton : NetworkBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    Button btn = gameObject.GetComponent<Button>();
    btn.onClick.AddListener(TaskOnClick);
  }

  void TaskOnClick()
  {
    Debug.Log("You have clicked the New Round button");
    CmdGetLastRiverCard();
  }

  [Command(requiresAuthority = false)]
  public void CmdGetLastRiverCard()
  {
    GameObject.FindObjectOfType<CardManager>().CmdStartNewRound();
  }
}
