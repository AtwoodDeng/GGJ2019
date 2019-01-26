namespace AtStudio.GGJ2019
{
	using InControl;


	public class PlayerActions : PlayerActionSet
	{
		public PlayerOneAxisAction RotateShip;
        public PlayerAction RotateShipClock;
        public PlayerAction RotateShipAClock;
		public PlayerAction PushShip;
		public PlayerOneAxisAction RotateCatcher;
        public PlayerAction RotateCatcherClock;
        public PlayerAction RotateCatcherAClock;

        public PlayerAction PushCatcher;
		public PlayerAction Left;
		public PlayerAction Right;
		public PlayerAction Up;
		public PlayerAction Down;
		public PlayerTwoAxisAction Rotate;

        public PlayerAction joinGame;


		public PlayerActions()
        {
            RotateShipClock = CreatePlayerAction("RotateShipClock");
            RotateShipAClock = CreatePlayerAction("RotateShipAClock");
            RotateShip = CreateOneAxisPlayerAction( RotateShipClock, RotateShipAClock);
			PushShip = CreatePlayerAction( "PushShip" );
            RotateCatcherClock = CreatePlayerAction("RotateCatcherClock");
            RotateCatcherAClock = CreatePlayerAction("RotateCatcherAClock");
            RotateCatcher = CreateOneAxisPlayerAction(RotateCatcherClock, RotateCatcherAClock);
			PushCatcher = CreatePlayerAction( "YPushCatcher" );
			Left = CreatePlayerAction( "Left" );
			Right = CreatePlayerAction( "Right" );
			Up = CreatePlayerAction( "Up" );
			Down = CreatePlayerAction( "Down" );
			Rotate = CreateTwoAxisPlayerAction( Left, Right, Down, Up );
            joinGame = CreatePlayerAction("Join");
		}


		public static PlayerActions CreateWithKeyboardBindings()
		{
			var actions = new PlayerActions();

            actions.RotateShipClock.AddDefaultBinding(Key.A);
            actions.RotateShipAClock.AddDefaultBinding(Key.D);
            actions.PushShip.AddDefaultBinding( Key.S );
            actions.RotateCatcherClock.AddDefaultBinding( Key.Q);
            actions.RotateCatcherAClock.AddDefaultBinding(Key.E);
            actions.PushCatcher.AddDefaultBinding( Key.F );

			actions.Up.AddDefaultBinding( Key.UpArrow );
			actions.Down.AddDefaultBinding( Key.DownArrow );
			actions.Left.AddDefaultBinding( Key.LeftArrow );
			actions.Right.AddDefaultBinding( Key.RightArrow );

            actions.joinGame.AddDefaultBinding(Key.J);

			return actions;
		}


		public static PlayerActions CreateWithJoystickBindings()
		{
			var actions = new PlayerActions();

			actions.RotateShipClock.AddDefaultBinding( InputControlType.LeftTrigger );
            actions.RotateShipAClock.AddDefaultBinding(InputControlType.RightTrigger);
            actions.PushShip.AddDefaultBinding( InputControlType.Action2 );
			actions.RotateCatcherClock.AddDefaultBinding( InputControlType.LeftBumper );
            actions.RotateCatcherAClock.AddDefaultBinding(InputControlType.RightBumper);
            actions.PushCatcher.AddDefaultBinding( InputControlType.Action4 );

			actions.Up.AddDefaultBinding( InputControlType.LeftStickUp );
			actions.Down.AddDefaultBinding( InputControlType.LeftStickDown );
			actions.Left.AddDefaultBinding( InputControlType.LeftStickLeft );
			actions.Right.AddDefaultBinding( InputControlType.LeftStickRight );

			actions.Up.AddDefaultBinding( InputControlType.DPadUp );
			actions.Down.AddDefaultBinding( InputControlType.DPadDown );
			actions.Left.AddDefaultBinding( InputControlType.DPadLeft );
			actions.Right.AddDefaultBinding( InputControlType.DPadRight );

            actions.joinGame.AddDefaultBinding(InputControlType.Action1);

			return actions;
		}
	}
}

