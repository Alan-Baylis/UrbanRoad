﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Game
{
	public class PieceFactory
	{
		static PieceFactory instance = null;
		private static Dictionary<int,PieceConfig> cache;

		private XmlDocument xml;
		private PieceFactory() {
			xml = XmlHelper.getXml ();
			cache = new Dictionary<int,PieceConfig> ();
		}
		public static PieceFactory getInstance(){
			if (instance == null) {
				instance = new PieceFactory ();
			}
			return instance;
		}

		public PieceConfig getConfig(int id){
			
			if (cache.ContainsKey (id)) {
				return cache [id];
			}

			PieceConfig config = new PieceConfig ();
			config.id = id;
			XmlNode node = xml.SelectSingleNode ("//piece[@id = '"+id+"']");
			if (node == null) {
				Debug.LogError ("Id:"+id + " piece not found!");
				return null;
			}
			XmlNodeList blocks = node.SelectNodes ("blocks/block");
			config.collision = getCollision (blocks);
			config.blockCount = blocks.Count;
			config.prefab = getPrefab (node.SelectSingleNode ("prefab"));
			config.placeholder = getTexture(node.SelectSingleNode ("placeholder"));
			cache.Add (id, config);
			return config;
		}

		private GameObject getPrefab(XmlNode node){

			string name = node.Attributes ["name"].Value;
			return (GameObject)Resources.Load ("Prefabs/"+name);
		}

		private Sprite getTexture(XmlNode node){
			string name = node.Attributes ["image"].Value;
			return Resources.Load ("Placeholders/"+name, typeof(Sprite)) as Sprite;
		}

		private List<Vector2> getCollision(XmlNodeList list){
			List<Vector2> output = new List<Vector2>();
			foreach (XmlNode node in list) {
				Vector2 temp = new Vector2 ();
				temp.x = float.Parse( node.Attributes["x"].Value );
				temp.y = float.Parse( node.Attributes["y"].Value );
				output.Add (temp);
			}
			return output;
		}
			
	}


}