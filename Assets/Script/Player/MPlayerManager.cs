﻿namespace AtStudio.GGJ2019
{
	using System.Collections.Generic;
	using InControl;
	using UnityEngine;


	// This example iterates on the basic multiplayer example by using action sets with
	// bindings to support both joystick and keyboard players. It would be a good idea
	// to understand the basic multiplayer example first before looking a this one.
	//
	public class MPlayerManager : MonoBehaviour
	{

        public static MPlayerManager _instance;
        public static MPlayerManager Instance
        {
            get
            {
                if ( _instance == null )
                {
                    _instance = GameObject.FindObjectOfType<MPlayerManager>();

                }
                return _instance;
            }
        }

        public bool IsMutiPlayer;

        public bool PlayersReady
        {
            get
            {
                if ( IsMutiPlayer )
                    return players.Count >= 2;

                return players.Count >= 1;
            }
        }

        public MPlayer playerOne
        {
            get
            {
                if (PlayersReady)
                    return players[0];
                return null;
            }
        }

        public MPlayer playerTwo
        {
            get
            {
                if (PlayersReady)
                {
                    if (IsMutiPlayer)
                        return players[1];
                    else
                        return players[0];
                }
                return null;
            }
        }

        public GameObject playerPrefab;

		const int maxPlayers = 4;

		List<Vector3> playerPositions = new List<Vector3>() {
			new Vector3( -1, 1, -10 ),
			new Vector3( 1, 1, -10 ),
			new Vector3( -1, -1, -10 ),
			new Vector3( 1, -1, -10 ),
		};

		List<MPlayer> players = new List<MPlayer>( maxPlayers );

		PlayerActions keyboardListener;
		PlayerActions joystickListener;


		void OnEnable()
		{
			InputManager.OnDeviceDetached += OnDeviceDetached;
			keyboardListener = PlayerActions.CreateWithKeyboardBindings();
			joystickListener = PlayerActions.CreateWithJoystickBindings();
		}


		void OnDisable()
		{
			InputManager.OnDeviceDetached -= OnDeviceDetached;
			joystickListener.Destroy();
			keyboardListener.Destroy();
		}


		void Update()
		{
			if (JoinButtonWasPressedOnListener( joystickListener ))
			{
				var inputDevice = InputManager.ActiveDevice;

				if (ThereIsNoPlayerUsingJoystick( inputDevice ))
				{
					CreatePlayer( inputDevice );
				}
			}

			if (JoinButtonWasPressedOnListener( keyboardListener ))
			{
				if (ThereIsNoPlayerUsingKeyboard())
				{
					CreatePlayer( null );
				}
			}
		}


		bool JoinButtonWasPressedOnListener( PlayerActions actions )
		{
            return actions.joinGame;
		}


		MPlayer FindPlayerUsingJoystick( InputDevice inputDevice )
		{
			var playerCount = players.Count;
			for (var i = 0; i < playerCount; i++)
			{
				var player = players[i];
				if (player.Actions.Device == inputDevice)
				{
					return player;
				}
			}

			return null;
		}


		bool ThereIsNoPlayerUsingJoystick( InputDevice inputDevice )
		{
			return FindPlayerUsingJoystick( inputDevice ) == null;
		}


		MPlayer FindPlayerUsingKeyboard()
		{
			var playerCount = players.Count;
			for (var i = 0; i < playerCount; i++)
			{
				var player = players[i];
				if (player.Actions == keyboardListener)
				{
					return player;
				}
			}

			return null;
		}


		bool ThereIsNoPlayerUsingKeyboard()
		{
			return FindPlayerUsingKeyboard() == null;
		}


		void OnDeviceDetached( InputDevice inputDevice )
		{
			var player = FindPlayerUsingJoystick( inputDevice );
			if (player != null)
			{
				RemovePlayer( player );
			}
		}


		MPlayer CreatePlayer( InputDevice inputDevice )
		{
			if (players.Count < maxPlayers)
			{
				// Pop a position off the list. We'll add it back if the player is removed.
				var playerPosition = playerPositions[0];
				playerPositions.RemoveAt( 0 );

				var gameObject = (GameObject) Instantiate( playerPrefab, playerPosition, Quaternion.identity );
				var player = gameObject.GetComponent<MPlayer>();

				if (inputDevice == null)
				{
					// We could create a new instance, but might as well reuse the one we have
					// and it lets us easily find the keyboard player.
					player.Actions = keyboardListener;
				}
				else
				{
					// Create a new instance and specifically set it to listen to the
					// given input device (joystick).
					var actions = PlayerActions.CreateWithJoystickBindings();
					actions.Device = inputDevice;

					player.Actions = actions;
				}

				players.Add( player );

				return player;
			}

			return null;
		}


		void RemovePlayer( MPlayer player )
		{
			playerPositions.Insert( 0, player.transform.position );
			players.Remove( player );
			player.Actions = null;
			Destroy( player.gameObject );
		}


		void OnGUI()
		{
			const float h = 22.0f;
			var y = 10.0f;

			GUI.Label( new Rect( 10, y, 300, y + h ), "Active players: " + players.Count + "/" + maxPlayers );
			y += h;

			if (players.Count < maxPlayers)
			{
				GUI.Label( new Rect( 10, y, 300, y + h ), "Press a button or a/s/d/f key to join!" );
				y += h;
			}
		}
	}
}