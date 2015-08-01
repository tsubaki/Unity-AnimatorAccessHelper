using UnityEngine;
using System.Collections;
using AnimatorParameter;

[RequireComponent(typeof(Animator))]
public class Sample2DebugCode : MonoBehaviour {

	[SerializeField]
	SampleController controller;

	void Reset()
	{
		controller = new SampleController ();
		controller.animator = GetComponent<Animator> ();
		controller.FloatParameter = 32;
		var result =  controller.FloatParameter;
	}

	void OnGUI()
	{
		using (var horizonal = new GUILayout.HorizontalScope()) {
			if( GUILayout.Button("+")){ 	controller.IntParameter += 1;	}
			if( GUILayout.Button("-")){ 	controller.IntParameter -= 1;	}
		}
		using (var horizonal = new GUILayout.HorizontalScope()) {
			if( GUILayout.Button("+")){ 	controller.FloatParameter += 0.1f;	}
			if( GUILayout.Button("-")){ 	controller.FloatParameter -= 0.1f;	}
		}
		controller.BoolParameter = GUILayout.Toggle( controller.BoolParameter, controller.BoolParameter.ToString());
		if( GUILayout.Button("trigger")){	controller.Trigger();	}
		using (var horizonal = new GUILayout.HorizontalScope()) {
			if(GUILayout.Button("Move State1") ){	controller.animator.Play( SampleController.State1_State1 );	}
			if(GUILayout.Button("Move State2") ){	controller.animator.Play( SampleController.State1_State2 );	}
		}
	}
}
