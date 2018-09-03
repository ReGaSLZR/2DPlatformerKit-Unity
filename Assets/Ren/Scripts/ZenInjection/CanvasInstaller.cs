using UnityEngine;
using Zenject;

public class CanvasInstaller : MonoInstaller<CanvasInstaller>
{
	[SerializeField] Canvas canvasHolder;

	private void Awake() {
		if(canvasHolder == null) {
			LogUtil.PrintWarning(gameObject, GetType(), "No canvasHolder defined. Destroying...");
			Destroy(this);
		}
	}

    public override void InstallBindings()
    {
		Container.Bind<Canvas>().FromInstance(canvasHolder);	

    }
}