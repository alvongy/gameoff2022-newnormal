using UnityEngine;

public class AnimeEffect : StateMachineBehaviour
{

	[SerializeField] string effectName = default;
	[SerializeField] bool endWithAnime = default;
	[SerializeField] [Range(0f, 1f)] float delayDuration = default;

	PlayerEffectController playerEffectController;

	public PlayerEffectController PlayerEffectController { get => playerEffectController; set => playerEffectController = value; }

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (delayDuration > 0f)
		{
			playerEffectController.EnableParticles(effectName, delayDuration * stateInfo.length);
		}
		else
		{
			playerEffectController.EnableParticles(effectName);
		}
	}
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (endWithAnime)
		{
			playerEffectController.DisableParticles(effectName);
		}
	}
}
