﻿/*
 * Define all the constants here
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants  {
    static public readonly Rect domains = new Rect(-10, -10, 20, 20);
    static public float Radius = 0.35F;
    static public float fovangle = Mathf.Cos(Mathf.PI / 4);
	static public float[] maxspeed = { 4, 3, 2 };
	static public float[] fov = { 2, 2, 2 };

	
}
