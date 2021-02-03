using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextClicker : MonoBehaviour
{
    public List<GameObject> nextInLine;
    public List<GameObject> awayWithYou;
    public void NextClick()
    {
        foreach (GameObject thing in nextInLine)
        {
            thing.SetActive(true);
        }

        foreach (GameObject thing in awayWithYou)
        {
            thing.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}
