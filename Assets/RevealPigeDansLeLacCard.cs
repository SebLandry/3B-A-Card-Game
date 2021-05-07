using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class RevealPigeDansLeLacCard : NetworkBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    Button btn = gameObject.GetComponent<Button>();
    btn.onClick.AddListener(TaskOnClick);
  }

  void TaskOnClick()
  {
    Debug.Log("You have clicked the Reveal Cards button");
    CmdRevealAllCards();
  }

  [Command(requiresAuthority = false)]
  public void CmdRevealAllCards()
  {
    GameObject.FindObjectOfType<CardManager>().RevealAllCards();
  }
}
