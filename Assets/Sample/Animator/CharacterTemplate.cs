using UnityEngine;

namespace AnimatorParameter
{
	[System.Serializable]
	public class CharacterTemplate
	{
		public Animator animator;

		public readonly static int SpeedHash = -823668238; public float Speed{ get{ return animator.GetFloat(SpeedHash); } set{ animator.SetFloat(SpeedHash, value); }}
		public static readonly int Base_Layer_Ground = -1553979363;

	}
}