using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GestureRecognizer {

	/// <summary>
	/// Classes to store gesture lines.
	/// </summary>

	[System.Serializable]
	public class GestureLine {
		public List<Vector2> points = new List<Vector2> ();
		public bool closedLine;
	}

	[System.Serializable]
	public class GestureData {
		public List<GestureLine> lines = new List<GestureLine>();
		public GestureLine LastLine { get { return lines [lines.Count - 1]; } }
	}

}