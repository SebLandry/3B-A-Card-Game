using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PigeDansLeLac : NetworkBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    Button btn = gameObject.GetComponent<Button>();
    btn.onClick.AddListener(TaskOnClick);
  }

  void TaskOnClick()
  {
    Debug.Log("You have clicked the Pige dans le lac button");
    CmdStartPigeDansLeLac();
  }

  [Command(requiresAuthority = false)]
  public void CmdStartPigeDansLeLac()
  {
    // to do GameObject.FindObjectOfType<CardManager>().StartPigeDansLeLac();
  }
}
