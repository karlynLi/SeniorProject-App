using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using road_running.Droid;
using road_running.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(GoogleManager))]
namespace road_running.Droid
{
	public class GoogleManager : Java.Lang.Object, IGoogleManager, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
	{
		public Action<GoogleUser, string> _onLoginComplete;
		public static GoogleApiClient _googleApiClient { get; set; }
		public static GoogleManager Instance { get; private set; }
		Context _context;

		public GoogleManager()
		{
			_context = global::Android.App.Application.Context;
			Instance = this;
		}

		public void Login(Action<GoogleUser, string> onLoginComplete)
		{
			GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
															 //.RequestIdToken("593390370555-peu8fuupn2lgqki6ocagbvvcsfiuql7e.apps.googleusercontent.com")
															 .RequestEmail()
															 .Build();

			//_mGoogleSignInClient = GoogleSignIn.GetClient(this, gso);
			_googleApiClient = new GoogleApiClient.Builder((_context).ApplicationContext)
				.AddConnectionCallbacks(this)
				.AddOnConnectionFailedListener(this)
				.AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
				.AddScope(new Scope(Scopes.Profile))
				.Build();

			_onLoginComplete = onLoginComplete;
			Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(_googleApiClient);
			// 開始登入並於MainActivity的OnActivityResult取得結果
			((MainActivity)Forms.Context).StartActivityForResult(signInIntent, 1); // 如果登入成功OnActivityResult中的resultCode應該與這邊的1一樣
			_googleApiClient.Connect(); // 連接API
		}

		public void Logout()
		{
			var gsoBuilder = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn).RequestEmail();

			GoogleSignIn.GetClient(_context, gsoBuilder.Build())?.SignOut();
			if (_googleApiClient != null)
            {
				_googleApiClient.Disconnect();
			}
			

			Console.WriteLine("google登出");
		}

		// Google登入返回結果
		public void OnAuthCompleted(GoogleSignInResult result) 
		{
			if (result.IsSuccess) // 登入成功
			{
				GoogleSignInAccount accountt = result.SignInAccount;
				Console.WriteLine("Google帳戶ID:  " + accountt.Id);
				Console.WriteLine("Google帳戶名稱:  " + accountt.DisplayName);
				Console.WriteLine("Google帳戶mail:  " + accountt.Email);
				Console.WriteLine("Google帳戶PhotoUrl: " + accountt.PhotoUrl);
				_onLoginComplete?.Invoke(new GoogleUser()
				{
					Google_ID = accountt.Id,
					Name = accountt.DisplayName,
					Email = accountt.Email,
					// 如果有大頭照則顯示，沒有則使用https://autisticdating.net/imgs/profile-placeholder.jpg為預設
					Picture = new Uri((accountt.PhotoUrl != null ? $"{accountt.PhotoUrl}" : $"https://autisticdating.net/imgs/profile-placeholder.jpg"))
				}, string.Empty);
			}
			else //登入失敗
			{
				Console.WriteLine(result.Status  +  "|"  + result.IsSuccess);
				Console.WriteLine("google登入失敗");
				//_onLoginComplete?.Invoke(null, "An error occured!");
			}
		}

		public void OnConnected(Bundle connectionHint)
		{

		}

		public void OnConnectionSuspended(int cause)
		{
			_onLoginComplete?.Invoke(null, "Canceled!");
		}

		public void OnConnectionFailed(ConnectionResult result)
		{
			_onLoginComplete?.Invoke(null, result.ErrorMessage);
		}
	}
}
