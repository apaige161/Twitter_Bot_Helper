
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TwitterBotDotNetHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            /*****TwitterBot post later helper program******/

            /***login***/

            //read passwords from file here using your own file path

            string pathOfApiKeys = $@".{Path.DirectorySeparatorChar}api_keys.txt";
            //read file and put contents into array
            string[] allKeys = File.ReadAllLines(pathOfApiKeys);

            string ApiKey = allKeys[0];
            string ApiKeySecret = allKeys[1];
            string AccessToken = allKeys[2];
            string AccessTokenSecret = allKeys[3];
            Console.WriteLine(ApiKey);
            // Set up your credentials (https://apps.twitter.com)
            Auth.SetUserCredentials(ApiKey, ApiKeySecret, AccessToken, AccessTokenSecret);


            //get current time
            DateTime currentTime = DateTime.Now;

            //get the user information from main program
            string userChoice = args[0]; //userchoice "3" text or "4" media
            Console.WriteLine(userChoice);
            if (userChoice == "3") // text
            {
                //not getting args[2]
                //("{0} \"{1}\" {2} {3} {4}", userInput, textToTweet, newTime, newTime, newTime);
                string textToTweet = args[1]; //grabs the text of tweet
                string newTimeDateString = args[2]; //grabs the date
                string newTimeTimeString = args[3]; //grabs the time
                string newTimeAmPmString = args[4]; //grabs AM or PM


                //test
                Console.WriteLine(textToTweet + " is the textToTweet from main");//text
                Console.WriteLine(newTimeDateString + " is the date from main");//date
                Console.WriteLine(newTimeTimeString + " is the time from main");//time
                Console.WriteLine(newTimeAmPmString + " is the AM or PM from main"); //AM or PM


                //add strings of DateTime data into a parseable form
                string newTimeString = newTimeDateString + " " + newTimeTimeString + " " + newTimeAmPmString;
                //parse to DateTime
                DateTime NewRealTime = DateTime.Parse(newTimeString);

                //display
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("twitter bot is attempting to post a text tweet");
                Console.WriteLine($"Your tweet will be published at {NewRealTime}");
                Console.ResetColor();

                TextWorker(NewRealTime, currentTime, textToTweet);

            } //text

            if (userChoice == "4") // media
            {
                string textToTweet = args[1]; //grabs the text of tweet
                string filePath = args[2]; //grabs file path of picture
                string newTimeDateString = args[3]; //grabs the date
                string newTimeTimeString = args[4]; //grabs the time
                string newTimeAmPmString = args[5]; //grabs AM or PM

                //test
                Console.WriteLine(textToTweet + " is the textToTweet from main");//text
                Console.WriteLine(filePath + " is the filepath from main");//date
                Console.WriteLine(newTimeDateString + " is the date from main");//time
                Console.WriteLine(newTimeTimeString + " is the time from main");//PM
                Console.WriteLine(newTimeAmPmString + " is the AM or PM from main"); //date


                //add strings of DateTime data into a parseable form
                string newTimeString = newTimeDateString + " " + newTimeTimeString + " " + newTimeAmPmString;
                //parse to DateTime
                DateTime NewRealTime = DateTime.Parse(newTimeString);

                //display
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("twitter bot is attempting to post a media tweet");
                Console.WriteLine($"Your tweet will be published at {NewRealTime}");
                Console.ResetColor();

                MediaWorker(NewRealTime, currentTime, textToTweet, filePath);
            } //media
            Console.ReadLine();
        }

        public static void TextWorker(DateTime newTime, DateTime currentTime, string textToTweet)
        {
            //datetime compare for while loop
            int timeCompare = DateTime.Compare(newTime, currentTime);

            /*  timeCompare:
             *  <0 − If date1 is earlier than date2
             *  0 − If date1 is the same as date2
             *  >0 − If date1 is later than date2
            */

            //waits until addTime is later than currentTime
            while (timeCompare > 0)
            {
                //end while loop, should end when timeCompare is changed
                Console.WriteLine("It is not time to post yet, the program will try again in 60 seconds...");
                Thread.Sleep(60000);   //wait for 60 seconds
                timeCompare = DateTime.Compare(newTime, DateTime.Now);

                /*****post tweet******/
                //runs program after while loop completes
                if (timeCompare < 0) //currentTime is later than addTime
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Tweet.PublishTweet(textToTweet);
                    Console.WriteLine("Tweet sent at " + DateTime.Now);
                    break;
                }
            }
        } //tweet text

        //tweet media

        public static void MediaWorker(DateTime newTime, DateTime currentTime, string textToTweet, string filePath)
        {
            //datetime compare for while loop
            int timeCompare = DateTime.Compare(newTime, currentTime);

            /*  timeCompare:
             *  <0 − If date1 is earlier than date2
             *  0 − If date1 is the same as date2
             *  >0 − If date1 is later than date2
            */

            //waits until addTime is later than currentTime
            while (timeCompare > 0)
            {
                //end while loop, should end when timeCompare is changed
                Console.WriteLine("It is not time to post yet, the program will try again in 60 seconds...");
                Thread.Sleep(60000);   //sleep for 60 seconds
                timeCompare = DateTime.Compare(newTime, DateTime.Now);

                /*****post tweet******/
                //runs program after while loop completes
                if (timeCompare < 0) //currentTime is later than addTime
                {
                    //pass in the file to post from the main program
                    byte[] file1 = File.ReadAllBytes(filePath);
                    var media = Upload.UploadBinary(file1);
                    Tweet.PublishTweet(textToTweet + " " + DateTime.Now, new PublishTweetOptionalParameters
                    {
                        Medias = new List<IMedia> { media }
                    });
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Tweet sent at " + DateTime.Now);
                    break;
                }

            }

        } //media tweet

    }
}
