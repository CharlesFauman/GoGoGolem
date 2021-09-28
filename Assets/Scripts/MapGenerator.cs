using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class MapGenerator : MonoBehaviour {
	
	public int gridSize;
	public float blockSize;
	public static List<GameObject> blocks;
	public List<GameObject> blockFabs;

	void Start () {
		//blockFabs = (Resources.LoadAll<GameObject>("Assets/Prefabs/Blocks")).ToList();
		blocks = new List<GameObject> ();
		//blockFabs = 
		LoadMap (1);
	}
	
	private bool LoadMap (int level) {
		try {
			string line;
			int lineNumber = 0;

			string fileName = "Assets/Maps/Level " +level.ToString ()+ ".txt";
			StreamReader Reader = new StreamReader(fileName, Encoding.Default);
			
			using (Reader){
				do{
					line = Reader.ReadLine ();
					if (line != null){
						List<int> variables = new List<int>( Array.ConvertAll(line.Split(','), new Converter<string, int>((s)=>{return Convert.ToInt32(s);}) ) );
						switch (lineNumber) {
						case 0:
							gridSize = variables[0];
							blockSize = 1/(float)gridSize;
							break;
						default:
							for(int y=0; y<gridSize;y++){
								for(int x=0; x<gridSize;x++){
									Vector3 pos = new Vector3(blockSize*(x+.5f)-.5f,blockSize*(y+.5f)-.5f,-1);
									Vector3 size = new Vector3(blockSize,blockSize,1);
									GameObject block = (GameObject)Instantiate(blockFabs[variables[y*gridSize + x]],pos,Quaternion.identity);
									block.transform.parent = transform;
									block.transform.localPosition = pos;
									block.transform.localScale = size;
								}
							}
							break;
						}
						lineNumber++;
					}
				} while (line !=null);
				
				Reader.Close ();
				return true;
			}
		} catch (IOException e){
			Debug.LogException(e, this);
			return false;
		}
	}
}
