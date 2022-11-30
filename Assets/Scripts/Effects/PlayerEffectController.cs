using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
	[SerializeField] Transform effRoot;

	struct EffectPlay
	{
		private bool _inPlay;
		private ParticleSystem _particleSystem;

		public EffectPlay(ParticleSystem particleSystem) : this()
		{
			this._particleSystem = particleSystem;
			_inPlay = false;
		}

		public void Play()
		{
			if (!_inPlay)
			{
				_inPlay = true;
				_particleSystem.Play();
			}
		}
		public void Stop()
		{
			if (_inPlay)
			{
				_inPlay = false;
				_particleSystem.Stop();
			}
		}
	}


	Dictionary<string, EffectPlay> _particleDict = new Dictionary<string, EffectPlay>();

	private void Awake()
	{
		foreach (Transform transf in effRoot)
		{
			_particleDict.Add(transf.name, new EffectPlay(transf.GetComponent<ParticleSystem>()));
		}
	}

	public void EnableParticles(string name)
	{
		if (_particleDict.ContainsKey(name))
		{
			_particleDict[name].Play();
		}
	}
	public void EnableParticles(string name, float delay)
	{
		if (_particleDict.ContainsKey(name))
		{
			StartCoroutine(EnableParticleDelay(_particleDict[name], delay));
		}
	}
	IEnumerator EnableParticleDelay(EffectPlay effect, float delay)
	{
		yield return new WaitForSeconds(delay);
		effect.Play();
	}
	public void DisableParticles(string name)
	{
		if (_particleDict.ContainsKey(name))
		{
			_particleDict[name].Stop();
		}
	}
}
