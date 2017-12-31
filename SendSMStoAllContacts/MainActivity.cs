using Android.App;
using Android.Widget;
using Android.OS;
using Android.Provider;
using System.Collections.Generic;
using Plugin.Messaging;

namespace SendSMStoAllContacts
{
    [Activity(Label = "Message All!", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        Button button;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            button = FindViewById<Button>(Resource.Id.button1);
            string message = FindViewById<EditText>(Resource.Id.editText1).Text;
            List<string> nums = GetContacts();
            button.Click += (sender, e) => nums.ForEach(ph1 => SendMessage(ph1, message));
            // "1" = "one", so "ph1" = "phone"
            // geddit geddit? XD
        }

        private List<string> GetContacts()
        {
            List<string> allNumbers = new List<string>();
            var phones = Application.Context.ContentResolver.Query(
                            ContactsContract.CommonDataKinds.Phone.ContentUri, null,
                            ContactsContract.CommonDataKinds.Phone.InterfaceConsts.ContactId,
                            null, null);

            while (phones.MoveToNext())
            {
                allNumbers.Add(phones.GetString(phones.GetColumnIndex(
                    ContactsContract.CommonDataKinds.Phone.Number)));
            }
            phones.Close();
            return allNumbers;
        }

        private void SendMessage(string number, string text)
        {
            number = number.Replace(' ', '\0');
            number = number.Replace('-', '\0');
            if (number.Length < 11)
                return;
                // Now, you can't send an sms to a BTCL number, can you?! :p

            CrossMessaging.Current.SmsMessenger.SendSmsInBackground(number, text);
        }
    }
}

