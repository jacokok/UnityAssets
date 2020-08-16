using System;
using UnityEditor;
using UnityEngine;

namespace Kok.CustomEditors
{
	[CustomPropertyDrawer (typeof (SceneAsset))]
	public class SceneAssetEditor : PropertyDrawer
	{
		#region -- Constants --------------------------------------------------
		private const string TOOLTIP_SCENE_MISSING =
			"Scene is not in build settings.";

		private const string ERROR_SCENE_MISSING =
			"You are refencing a scene that is not added to the build. Add it to the editor build settings now?";

		private const string TOOLTIP_SCENE_DISABLED =
			"Scene is not enebled in build settings.";

		private const string ERROR_SCENE_DISABLED =
			"You are refencing a scene that is not active the build. Enable it in the build settings now?";
		#endregion -- Constants -----------------------------------------------

		#region -- Private Variables ------------------------------------------
		private SerializedProperty scene;
		private SerializedProperty sceneName;
		private SerializedProperty sceneIndex;
		private SerializedProperty sceneEnabled;
		private UnityEditor.SceneAsset sceneAsset;
		private string sceneAssetPath;
		private string sceneAssetGUID;

		private GUIContent errorTooltip;
		private GUIStyle errorStyle;
		#endregion -- Private Variables ---------------------------------------

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty (position, label, property);
			position = EditorGUI.PrefixLabel (position, label);

			CacheProperties (property);
			UpdateSceneState ();

			position = DisplayErrorsIfNecessary (position);

			EditorGUI.BeginChangeCheck ();
			EditorGUI.PropertyField (position, scene, GUIContent.none, false);
			if (EditorGUI.EndChangeCheck ())
			{
				property.serializedObject.ApplyModifiedProperties ();
				CacheProperties (property);
				UpdateSceneState ();
				Validate ();
			}

			EditorGUI.EndProperty ();
		}

		private void CacheProperties (SerializedProperty property)
		{
			scene = property.FindPropertyRelative ("Scene");
			sceneName = property.FindPropertyRelative ("SceneName");
			sceneIndex = property.FindPropertyRelative ("sceneIndex");
			sceneEnabled = property.FindPropertyRelative ("sceneEnabled");
			sceneAsset = scene.objectReferenceValue as UnityEditor.SceneAsset;

			if (sceneAsset != null)
			{
				sceneAssetPath = AssetDatabase.GetAssetPath (scene.objectReferenceValue);
				sceneAssetGUID = AssetDatabase.AssetPathToGUID (sceneAssetPath);
			}
			else
			{
				sceneAssetPath = null;
				sceneAssetGUID = null;
			}
		}

		private void UpdateSceneState ()
		{
			if (sceneAsset != null)
			{
				EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

				sceneIndex.intValue = -1;
				for (int i = 0; i < scenes.Length; i++)
				{
					if (scenes[i].guid.ToString () == sceneAssetGUID)
					{
						if (sceneIndex.intValue != i)
							sceneIndex.intValue = i;
						sceneEnabled.boolValue = scenes[i].enabled;
						if (scenes[i].enabled)
						{
							if (sceneName.stringValue != sceneAsset.name)
								sceneName.stringValue = sceneAsset.name;
						}
						break;
					}
				}
			}
			else
			{
				sceneName.stringValue = "";
			}
		}

		private void DisplaySceneErrorPrompt (string message)
		{
			EditorBuildSettingsScene[] scenes =
				EditorBuildSettings.scenes;

			int choice = EditorUtility.DisplayDialogComplex ("Scene Not In Build",
				message, "Yes", "No", "Open Build Settings");

			if (choice == 0)
			{
				int newCount = sceneIndex.intValue < 0 ?
					scenes.Length + 1 : scenes.Length;
				EditorBuildSettingsScene[] newScenes =
					new EditorBuildSettingsScene[newCount];
				Array.Copy (scenes, newScenes, scenes.Length);

				if (sceneIndex.intValue < 0)
				{
					newScenes[scenes.Length] = new EditorBuildSettingsScene (
						sceneAssetPath, true);
					sceneIndex.intValue = scenes.Length;
				}

				newScenes[sceneIndex.intValue].enabled = true;

				EditorBuildSettings.scenes = newScenes;
			}
			else if (choice == 2)
			{
				EditorApplication.ExecuteMenuItem ("File/Build Settings...");
			}
		}

		private Rect DisplayErrorsIfNecessary (Rect position)
		{
			if (errorStyle == null)
			{
				errorStyle = "CN EntryErrorIconSmall";
				errorTooltip = new GUIContent ("", "error");
			}

			if (sceneAsset == null)
				return position;

			Rect warningRect = new Rect (position);
			warningRect.width = errorStyle.fixedWidth + 4;

			if (sceneIndex.intValue < 0)
			{
				errorTooltip.tooltip = TOOLTIP_SCENE_MISSING;
				position.xMin = warningRect.xMax;
				if (GUI.Button (warningRect, errorTooltip, errorStyle))
				{
					DisplaySceneErrorPrompt (ERROR_SCENE_MISSING);
				}
			}
			else if (!sceneEnabled.boolValue)
			{
				errorTooltip.tooltip = TOOLTIP_SCENE_DISABLED;
				position.xMin = warningRect.xMax;
				if (GUI.Button (warningRect, errorTooltip, errorStyle))
				{
					DisplaySceneErrorPrompt (ERROR_SCENE_DISABLED);
				}
			}

			return position;
		}

		private void Validate ()
		{
			if (sceneAsset != null)
			{
				EditorBuildSettingsScene[] scenes =
					EditorBuildSettings.scenes;

				sceneIndex.intValue = -1;
				for (int i = 0; i < scenes.Length; i++)
				{
					if (scenes[i].guid.ToString () == sceneAssetGUID)
					{
						if (sceneIndex.intValue != i)
							sceneIndex.intValue = i;
						if (scenes[i].enabled)
						{
							if (sceneName.stringValue != sceneAsset.name)
								sceneName.stringValue = sceneAsset.name;
							return;
						}
						break;
					}
				}

				if (sceneIndex.intValue >= 0)
					DisplaySceneErrorPrompt (ERROR_SCENE_DISABLED);
				else
					DisplaySceneErrorPrompt (ERROR_SCENE_MISSING);
			}
			else
			{
				sceneName.stringValue = "";
			}
		}
	}
}