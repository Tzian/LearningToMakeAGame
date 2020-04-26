using UnityEditor;


namespace Cheky.Editor
{
	public static class BuildScript
	{
#if CHEKY_CLIENT
		[MenuItem("Cheky Tools/Build/Win64 GameClient")]
		static void BuildWin64Client()
		{
			string[] clientScenes = 
			{
				"Assets/Cheky/Scenes/ClientStartScene.unity",
				"Assets/Cheky/Scenes/ClientGameScene.unity"
			};

			BuildPlayerOptions buildOptions = new BuildPlayerOptions
			{
				locationPathName = "E:/_GameDev_LearningToMakeAGame/Cheky/OurGameName/ChekyGame.exe",
				scenes = clientScenes,
				target = BuildTarget.StandaloneWindows64,
				options = BuildOptions.AllowDebugging | BuildOptions.Development
			};

			BuildPipeline.BuildPlayer(buildOptions);
		}
#endif

#if CHEKY_SERVER
		[MenuItem (itemName : "Cheky Tools/Build/Win64 GameServer")]
		static void BuildWin64Server ()
		{
			string [] serverScenes =
			{
				"Assets/Cheky/Scenes/ServerStartScene.unity",
				"Assets/Cheky/Scenes/ServerGameScene.unity"
			};
			BuildPlayerOptions buildOptions = new BuildPlayerOptions
			{
				locationPathName = "E:/_GameDev_LearningToMakeAGame/Cheky/OurGameName/Server/ChekyServer.exe",
				scenes = serverScenes,
				target = BuildTarget.StandaloneWindows64,
				options = BuildOptions.AllowDebugging | BuildOptions.Development
			};
			BuildPipeline.BuildPlayer (buildOptions);
		}
#endif
	}
}
