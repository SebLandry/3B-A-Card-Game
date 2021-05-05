using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class CardManager : NetworkBehaviour
{
  private DeckOfCards mDeck;
  public GameObject[] mTopPos;
  public GameObject[] mMiddlePos;
  public GameObject[] mBottomPos;
  public GameObject[] mPlayerHandPos;
  public GameObject _DeckPrefab;
  private bool[] mLastRiverCardPlaced = new bool[] { false, false, false };

  // Start is called before the first frame update
  void Start()
  {
    Debug.Log("Creating Deck of Cards");
    Instantiate(_DeckPrefab);
  }

  // Update is called once per frame
  void Update()
  {
    if (!isLocalPlayer)
    {
      return;
    }
  }

  public void AssignDeck(DeckOfCards iDeck)
  {
    mDeck = iDeck;

    // Temporary
    PlaceCenterCards();
  }

  public void PlaceCenterCards()
  {
    foreach (var pos in mTopPos)
    {
      mDeck.PlaceRiverCard().transform.position = pos.transform.position;
    }
    foreach (var pos in mMiddlePos)
    {
      mDeck.PlaceRiverCard().transform.position = pos.transform.position;
    }
    foreach (var pos in mBottomPos)
    {
      mDeck.PlaceRiverCard().transform.position = pos.transform.position;
    }
  }

  [Server]
  public List<Card> CmdGetPlayerCards()
  {
    Debug.Log("Card Manager: getting card from deck.");
    List<Card> wReturnCards = new List<Card>();
    foreach (var pos in mPlayerHandPos)
    {
      var wRandomCard = mDeck.GetRandomCard();
      wRandomCard.transform.position = pos.transform.position;

      wReturnCards.Add(wRandomCard);
    }
    return wReturnCards;
  }

  [Server]
  public void CmdGetLastRiverCard(int iRow)
  {
    switch (iRow)
    {
      case 1:
        if (!mLastRiverCardPlaced[0])
        {
          mDeck.PlaceRiverCard().transform.position = mTopPos[1].transform.position + new Vector3(3, 3, 0);
          mDeck.PlaceRiverCard().transform.position = mTopPos[2].transform.position + new Vector3(3, 3, 0);
          Debug.Log($"Handing out last cards for row {iRow}");
          mLastRiverCardPlaced[0] = true;
        }
        break;
      case 2:
        if (!mLastRiverCardPlaced[1])
        {
          mDeck.PlaceRiverCard().transform.position = mMiddlePos[1].transform.position + new Vector3(3, 3, 0);
          mDeck.PlaceRiverCard().transform.position = mMiddlePos[2].transform.position + new Vector3(3, 3, 0);
          Debug.Log($"Handing out last cards for row {iRow}");
          mLastRiverCardPlaced[1] = true;
        }
        break;
      case 3:
        if (!mLastRiverCardPlaced[2])
        {
          mDeck.PlaceRiverCard().transform.position = mBottomPos[1].transform.position + new Vector3(3, 3, 0);
          mDeck.PlaceRiverCard().transform.position = mBottomPos[2].transform.position + new Vector3(3, 3, 0);
          Debug.Log($"Handing out last cards for row {iRow}");
          mLastRiverCardPlaced[2] = true;
        }
        break;
      case 0:
        Debug.LogError("Wrong row selected from button press");
        return;
    }
    RevealPlayerHiddenCards(iRow);
  }

  [Command(requiresAuthority = false)]
  public void CmdStartNewRound()
  {
    // Reset Rows
    Debug.Log("New Round Button Pressed!");
    mLastRiverCardPlaced[0] = false;
    mLastRiverCardPlaced[1] = false;
    mLastRiverCardPlaced[2] = false;

    // Delete cards
    Debug.Log("Deleting Cards");
    var wCards = GameObject.FindObjectsOfType<Card>();
    foreach (var card in wCards)
    {
      Debug.Log($"Destroying: {card.name}");
      NetworkServer.Destroy(card.gameObject);
    }
    // delete deck
    Debug.Log("Deleting Deck");
    var wDeck = GameObject.FindObjectOfType<DeckOfCards>();
    NetworkServer.Destroy(wDeck.gameObject);

    // delete player card
    var wPlayers = GameObject.FindGameObjectsWithTag("Player");
    foreach (var wPlayer in wPlayers)
    {
      wPlayer.GetComponent<QuickStart.PlayerScript>().mHasCard = false;
    }

    // Spawn new deck
    Debug.Log("Deck deleted, creating new deck");
    Instantiate(_DeckPrefab);
  }

  public void RevealPlayerHiddenCards(int iRow)
  {
    foreach (var card in GameObject.FindObjectsOfType<FaceSelector>())
    {
      if (card.mRow == iRow)
      {
        card.RpcRevealPlayerCards();
      }
    }
  }
}
