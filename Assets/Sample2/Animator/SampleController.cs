using UnityEngine;

namespace AnimatorParameter
{
	[System.Serializable]
	public class SampleController
	{
		public Animator animator;

		protected readonly static int IntParameterHash = 1701893327; public int IntParameter{ get{ return animator.GetInteger(IntParameterHash); } set{ animator.SetInteger(IntParameterHash, value); }}
		protected readonly static int FloatParameterHash = -2032648613; public float FloatParameter{ get{ return animator.GetFloat(FloatParameterHash); } set{ animator.SetFloat(FloatParameterHash, value); }}
		protected readonly static int BoolParameterHash = -822407162; public bool BoolParameter{ get{ return animator.GetBool(BoolParameterHash); } set{ animator.SetBool(BoolParameterHash, value); }}
		protected readonly static int TriggerHash = -707381567; public void Trigger(){ animator.SetTrigger (TriggerHash); } public void ResetTrigger() { animator.ResetTrigger (TriggerHash); }
		public static readonly int State1_State2 = -1259946821;
		public static readonly int State1_State1 = 770698497;

	}
}