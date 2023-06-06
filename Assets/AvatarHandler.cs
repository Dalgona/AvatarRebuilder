﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/*
 * VRSuya AvatarRebuilder
 * Contact : vrsuya@gmail.com // Twitter : https://twitter.com/VRSuya
 * Forked from emilianavt/ReassignBoneWeigthsToNewMesh.cs ( https://gist.github.com/emilianavt/721cd4dd2d4a62ba54b002b63f894dbf )
 * Thanks to Dalgona. & C_Carrot & Naru & Rekorn
 */

namespace com.vrsuya.avatarrebuilder {

	[ExecuteInEditMode]
	[AddComponentMenu("")]
	public class AvatarHandler : AvatarRebuilder {

		/// <summary>
		/// 본 프로그램의 메인 세팅 로직입니다.
		/// </summary>
		internal static void RequestCheckNewAvatar() {
			if (!IsSameFBX()) {
				if(!IsExistNewAvatarInScene()) {
					ApplyNewAvatarFBXModel();
					PlaceGameObejctInScene();
				}
			}
			return;
		}

		/// <summary>기존 아바타를 복제하여 백업본을 생성합니다.</summary>
		internal static void CreateDuplicateAvatar() {
			Undo.RecordObject(OldAvatarGameObject, "Duplicated Old Avatar");
			GameObject DuplicatedAvatar = Instantiate(OldAvatarGameObject);
			Undo.RegisterCreatedObjectUndo(DuplicatedAvatar, "Duplicated Old Avatar");
			DuplicatedAvatar.name = OldAvatarGameObject.name;
			OldAvatarGameObject.name = DuplicatedAvatar.name + "(Backup)";
			OldAvatarGameObject.SetActive(false);
			Undo.CollapseUndoOperations(UndoGroupIndex);

			OldAvatarGameObject = DuplicatedAvatar;
			OldAvatarAnimator = OldAvatarGameObject.GetComponent<Animator>();
			AvatarRootBone = OldAvatarAnimator.GetBoneTransform(HumanBodyBones.Hips);
			return;
		}

		/// <summary>새 아바타 GameObject가 Scene에 존재하는지 여부를 알려줍니다.</summary>
		/// <returns>새 아바타 GameObject가 Scene에 존재하는지 여부</returns>
		private static bool IsSameFBX() {
			bool Result = false;
			UnityEngine.Avatar OldAnimatorAvatar = OldAvatarAnimator.avatar;
			UnityEngine.Avatar NewAnimatorAvatar = NewAvatarAnimator.avatar;
			string OldAvatarAssetPath = AssetDatabase.GetAssetPath(OldAnimatorAvatar);
			string NewAvatarAssetPath = AssetDatabase.GetAssetPath(NewAnimatorAvatar);
			if (OldAvatarAssetPath == NewAvatarAssetPath) Result = true;
			return Result;
		}

		/// <summary>새 아바타 GameObject가 Scene에 존재하는지 여부를 알려줍니다.</summary>
		/// <returns>새 아바타 GameObject가 Scene에 존재하는지 여부</returns>
		private static bool IsExistNewAvatarInScene() {
			return NewAvatarGameObject.scene.IsValid();
		}

		/// <summary>기존 아바타의 FBX 메타 데이터를 복제하여, 새 아바타의 FBX 메타 데이터에 적용 합니다.</summary>
		private static void ApplyNewAvatarFBXModel() {
			UnityEngine.Avatar OldAnimatorAvatar = OldAvatarAnimator.avatar;
			UnityEngine.Avatar NewAnimatorAvatar = NewAvatarAnimator.avatar;
			string OldAvatarAssetPath = AssetDatabase.GetAssetPath(OldAnimatorAvatar);
			string NewAvatarAssetPath = AssetDatabase.GetAssetPath(NewAnimatorAvatar);
			ModelImporter OldAvatarModelImporter = AssetImporter.GetAtPath(OldAvatarAssetPath) as ModelImporter;
			ModelImporter NewAvatarModelImporter = AssetImporter.GetAtPath(NewAvatarAssetPath) as ModelImporter;
			AssetDatabase.ImportAsset(NewAvatarAssetPath);
		}

		/// <summary>새 아바타 GameObject를 Scene에 배치를 합니다.</summary>
		private static void PlaceGameObejctInScene() {
			GameObject NewInstance = (GameObject)PrefabUtility.InstantiatePrefab(NewAvatarGameObject);
			Debug.Log("Done");
			Undo.RegisterCreatedObjectUndo(NewInstance, "Added New GameObject");
			Undo.CollapseUndoOperations(UndoGroupIndex);
			return;
		}
	}
}
#endif