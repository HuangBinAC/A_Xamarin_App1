using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Push;
using System.Collections.Generic;
using System.Text;

namespace App1
{
    [Activity(Label = "App1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            AppCenter.SetLogUrl("https://in-staging-south-centralus.staging.avalanch.es");
            Push.SetSenderId("359445903465");
            AppCenter.Start("3519e059-856f-4e62-a320-c8dc5d28db94",
                   typeof(Analytics), typeof(Crashes), typeof(Distribute), typeof(Push));

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // New code will go here

            // Get our UI controls from the loaded layout
            EditText phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            TextView translatedPhoneWord = FindViewById<TextView>(Resource.Id.TranslatedPhoneWord);
            Button translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            Button crashes_Button = FindViewById<Button>(Resource.Id.Crashes_Button);

            // Add code to translate number
            translateButton.Click += (sender, e) =>
            {
                // Translate user’s alphanumeric phone number to numeric
                string translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
                if (string.IsNullOrWhiteSpace(translatedNumber))
                {
                    translatedPhoneWord.Text = string.Empty;
                    Crashes.GenerateTestCrash();
                }
                else
                {
                    translatedPhoneWord.Text = translatedNumber;
                }
            };
            
            crashes_Button.Click += (sender, e) =>
            {
                Crashes.GenerateTestCrash();
            };

            Analytics.TrackEvent("Video clicked", new Dictionary<string, string> {
                { "Category", "Music" },
                { "FileName", "favorite.avi"}

            });

            
            Crashes.GetErrorAttachments = (ErrorReport report) =>
            {
                // Your code goes here.
                return new ErrorAttachmentLog[]
                {
                    ErrorAttachmentLog.AttachmentWithText("Hello world!", "hello.txt"),
                    ErrorAttachmentLog.AttachmentWithBinary(Encoding.UTF8.GetBytes("Fake image"), "fake_image.jpeg", "image/jpeg")
                };
            };

            // This should come before AppCenter.Start() is called
            Push.PushNotificationReceived += (sender, e) => {

                // Add the notification message and title to the message
                var summary = $"Push notification received:" +
                                    $"\n\tNotification title: {e.Title}" +
                                    $"\n\tMessage: {e.Message}";

                // If there is custom data associated with the notification,
                // print the entries
                if (e.CustomData != null)
                {
                    summary += "\n\tCustom data:\n";
                    foreach (var key in e.CustomData.Keys)
                    {
                        summary += $"\t\t{key} : {e.CustomData[key]}\n";
                    }
                }

                // Send the notification summary to debug output
                System.Diagnostics.Debug.WriteLine(summary);
            };


        }
    }
}

