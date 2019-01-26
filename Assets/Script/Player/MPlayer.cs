namespace AtStudio.GGJ2019
{
	using UnityEngine;


	// This is just a simple "player" script that rotates and colors a cube
	// based on input read from the actions field.
	//
	// See comments in PlayerManager.cs for more details.
	//
	public class MPlayer : MonoBehaviour
	{
		public PlayerActions Actions { get; set; }

		void OnDisable()
		{
			if (Actions != null)
			{
				Actions.Destroy();
			}
		}


		void Start()
		{
		}


		void Update()
		{
		}
        
	}
}

