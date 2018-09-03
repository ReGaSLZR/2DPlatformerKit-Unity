using UnityEngine;
using Zenject;

public class ModelsInstaller : MonoInstaller<ModelsInstaller>, 
								Instantiator
{
	[SerializeField] private CameraShaker cameraShaker;

	[Space]
	[SerializeField] private PlayerGroundController playerGround;
	[SerializeField] private PlayerStats playerStats;
	[SerializeField] private PlayerInputControls playerInputControls;

	[Space]
	[SerializeField] private VolumeController volumeController;
	[SerializeField] private TimerController timerController;
	[SerializeField] private CheckpointController checkpointController;

	[Space]
	[SerializeField] private DialogueController dialogueController;

	/* ---------------------------------------------------------------------------------------- */

	public void InjectPrefab(GameObject gameObject) {
		Container.InjectGameObject(gameObject);
	}

	public void InjectPrefab(GameObject prefab, GameObject parent) {
		if(prefab == null) {
			return;
		}

		InjectPrefab(Instantiate(prefab, parent.transform.position, parent.transform.rotation));
	}

	/* ---------------------------------------------------------------------------------------- */

    public override void InstallBindings()
    {
		Container.Bind<Instantiator>().FromInstance(this);

		SetCameraInjections();
		SetDialogueInjections();
		SetPlayerInjections();

		SetVolumeInjections();
		SetTimerInjections();
		SetCheckpointInjections();
    }

	private void SetCameraInjections() {
		if(cameraShaker != null) {
			Container.Bind<CameraShake_Setter>().FromInstance(cameraShaker);	
		}
	}

	private void SetDialogueInjections() {
		if(dialogueController != null) {
			Container.Bind<DialogueController_Observer>().FromInstance(dialogueController);	
			Container.Bind<DialogueController_Setter>().FromInstance(dialogueController);	
		}
	}

	private void SetPlayerInjections() {
		/*  GROUND  */
		if(playerGround != null) {
			Container.Bind<PlayerGround_Observer>().FromInstance(playerGround);
			Container.Bind<PlayerGround_Setter>().FromInstance(playerGround);
		}

		/*  STATS OBSERVER  */
		if(playerStats != null) {
			Container.Bind<PlayerStats_Observer>().FromInstance(playerStats);
		}

		/*  STATS SETTERS  */
		if(playerStats != null) {
			Container.Bind<PlayerStatSetter_Health>().FromInstance(playerStats);
			Container.Bind<PlayerStatSetter_Lives>().FromInstance(playerStats);
			Container.Bind<PlayerStatSetter_Shots>().FromInstance(playerStats);
			Container.Bind<PlayerStatSetter_Money>().FromInstance(playerStats);
			Container.Bind<PlayerStatSetter_Scrolls>().FromInstance(playerStats);
		}

		/*  INPUT CONTROLS  */
		if(playerInputControls != null) {
			Container.Bind<PlayerInputControls>().FromInstance(playerInputControls);
			Container.Bind<InputControlDisabler>().FromInstance(playerInputControls);
		}
	}

	private void SetVolumeInjections() {
		if(volumeController != null) {
			Container.Bind<VolumeController_Observer>().FromInstance(volumeController);
			Container.Bind<VolumeController_Setter>().FromInstance(volumeController);
		}
	}

	private void SetTimerInjections() {
		if(timerController != null) {
			Container.Bind<Timer_Observer>().FromInstance(timerController);
			Container.Bind<Timer_Setter>().FromInstance(timerController);
		}
	}

	private void SetCheckpointInjections() {
		if(checkpointController != null) {
			Container.Bind<Checkpoint_Getter>().FromInstance(checkpointController);
			Container.Bind<Checkpoint_Setter>().FromInstance(checkpointController);
		}
	}

}

public interface Instantiator {
	void InjectPrefab(GameObject gameObject);
	void InjectPrefab(GameObject prefab, GameObject parent);
}