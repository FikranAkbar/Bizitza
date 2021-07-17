using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GestureRecognizer {

	/// <summary>
	/// Script do store "gesture".
	/// </summary>
	[CreateAssetMenu(menuName = "GestureRecognizer/GesturePattern")]
	public class GesturePattern : ScriptableObject {
	
		public string id;

		[System.Obsolete("'points' is deprecated, please use 'gesture' instead.")]
		[HideInInspector]
		[SerializeField]
		public List<Vector2> points;
	
		public GestureData gesture;

		public bool useLinesOrder;
		public bool useLinesDirections;

	}

}