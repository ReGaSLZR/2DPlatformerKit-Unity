using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class PlayerSkillsAI : AIBehaviour	
{

	[SerializeField] private SkillBehaviour skillBasicAttack;
	[SerializeField] private int basicAttack_Cost = 1;
	[SerializeField] private float basicAttack_Interval = 0.5f;
	private System.DateTimeOffset _lastBasicAttack;
	[SerializeField] private bool noAttackOnSkillEffectivity;

	[Space]

	[SerializeField] private SkillBehaviour skillSpecial_1;
	[SerializeField] private int special_1_Cost = 20;
	[SerializeField] private float special_1_Interval = 0.5f;
	private System.DateTimeOffset _lastSpecial1;

	[Space]

	[SerializeField] private SkillBehaviour skillSpecial_2;
	[SerializeField] private int special_2_Cost = 25;
	[SerializeField] private float special_2_Interval = 0.5f;
	private System.DateTimeOffset _lastSpecial2;

	[Inject] PlayerStats_Observer playerStats;
	[Inject] PlayerStatSetter_Shots playerShots;
	[Inject] PlayerInputControls playerInput;

	private void Awake() {
		if((skillBasicAttack == null) || (skillSpecial_1 == null) || (skillSpecial_2 == null)) {
			LogUtil.PrintError(gameObject, GetType(), "Incomplete SkillBehaviour parameters supplied. Destroying...");
			Destroy(this);
		}
	}

	private void Start () {
		this.FixedUpdateAsObservable()
			.Select(_ => playerInput.hasAttacked)
			.Where(hasPressedKey => (hasPressedKey) && !CheckNoSkillsInEffect())
			.Timestamp()
			.Where(x => x.Timestamp > _lastBasicAttack.AddSeconds(basicAttack_Interval))
			.Subscribe(x => {
				if(playerShots.TakeShots(basicAttack_Cost)) {
					_lastBasicAttack = x.Timestamp;
					skillBasicAttack.UseSkill(false);
				}
			})
			.AddTo(this);
		
		this.FixedUpdateAsObservable()
			.Select(_ => playerInput.hasActivated_Skill1)
			.Where(hasPressedKey => (hasPressedKey))
			.Timestamp()
			.Where(x => x.Timestamp > _lastSpecial1.AddSeconds(special_1_Interval))
			.Subscribe(x => {
				if(playerShots.TakeShots(special_1_Cost)) {
					_lastSpecial1 = x.Timestamp;
					skillSpecial_1.UseSkill(false); 
				}
			})
			.AddTo(this);
	
		this.FixedUpdateAsObservable()
			.Select(_ => playerInput.hasActivated_Skill2)
			.Where(hasPressedKey => (hasPressedKey))
			.Timestamp()
			.Where(x => x.Timestamp > _lastSpecial2.AddSeconds(special_2_Interval))
			.Subscribe(x => {
				if(playerShots.TakeShots(special_2_Cost)) {
					_lastSpecial2 = x.Timestamp;
					skillSpecial_2.UseSkill(false);
				}
			})
			.AddTo(this);
	
	}

	private bool CheckNoSkillsInEffect() {
		bool skillsInEffect = (noAttackOnSkillEffectivity && (skillSpecial_1.isInUse || skillSpecial_2.isInUse));

		if(skillsInEffect) {
			LogUtil.PrintInfo(gameObject, GetType(), "Cannot Attack. Skill 1/2 is in use.");
		}

		return skillsInEffect;
	}

}
