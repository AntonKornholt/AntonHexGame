using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// The Hex class defines the grid position, world space position, size, neighbours, etc of a Hex Tile.
/// However, it does NOT interact with Unity directly in any way.

public class Hex  {

	// Q + R + S = 0 
	// S = -(Q + R)

	public readonly int Q; //Column
	public readonly int R; //Row
	public readonly int S; //Some Sum

	//Data for map generations and maybe ingame effects
	public float Elevation;
	public float Moisture;

	//used to calculate width of hex.
	static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

	public Hex(int q, int r)
	{
		this.Q = q;
		this.R = r;
		this.S = -(q+r);
	}

	float radius = 1f;
	bool allowWrapEastWest = true;
	bool allowWrapNorthSouth = false;

	//Returns the world-space position of this hex.
	public Vector3 Position()
	{

		return new Vector3(
			HexHorizontalSpacing() * (this.Q + this.R/2f),
			0,
			HexVerticalSpacing() * this.R
		);
	}

	public float HexHeight()
	{
		return radius*2;
	}

	public float HexWidth()
	{
		return WIDTH_MULTIPLIER * HexHeight();
	}

	public float HexVerticalSpacing()
	{
		return HexHeight()*0.75f;
	}
	public float HexHorizontalSpacing()
	{
		return HexWidth();
	}

	public Vector3 PositionFromCamera(Vector3 cameraPosition, float numRows, float numColumns)
	{
		float mapHeight = numRows * HexVerticalSpacing();
		float mapWidth = numColumns * HexHorizontalSpacing();
		
		Vector3 position = Position();

		//We want howManyWidthsFromCamera to be between -0.5 to 0.5.

		/*if(Mathf.Abs(howManyWidthsFromCamera) <= 0.5f)
		{
			//We're good. The code below makes this check obsolete.
			return position;
		}*/

		if(allowWrapEastWest)
		{
			float howManyWidthsFromCamera = (position.x - cameraPosition.x) / mapWidth;

			if(howManyWidthsFromCamera > 0)
				howManyWidthsFromCamera += 0.5f;
			else
				howManyWidthsFromCamera -= 0.5f;

			int howManyWidthToFix = (int) howManyWidthsFromCamera;

			position.x -= howManyWidthToFix * mapWidth;
		}

		if(allowWrapNorthSouth)
		{
			float howManyHeightsFromCamera = (position.z - cameraPosition.z) / mapHeight;
			if(howManyHeightsFromCamera > 0)
				howManyHeightsFromCamera += 0.5f;
			else
				howManyHeightsFromCamera -= 0.5f;

			int howManyHeightsToFix = (int) howManyHeightsFromCamera;

			position.x -= howManyHeightsToFix * mapHeight;
		}

		return position;

	}

	//FIX ME WRAPPING
	public static float Distance(Hex a, Hex b)
	{
		return 
			Mathf.Max(
				Mathf.Abs(a.Q-b.Q),
				Mathf.Abs(a.R-b.R),
				Mathf.Abs(a.S-b.S)
			);

	}
}
