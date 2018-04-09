using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_Continent : HexMap {

	override public void GenerateMap()
	{
		//First call the base verstion to make all the hexes we need
		base.GenerateMap();

		int numContinents = 2;
		int continentSpacing = numColumns/numContinents;

		Random.InitState(20);
		for (int c = 0; c < numContinents; c++)
		{
			//Make som kind of raised area
			int numSplats = Random.Range(8,12);
			for (int i = 0; i < numSplats; i++)
			{
				int range = Random.Range(6,8);
				int y = Random.Range(range, numRows - range);
				int x = Random.Range(0,10) - y/2 + (c*continentSpacing);

				ElevateArea(x, y, range);
			}
		}

	
		//Add lumpiness Perlin Noise?
		float noiseResolution = 0.1f;
		Vector2 noiseOffset = new Vector2(Random.Range(0f,1f), Random.Range(0f,1f));
		float noiseScale = 2f; //Larger values makes more islands and lakes
		for (int column = 0; column < numColumns; column++)
		{
			for (int row = 0; row < numRows; row++)
			{
				Hex h = GetHexAt(column, row);
				float noise =
					Mathf.PerlinNoise(
						((float)column/Mathf.Max(numColumns,numRows)/noiseResolution) + noiseOffset.x, 
						((float)row/Mathf.Max(numColumns,numRows)/noiseResolution) + noiseOffset.y)
						-0.5f;
				h.Elevation += noise*noiseScale;
			}
		}



		//Set mesh to mountain/hill/flat/water based on height

		//Simulate rainfall/moisture (probably just Perlin it for now) and set plains/grasslands + forest

		//Now make sure all the hex visuals are updated to match data

		UpdateHexVisuals();
	}

	void ElevateArea(int q, int r, int range, float centerHeight = 0.8f)
	{
		Hex centerHex = GetHexAt(q, r);


		Hex[] areaHexes = GetHexesWithinRangeOf(centerHex, range);

		foreach (Hex h in areaHexes)
		{
			/*if(h.Elevation < 0)
				h.Elevation = 0;*/

			h.Elevation = centerHeight * Mathf.Lerp(1f, 0.25f, Hex.Distance(centerHex, h)/range);
		}


	}

	

}
