using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		GenerateMap();
	}
	
	
	public GameObject HexPrefab;

	public Mesh MeshWater;
	public Mesh MeshFlat;
	public Mesh MeshHill;
	public Mesh MeshMountain;

	//public Material[] HexMaterials;

	public Material MatOcean;
	public Material MatPlains;
	public Material MatGrassLands;
	public Material MatMountains;
	//Tiles with height above a certain value is something.
	public float HeightMountain = 1f;
	public float HeightHill = 0.6f;
	public float HeightFlat = 0.0f;
	public int numRows = 30;
	
	public int numColumns = 60;

	bool allowWrapEastWest = true;
	bool allowWrapNorthSouth = false;

	private Hex[,] hexes;
	private Dictionary<Hex, GameObject> hexToGameObjectMap;

	public Hex GetHexAt(int x, int y)
	{
		if(hexes == null)
		{
			Debug.LogError("Hexes array not yet instantiated");
			return null;
		}
		if(allowWrapEastWest)
			x = x % numColumns;
			if (x < 0)
			{
				x += numColumns;
			}
		if(allowWrapNorthSouth)
			y = y % numRows;

		try
		{
			return hexes[x, y];
		}
		catch
		{
			Debug.LogError("GetHexAt: " + x + "," + y);
			return null;
		}

	}
	
	virtual public void GenerateMap()
	{

		hexes = new Hex[numColumns, numRows];
		hexToGameObjectMap = new Dictionary<Hex, GameObject>();

		// Generate a map filled with ocean

		for (int column = 0; column < numColumns; column++)
		{
			

			for (int row = 0; row < numRows; row++)
			{

				Hex h = new Hex(column, row);
				h.Elevation = -0.5f;
				hexes[column, row] = h;
				
				
				Vector3 pos = h.PositionFromCamera(
					Camera.main.transform.position,
					numRows,
					numColumns
				);

				GameObject hexGO = (GameObject)Instantiate(
					HexPrefab, 
					pos, 
					Quaternion.identity,
					this.transform
				);

				hexToGameObjectMap[h] = hexGO;

				hexGO.name = string.Format("HEX: {0}, {1}", column, row);
				hexGO.GetComponent<HexComponent>().Hex = h;
				hexGO.GetComponent<HexComponent>().HexMap = this;
				
				hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

				MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
				mr.material = MatOcean;

				MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();
				mf.mesh = MeshWater;

				

			}
			
		}
	
	//Makes the map static - can't move the map or tiles anymore but uses fewer batches
	//StaticBatchingUtility.Combine(this.gameObject);

	}	
	
	public void UpdateHexVisuals()
	{

		for (int column = 0; column < numColumns; column++)
		{
			for (int row = 0; row < numRows; row++)
			{
				Hex h = hexes[column,row];
				GameObject hexGO = (GameObject)hexToGameObjectMap[h];

				MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
				if(h.Elevation >= HeightMountain){
					mr.material = MatMountains;
				}
				else if(h.Elevation >= HeightHill){
					mr.material = MatPlains;
				}

				else if(h.Elevation >= HeightFlat){
					mr.material = MatGrassLands;
				}
				else{
					mr.material = MatOcean;
				}
				
				
				MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();
				mf.mesh = MeshWater;
				
			}
		}

	}

	public Hex[] GetHexesWithinRangeOf(Hex centerHex, int range)
	{
		List<Hex> results = new List<Hex>();

		for (int dx = -range; dx < range-1; dx++)
		{
			for (int dy = Mathf.Max(-range+1, -dx-range); dy < Mathf.Min(range, -dx+range-1); dy++)
			{
				results.Add(GetHexAt(centerHex.Q+1 + dx, centerHex.R+1 + dy));
			}
		}

		return results.ToArray();
	}
}
