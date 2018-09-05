public class TagUtil {

	public static bool IsUntagged(string tag) {
		return tag.Equals(OBJECT_TAG.Untagged.ToString());
	}

	public static bool IsTagPlayer(string tag, bool allowCamouflageCheck) {
		bool temp = tag.Equals(OBJECT_TAG.Player.ToString());

		if(!temp && allowCamouflageCheck) {
			temp = tag.Equals(OBJECT_TAG.Player_Camouflaged.ToString());
		}

		return temp;
	}

	public static bool IsTagItemMultiValue(string tag) {
		return (tag.Equals(OBJECT_TAG.Item_Money.ToString())) || 
			(tag.Equals(OBJECT_TAG.Item_Shots.ToString()) ||
				(tag.Equals(OBJECT_TAG.Item_Clock.ToString())));
	}

	public static bool IsTagWalkable(string tag) {
		return (tag.Equals(OBJECT_TAG.Floor.ToString()) || tag.Equals(OBJECT_TAG.Item_Box.ToString()) ||
			tag.Equals(OBJECT_TAG.Enemy.ToString()));
	}

	public static bool IsTagSlideable(string tag) {
		return (tag.Equals(OBJECT_TAG.Floor.ToString()) || tag.Equals(OBJECT_TAG.Item_Box.ToString()));
	}

}

/* NOTE: the values of this enum has to be present in the EditorGUI tags and vice versa. */
public enum OBJECT_TAG {

	Floor, Hidden_Passage_Blocker,

	Player, Player_Camouflaged,

	Ally,

	Enemy, Enemy_Sleeper,

	Item,
	Item_Health, Item_Life, Item_Money,
	Item_Scroll, Item_Shots, Item_Clock,
	Item_Box,

	Untagged

}
