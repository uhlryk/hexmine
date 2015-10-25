using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.GameCenter;
public class GameCenter{
	/**
	 * inicjujemy gameCenter
	 */ 
	public static void Authenticate(){
		#if UNITY_EDITOR
		return;
		#endif
		#if UNITY_IOS
		Social.localUser.Authenticate(GameCenter.CallbackCheckAuthIOS);
		#endif
	}
	private static void CallbackCheckAuthIOS(bool success){
		if (success) {
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
			Debug.Log("Autoryzacja Success "+Social.localUser.userName);
		}else{
			Debug.Log("Autoryzacja failed "+Social.localUser.userName);
		}
	}
	/**
	 * rejestrujemy achievement
	 */ 
	public static void AddAchievement(string acheivementId,double progress){
		#if UNITY_EDITOR
		return;
		#endif
		#if UNITY_IOS
		if(progress>100)progress=100;
		if(Social.localUser.authenticated){
			Social.ReportProgress(acheivementId,progress,GameCenter.CallbackCheckAchievement);
		}
		#endif
	}
	private static void CallbackCheckAchievement(bool success){
		if (success) {
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
			Debug.Log("Achievement Success "+Social.localUser.userName);
		}else{
			Debug.Log("Achievement failed "+Social.localUser.userName);
		}
	}
	/**
	 * rejestrujemy zdobyte punkty 
	 */
	public static void AddLeaderboard(string leaderboardId,long score){
		#if UNITY_EDITOR
		return;
		#endif
		#if UNITY_IOS
		if(Social.localUser.authenticated){
			Social.ReportScore(score,leaderboardId,GameCenter.CallbackCheckScore);
		}
		#endif
	}
	private static void CallbackCheckScore(bool success){
		if (success) {
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
			Debug.Log("Score Success "+Social.localUser.userName);
		}else{
			Debug.Log("Score failed "+Social.localUser.userName);
		}
	}
	public static class Leaderboard{
		public static string EASY="pl.artwave.hexmines.easylevel";
		public static string NORMAL="pl.artwave.hexmines.normallevel";
		public static string HARD="pl.artwave.hexmines.hardlevel";
		public static string EXTRALARGE="pl.artwave.hexmines.extralargelevel";
		public static string VERYHARD="pl.artwave.hexmines.veryhardlevel";
	}
}
