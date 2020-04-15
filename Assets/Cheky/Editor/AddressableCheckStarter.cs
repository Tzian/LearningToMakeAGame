using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;


namespace Cheky.Editor
{
	public class AddressableCheckStarter
	{
#if UNITY_EDITOR
		static AddressableAssetSettings settings;
		static AddressableAssetProfileSettings profileSettings;

		[MenuItem ("Cheky Tools/Update Addressables", priority = 902)]
		static void UpdateAddressables ()
		{
			settings = GetAddressableSettings ();
			CheckVisualAssets();	
		}


		static AddressableAssetSettings GetAddressableSettings ()
		{
			string [] settingsPath = new []
			{
				"Assets/AddressableAssetsData"
			};
			
			string settingsName = "AddressableAssetSettings";

			AddressableAssetSettings setting;
			string [] settingsGuids = AssetDatabase.FindAssets (settingsName, settingsPath);
			if (settingsGuids.Length == 0)
			{
				Debug.Log ("Cannot find addressable asset settings");
				return null;
			}
			setting = (AddressableAssetSettings) AssetDatabase.LoadAssetAtPath (settingsPath [0] + "/" + settingsName + ".asset", typeof (AddressableAssetSettings));
			if (Directory.Exists ("E:/__MyPackages/ServerData"))
				try
				{
					Directory.Delete ("E:/__MyPackages/ServerData", true);
				}
				catch (Exception e)
				{
					Debug.LogException(e);
				}
			return setting;
		}
		
		static void CheckVisualAssets ()
		{
			string [] visualsPath = AddressablesPackagePaths.GetAddressablesPackagePath ();
			DirectoryInfo info = new DirectoryInfo(visualsPath [0]);
			DirectoryInfo [] directories = info.GetDirectories ();
			
			foreach (DirectoryInfo directory in directories)
			{
				string directoryName = directory.Name;
				
				DirectoryInfo [] subDirectories = directory.GetDirectories ();

				string path = visualsPath [0] + "/" + directory.Name;
				
				foreach (DirectoryInfo subDirectory in subDirectories)
				{
					string addressableGroupName = subDirectory.Name;
					string subPath = path + "/" + addressableGroupName;
					
					AddressableAssetGroup group = FindOrCreateAddressableGroup (addressableGroupName);

					if (group == null)
					{
						Debug.Log ("Group is null!");
						continue;
					}

					FileInfo [] filesInFolder = subDirectory.GetFiles ("*", SearchOption.AllDirectories);

					foreach (FileInfo fileInfo in filesInFolder)
					{
						if (fileInfo.Name.Contains (".meta"))
						{
							continue;
						}
						AddressableAssetEntry entry = CreateOrUpdateAddressableAssetEntry (group, subPath, fileInfo.Name);
					}
				}
			}
			AddressablesDataBuilderInput buildContext = new AddressablesDataBuilderInput (settings, "1");
			settings.ActivePlayerDataBuilder.BuildData <AddressablesPlayerBuildResult> (buildContext);
		}

		static AddressableAssetEntry CreateOrUpdateAddressableAssetEntry (AddressableAssetGroup group, string subPath, string fileName)
		{
			string filePath = subPath + "/" + fileName;

			string guid = AssetDatabase.AssetPathToGUID (filePath);
			AddressableAssetEntry entry = settings.CreateOrMoveEntry (guid, group);
			if (entry != null)
			{
				entry.address = fileName;
				entry.SetLabel (group.Name, true, true);
			}
			return entry;
		}
		
		static AddressableAssetGroup FindOrCreateAddressableGroup (string addressableGroupName)
		{
			string [] addressableGroupsPath = new []
			{
				"Assets/AddressableAssetsData/AssetGroups"
			};
	
			string [] groupGuids = AssetDatabase.FindAssets (addressableGroupName, addressableGroupsPath);
			if (groupGuids.Length == 0)
			{
				return CreateAssetGroup <BundledAssetGroupSchema> (settings, addressableGroupName);
			}
			return (AddressableAssetGroup) AssetDatabase.LoadAssetAtPath (addressableGroupsPath [0] + "/" + addressableGroupName + ".asset", typeof (AddressableAssetGroup));
		}

		static AddressableAssetGroup CreateAssetGroup <SchemaType> (AddressableAssetSettings setting, string groupName)
		{
			AddressableAssetGroup assetGroup;
			
			assetGroup = setting.CreateGroup (groupName, false, false, false, new List <AddressableAssetGroupSchema>
			{
				setting.DefaultGroup.Schemas [0],
				setting.DefaultGroup.Schemas [1]
			}, typeof (SchemaType));
			return assetGroup;
		}
#endif
	}
}
