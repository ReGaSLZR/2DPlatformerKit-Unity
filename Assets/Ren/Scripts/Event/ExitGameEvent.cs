using UnityEngine;

public class ExitGameEvent : InGameEvent {

	protected override bool FireNow() {
		LogUtil.PrintInfo(gameObject, GetType(), "Exiting game. Thank you for playing... :)");
		Application.Quit();
		return true;
	}

	public override void CancelNow() {
		LogUtil.PrintInfo(gameObject, GetType(), "Sorry, you cannot cancel an ExitGame event's operation.");	
	}

}
