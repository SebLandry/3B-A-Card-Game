using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Card : NetworkBehaviour
{
  private float wTime = 0;

  void Update()
  {
    //wTime += Time.deltaTime;
    if (wTime > 3)
    {
      if (hasAuthority)
      {
        Debug.Log($"Cards.cs, {this.name} has authority.");
      }
      else
      {
        Debug.Log($"Cards.cs, {this.name} doesn't have authority.");
      }

      if (netIdentity.hasAuthority) Debug.Log($"PlayerScript, {this.name} has network authority.");
      else Debug.Log($"Cards.cs, {this.name} doesn't have network authority.");

      wTime = 0;
    }
  }

  [Command(requiresAuthority = false)]
  public void CmdAssignToPlayer()
  {
    Debug.Log($"assiging authority to: {this.name}");
    netIdentity.AssignClientAuthority(this.netIdentity.connectionToClient);

    // Set the card face up only for the authoritative player
    gameObject.GetComponent<FaceSelector>().SetFaceUp();
  }
}