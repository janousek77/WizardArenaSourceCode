using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//modified from https://www.youtube.com/watch?v=fbUOG7f3jq8

public class floatingTextBehavior : MonoBehaviour {
    public Animator animator;

	// Use this for initialization
	void Start () {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setText(string text) {
        animator.GetComponent<Text>().text = text;
    }

    public void setColor(Color c)
    {
        animator.GetComponent<Text>().color = c;
    }
}
