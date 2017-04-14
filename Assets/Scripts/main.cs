/*
 * this module manages the whole scene
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour {
    public GameObject arrowred;
    public GameObject arrowblack;
    public GameObject arrowgreen;
	public GameObject obs;
	public Text connter;
	int red = 10;
	int black = 10;
	int green = 20;
    List<Fish> f = new List<Fish>();
	List<obstacle> listObs = new List<obstacle>();
    private void Awake()
    {
		listObs.Add (new obstacle (Instantiate (obs)));
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < 10; j++)
                {
                    f.Add(new Fish(Instantiate(arrowred), i));
                }
            }
			if (i == 1)
			{
				for (int j = 0; j < 10; j++)
				{
					f.Add(new Fish(Instantiate(arrowblack), i));
				}
			}
			if (i == 2)
			{
				for (int j = 0; j < 20; j++)
				{
					f.Add(new Fish(Instantiate(arrowgreen), i));
				}
			}
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.R)) {
			reset ();
		}
		red = 0;
		green = 0;
		black = 0;
		foreach (Fish d in f) {
			switch (d.r.i) {
			case 0:
				red++;
				break;
			case 1:
				black++;
				break;
			case 2:
				green++;
				break;
			}
			foreach (obstacle o in listObs) {
				o.push(d);
			}
			d.Update (ref f, ref listObs, Time.fixedDeltaTime);
		}
		connter.text = string.Format ("Counter\nRed:{0}\nBlack:{1}\nGreen:{2}", red, black, green);;

	}

	void reset() {
		Application.LoadLevel (0); 
	}


}
