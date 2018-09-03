using UnityEngine;

public enum DIRECTION {
	LEFT, RIGHT, UP, DOWN
}

public class DirectionUtil {

	public static Vector2 getVectorDirection(DIRECTION direction) {
		switch(direction) {
		case DIRECTION.UP : {
				return Vector2.up;
			}
		case DIRECTION.DOWN : {
				return Vector2.down;
			}
		case DIRECTION.LEFT : {
				return Vector2.left;
			}
		case DIRECTION.RIGHT : {
				return Vector2.right;
			}
		default: {
				return Vector2.up;	
			}
		}
	}

}