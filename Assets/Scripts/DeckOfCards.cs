using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class DeckOfCards : NetworkBehaviour
{
  public Dictionary<string, Card> mCards = new Dictionary<string, Card>();
  public List<string> mCardNames = new List<string>();

  private string[] mNumber = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
  private string[] mSuit = new string[] { "C", "D", "H", "S" };

  public GameObject _CardObjectPrefab;

  // Start is called before the first frame update
  void Start()
  {
    InitializeDeck();
  }

  public void InitializeDeck()
  {
    mCardNames = new List<string>();

    foreach (string number in mNumber)
    {
      foreach (string suit in mSuit)
      {
        mCardNames.Add(number + suit);
      }
    }

    GenerateCards();
    GameObject.FindObjectOfType<CardManager>().AssignDeck(this);
  }

  private void GenerateCards()
  {
    var wOffsetZ = 0.0f;
    foreach (string card in mCardNames)
    {
      GameObject wTempsCard = Instantiate(_CardObjectPrefab, new Vector3(-80f, 45f, wOffsetZ), Quaternion.identity);
      // Set Card name so it can find its sprite
      wTempsCard.name = card;
      wTempsCard.GetComponent<FaceSelector>().mName = card;

      // Offset cards for better selection
      wOffsetZ += 0.015f;

      mCards.Add(card, wTempsCard.GetComponent<Card>());
      NetworkServer.Spawn(wTempsCard);
    }
  }

  public Card GetRandomCard()
  {
    var wCardName = mCardNames[Random.Range(0, mCardNames.Count - 1)];
    if (mCards.TryGetValue(wCardName, out Card oCard))
    {
      mCards.Remove(wCardName);
      mCardNames.Remove(wCardName);
    }

    return oCard;
  }

  public Card PlaceRiverCard()
  {
    Card wCard = GetRandomCard();
    wCard.GetComponentInParent<FaceSelector>().mIsRiverCard = true;
    wCard.GetComponentInParent<FaceSelector>().RpcSetIsRiverCard(true); // Needed for timing purposes
    wCard.GetComponentInParent<FaceSelector>().RpcSetFaceUp();

    return wCard;
  }

  public void PrintDeck()
  {
    Debug.Log($"name: {this.name}, mCardNames size: {mCardNames.Count}");
    foreach (var card in mCardNames)
    {
      Debug.Log(card);
    }
  }

  [Command(requiresAuthority = false)]
  public void CmdDeleteCards()
  {
    var wCards = GameObject.FindObjectsOfType<Card>();
    foreach (var card in wCards)
    {
      Debug.Log($"Destroying: {card.name}");
      NetworkServer.Destroy(card.gameObject);
    }
  }
}
