using System.Collections;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

/*
	NOTE: This Skill works by the following steps. 
	[1] Defaults itself to be on Static Rigidbody type and renders itself floating in the Scene.
	[2] When the AI this Skill is associated with has received a ping from the TargetDetector, this Skill 
		switches itself to the Dynamic Rigidbody type hence, rendering itself falling from its original position.
	[3] When the gameObject collides against something, this skill spawns a prefabSub for FX 
		and then Destroys the gameObject hence, making it disappear from the Scene.

	EXTRA NOTES: 
	[1] Once this skill has been triggered, there is no way to "stop" it. (It doesn't implement anything under the
		StopSkill() inherited function. This is by design.)
*/

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FallSkill : SkillBehaviour
{
	
	[SerializeField] private float objectMass = 5f;
	[SerializeField] private float objectGravityScale = 5f;
	[SerializeField] private AudioClip clipFall;

	[Header("Option to Break on Collision")]
	[SerializeField] private GameObject prefabBreakFX;
	[SerializeField] private float delayBreakFX = 0.1f;

	[Inject] Instantiator instantiator;

	private AudioSource _audioSource;
	private Rigidbody2D _rigidbody2D;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		_rigidbody2D = GetComponent<Rigidbody2D>();

		_rigidbody2D.bodyType = RigidbodyType2D.Static;
	}

	private void Start()
	{
		if(prefabBreakFX != null) {
			this.OnCollisionEnter2DAsObservable()
				.Subscribe(otherCollision2D => StartCoroutine(CorBreakSelf()))
				.AddTo(this);
		}
	}

	private IEnumerator CorBreakSelf() {
		yield return new WaitForSeconds(delayBreakFX);

		if(prefabBreakFX != null) {
			instantiator.InjectPrefab(prefabBreakFX, this.gameObject);
		}
		Destroy(this.gameObject);
	}

	protected override void UseSkillOnce() {
		LogUtil.PrintInfo(gameObject, GetType(), "Skill has been triggered!");

		AudioUtil.PlaySingleClip(GetType(), clipFall, _audioSource);

		_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
		_rigidbody2D.mass = objectMass;
		_rigidbody2D.gravityScale = objectGravityScale;
	}

	protected override void UseSkillRepeat() {
		LogUtil.PrintWarning(gameObject, GetType(), "UserSkillRepeat() >> using UseSkillOnce() instead.");
		UseSkillOnce();
	}

	protected override void StopSkill() {
		LogUtil.PrintInfo(gameObject, GetType(), 
			"StopSkill() >> Sorry, you can't stop the fall nor the breakage of this object.");
		//This is by design. -Ren
	}

}

