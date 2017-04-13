using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {
    public GameObject arrowred;
    public GameObject arrowblack;
    public GameObject arrowgreen;
    List<Fish> f = new List<Fish>();
    private void Awake()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < 12; j++)
                {
                    f.Add(new Fish(Instantiate(arrowred), new Rule(i)));
                }
            }
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		foreach (Fish d in f) {
			d.Update (ref f, Time.fixedDeltaTime);
		}
	}
}
