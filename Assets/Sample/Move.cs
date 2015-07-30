using UnityEngine;
using System.Collections;
using AnimatorParameter;

[SelectionBase]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour {
	
	[SerializeField]
	CharacterTemplate characterAnimator = new CharacterTemplate();
	
	[SerializeField, DisappearAttachedField]
	CharacterController characterController;
	
	private Vector3 direction;
	
	void Reset()
	{
		var animator = GetComponent<Animator> ();
		characterAnimator.animator = animator;
		animator.applyRootMotion = false;
		
		characterController = GetComponent<CharacterController> ();
		characterController.center = Vector3.up * 0.75f;
		characterController.height = 1.5f;
		characterController.radius = 0.3f;
	}
	
	void Update () 
	{
		var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
		direction = cameraForward * Input.GetAxis ("Vertical") + 
			Camera.main.transform.right * Input.GetAxis ("Horizontal");

		characterAnimator.Speed = direction.sqrMagnitude;
	}
	
	void OnAnimatorMove()
	{
		characterController.SimpleMove (direction.normalized * 3);
		if (direction != Vector3.zero) {
			var velocity = direction;
			velocity.y = 0;
			transform.rotation = Quaternion.LookRotation(velocity);
		}
	}
}