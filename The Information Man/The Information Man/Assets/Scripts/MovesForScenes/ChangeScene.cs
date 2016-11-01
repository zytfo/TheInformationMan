using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {
	public string stageName;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "player")
		{
			Application.LoadLevel (this.stageName);
		}
	}
}
