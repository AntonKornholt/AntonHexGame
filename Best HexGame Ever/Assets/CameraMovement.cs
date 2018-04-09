using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		oldPosition = this.transform.position;
	}

	Vector3 oldPosition;
	
	// Update is called once per frame
	void Update () {
		//TODO:
		// code to click and drag camera,
		// WASD,
		// Zoom in and out

		CheckifCameraMoved();
	}

	public void PanToHex(Hex hex){
		//TODO: Move camera to hex
		
	}

	HexComponent[] hexes;
	void CheckifCameraMoved()
	{
		if(oldPosition != this.transform.position)
		{
			//Something moved the camera
			oldPosition = this.transform.position;

			//TODO: Probably Hexmap will have a dictionary of all these later
			if(hexes == null)
				hexes = GameObject.FindObjectsOfType<HexComponent>();
				
			foreach (HexComponent hex in hexes)
			{
				hex.UpdatePosition();
			}

		}
	}
}
