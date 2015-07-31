/**
	CreateAnimatorParameterSettings

	Copyright (c) 2015 Tatsuhiko Yamamura
    This software is released under the MIT License.
    http://opensource.org/licenses/mit-license.php
*/
#region using
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.IO;
using UnityEngine.Assertions;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;


#endregion

public class CreateAnimatorParameterSettings : AssetPostprocessor
{
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) 
	{
		foreach (var str in importedAssets)
		{
			if( Path.GetExtension(str) != ".controller"){	continue;}
			var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(str);
			var code = GenerateCode(controller);
			var fileName = GetFileName(str);
			File.WriteAllText(fileName, code);
		}
		foreach (var str in deletedAssets) 
		{
			if( Path.GetExtension(str) != ".controller"){	continue;}
			var path = GetPath( GetFileName(str));
			AssetDatabase.DeleteAsset(path);
		}
		
		for (var i=0; i<movedAssets.Length; i++)
		{
			var fromPath = movedFromAssetPaths[i];
			var toPath = movedAssets[i];
			if( Path.GetExtension(fromPath) != ".controller"){	continue;}
			var path = GetPath( GetFileName(fromPath));
			AssetDatabase.MoveAsset(path, GetFileName(toPath));
		}

		AssetDatabase.Refresh ();
	}

	static string GetFileName(string path){

		var directry = Directory.GetParent (path);
		var fileName = Path.GetFileNameWithoutExtension (path);

		return string.Format ("{0}/{1}.cs", directry, StripSpace(fileName));
	}

	static string GetPath(string path){
		if (File.Exists (path)) {
			return path;
		}

		var fileName = Path.GetFileName (path);
		var files = Directory.GetFiles ("Assets", fileName, SearchOption.AllDirectories);
		if (files.Length == 0) {
			return null;
		}
		return files [0];
	}

	static string StripSpace(string name){
		return Regex.Replace(name, "\\W", "_");
	}

	static string GenerateCode(AnimatorController animatorController)
	{
		string intent = "		";
		string floatPropertyTemplate = intent + "protected readonly static int {0}Hash = {1}; public float {0}{{ get{{ return animator.GetFloat({0}Hash); }} set{{ animator.SetFloat({0}Hash, value); }}}}";
		string intPropertyTemplate = intent + "protected readonly static int {0}Hash = {1}; public int {0}{{ get{{ return animator.GetInteger({0}Hash); }} set{{ animator.SetInteger({0}Hash, value); }}}}";
		string boolPropertyTemplate = intent + "protected readonly static int {0}Hash = {1}; public bool {0}{{ get{{ return animator.GetBool({0}Hash); }} set{{ animator.SetBool({0}Hash, value); }}}}";
		string triggerTemplate = intent + "protected readonly static int {0}Hash = {1}; public void {0}(){{ animator.SetTrigger ({0}Hash); }} public void Reset{0}() {{ animator.ResetTrigger ({0}Hash); }}";

		string stateTemplate = intent + "public static readonly int {0} = {1};";

		var codePath = GetPath ("Assets/AnimatorAccessHelper/Editor/Resources/AnimatorParameterImporter.txt");
		var codeTemplate = File.ReadAllText (codePath);
		Assert.IsNotNull (codeTemplate);

		StringBuilder fields = new StringBuilder ();
		foreach (var param in animatorController.parameters) {
			string code = string.Empty;
			string name = StripSpace(param.name);
			if( param.type == AnimatorControllerParameterType.Bool)
				code = string.Format(boolPropertyTemplate, name, param.nameHash);
			if( param.type == AnimatorControllerParameterType.Float)
				code = string.Format(floatPropertyTemplate, name, param.nameHash);
			if( param.type == AnimatorControllerParameterType.Int)
				code = string.Format(intPropertyTemplate, name, param.nameHash);
			if( param.type == AnimatorControllerParameterType.Trigger)
				code = string.Format(triggerTemplate, name, param.nameHash);
			fields.AppendLine(code);
		}

		Dictionary<string, int> hashState = new Dictionary<string, int>();

		foreach (var layer in animatorController.layers) {
			StateCheck(layer.stateMachine, layer.stateMachine.name + ".", ref hashState);
		}
		foreach (var states in hashState) {
			var code = string.Format(stateTemplate, states.Key, states.Value);
			fields.AppendLine(code);
		}

		return string.Format(codeTemplate, StripSpace(animatorController.name), fields.ToString());
	}

	static void StateCheck(AnimatorStateMachine statemachiene, string stateNameSpace, ref Dictionary<string, int> hashState)
	{
		foreach (var child in statemachiene.stateMachines) {
			StateCheck(child.stateMachine, stateNameSpace + child.stateMachine.name + ".", ref hashState);
		}
		foreach (var state in statemachiene.states) {
			var name = stateNameSpace + state.state.name ;
			var hash = Animator.StringToHash(name);
			var stripedName = StripSpace(name);
			if( hashState.ContainsKey(stripedName) == false ){
				hashState.Add(stripedName, hash);
			}
		}
	}

}
