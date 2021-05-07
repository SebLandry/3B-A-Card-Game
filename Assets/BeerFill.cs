using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class BeerFill : NetworkBehaviour
{
  public Image[] mDrinks;
  private Coroutine mBeerCoroutine;
  [SyncVar] private float mFillAmount = 0.05f;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    foreach (var drink in mDrinks)
    {
      drink.fillAmount = mFillAmount;
    }
  }

  void OnMouseDown()
  {
    Debug.Log("Filling the Beer glass");
    mBeerCoroutine = StartCoroutine(BeerFillCoroutine());
  }

  void OnMouseUp()
  {
    Debug.Log("Stop filling the Beer glass");
    StopCoroutine(mBeerCoroutine);
  }

  public void ResetBeerGlass()
  {
    Debug.Log("Reset Button clicked, emptying glass..");
    mFillAmount = 0.05f;
    CmdUpdateFill(mFillAmount);
  }

  IEnumerator BeerFillCoroutine()
  {
    while (mFillAmount <= 1f)
    {
      mFillAmount += 0.002f;
      CmdUpdateFill(mFillAmount);
      yield return null;
    }
    Debug.Log("Glass is full");
  }

  [Command(requiresAuthority = false)]
  private void CmdUpdateFill(float iFill)
  {
    mFillAmount = iFill;
  }
}
