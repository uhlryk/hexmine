using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMob {
	
	private BannerView bannerView;
	private InterstitialAd interstitial;

	#if UNITY_EDITOR
	private string adUnitId = "unused";
	#elif UNITY_ANDROID
	private string adUnitId = "ca-app-pub-7054698623904367/5422586239";
	#elif UNITY_IOS
	private string adUnitId = "ca-app-pub-7054698623904367/2469119839";
	#else
	private string adUnitId = "unexpected_platform";
	#endif

	public void StartInterstitial(){
		#if SIMULATOR
		return;
		#endif
		#if UNITY_ANDROID || UNITY_IPHONE
		interstitial = new InterstitialAd(adUnitId);
		AdRequest request = new AdRequest.Builder().Build();
		interstitial.LoadAd(request);
		#endif
	}
	public void ShowInterstitial(){
		#if SIMULATOR
		return;
		#endif
		#if UNITY_ANDROID || UNITY_IPHONE
		if (interstitial.IsLoaded()) {
			interstitial.Show();
		}
		#endif
	}
	public void DestroyInterstitial(){
		#if SIMULATOR
		return;
		#endif
		#if UNITY_ANDROID || UNITY_IPHONE
		interstitial.Destroy();
		#endif
	}
}