using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;

public class CardManager : NetworkBehaviour
{
  private DeckOfCards mDeck;
  public GameObject[] mTopPos;
  public GameObject[] mMiddlePos;
  public GameObject[] mBottomPos;
  public GameObject _DeckPrefab;
  private int[] mLastRiverCardPlaced = new int[] { 0, 0, 0 };

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
    for (int idx = 0; idx < 6; idx++)
    {
      var wRandomCard = mDeck.GetRandomCard();

      wReturnCards.Add(wRandomCard);
    }
    return wReturnCards;
  }

  [Server]
  public void CmdGetLastRiverCard(int iRow)
  {
    // if ( GameObject.Find($"RowCollider{iRow}").GetComponent<BoxCollider>().)
    Debug.Log($"Row{iRow} card count: {CountRowPlayerCards(iRow)}, Network player count: {NetworkServer.connections.Count}");
    //if (CountRowPlayerCards(iRow) < (2 * NetworkServer.connections.Count)) { return; }
    switch (iRow)
    {
      case 1:
        if (mLastRiverCardPlaced[0] == 0)
        {
          mDeck.PlaceRiverAdditionnalCard().transform.position = mTopPos[1].transform.position + new Vector3(3, 3, 0);
          mLastRiverCardPlaced[0]++;
        }
        else if (mLastRiverCardPlaced[0] == 1)
        {
          Debug.Log($"Handing out last cards for row {iRow}");
          mDeck.PlaceRiverAdditionnalCard().transform.position = mTopPos[2].transform.position + new Vector3(3, 3, 0);
          mLastRiverCardPlaced[0]++;
        }
        break;
      case 2:
        if (mLastRiverCardPlaced[1] == 0)
        {
          mDeck.PlaceRiverAdditionnalCard().transform.position = mMiddlePos[1].transform.position + new Vector3(3, 3, 0);
          mLastRiverCardPlaced[1]++;
        }
        else if (mLastRiverCardPlaced[1] == 1)
        {
          Debug.Log($"Handing out last cards for row {iRow}");
          mDeck.PlaceRiverAdditionnalCard().transform.position = mMiddlePos[2].transform.position + new Vector3(3, 3, 0);
          mLastRiverCardPlaced[1]++;
        }
        break;
      case 3:
        if (mLastRiverCardPlaced[2] == 0)
        {
          mDeck.PlaceRiverAdditionnalCard().transform.position = mBottomPos[1].transform.position + new Vector3(3, 3, 0);
          mLastRiverCardPlaced[2]++;
        }
        else if (mLastRiverCardPlaced[2] == 1)
        {
          Debug.Log($"Handing out last cards for row {iRow}");
          mDeck.PlaceRiverAdditionnalCard().transform.position = mBottomPos[2].transform.position + new Vector3(3, 3, 0);
          mLastRiverCardPlaced[2]++;
        }
        break;
      case 0:
        Debug.LogError("Wrong row selected from button press");
        return;
    }
    RevealRiverHiddenCards(iRow);
  }

  [Server]
  private int CountRowPlayerCards(int iRow)
  {
    return GameObject.FindGameObjectsWithTag($"Row{iRow}").Length;
  }

  [Server]
  public void StartNewRound()
  {
    // Reset Rows
    Debug.Log("New Round Button Pressed!");
    mLastRiverCardPlaced[0] = 0;
    mLastRiverCardPlaced[1] = 0;
    mLastRiverCardPlaced[2] = 0;

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

  [Server]
  public Card GetPigeDansLeLacCard()
  {
    //StartNewRound();
    Debug.Log("Card Manager: getting card from deck.");

    var wRandomCard = mDeck.GetRandomCard();
    return wRandomCard;
  }

  public void RevealRiverHiddenCards(int iRow)
  {
    foreach (var card in GameObject.FindObjectsOfType<FaceSelector>())
    {
      if (card.mRow == iRow)
      {
        card.RpcRevealCard();
      }
    }
  }

  [Server]
  public void RevealAllCards()
  {
    var wCards = GameObject.FindObjectsOfType<FaceSelector>();
    Debug.Log($"Revealing card from card manager. Card amount: {wCards.Length}");
    foreach (var card in wCards)
    {
      card.RpcRevealCard();
    }
  }
}
