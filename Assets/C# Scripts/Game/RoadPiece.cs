﻿using UnityEngine;
using System.Collections;
namespace Game
{
	public class RoadPiece
	{
		public int type = 1;
		public Vector2 Position;
		public Direction flipFrom = Direction.UP;

		private GameObject displayedObject;
		private int? rotation;



		public GameObject getPrefab(){
			if (displayedObject == null || !displayedObject.activeInHierarchy) {
				displayedObject = Object.Instantiate (RoadPieceHelper.getRoadPrefab (type));
			}
			return displayedObject;
		}
		public void deletePrefab(){
			if (displayedObject != null && displayedObject.activeInHierarchy) {
				Object.Destroy (displayedObject);
			}
		}
		public int getRotation(){
			if (rotation == null) {
				rotation = RoadPieceHelper.getRotation (type);
			}
			return (int)rotation;
		}
	}


}