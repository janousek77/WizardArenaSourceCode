using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//modified from https://www.youtube.com/watch?v=fbUOG7f3jq8

public class TextSpawner : MonoBehaviour {
    public GameObject popUp;
    private static GameObject canvas;
    
    public static void Initialize() {
        canvas = GameObject.Find("Canvas");
    }

    public void makeText(string text, Color c, Transform pos) {
        GameObject newText = Instantiate(popUp);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos.position);
        newText.transform.SetParent(canvas.transform, false);
        newText.transform.position = screenPos;
        newText.GetComponent<floatingTextBehavior>().setText(text);
        newText.GetComponent<floatingTextBehavior>().setColor(c);
    }
}
