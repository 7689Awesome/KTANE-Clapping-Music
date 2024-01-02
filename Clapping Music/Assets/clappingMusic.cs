using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;

public class clappingMusic: MonoBehaviour {

   public KMBombInfo bomb;
   public KMAudio audio;

   public KMSelectable ClapButton;
   public KMSelectable RestButton;

   public String[] pattern1;
   public String[] pattern2;
   public String[] pattern3;
   public String[] pattern4;
   public String[] pattern5;
   public String[] pattern6;
   public String[] pattern7;
   public String[] pattern8;
   public String[] pattern9;
   public String[] pattern10;
   public String[] pattern11;
   public String[] pattern12;

   private String[] correctPattern = new String[12];
   private String[] clapAnswer = new String[12];
   string correctPatternArray;
   string answeredPatternArray;
   int correctPatternNumber = 0;
   int stage = 0;
   int correct = 0;

   static int ModuleIdCounter = 1;
   int moduleId;
   private bool ModuleSolved;

   void Awake() { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.

   moduleId = ModuleIdCounter++;
   /*foreach (KMSelectable object in keypad)
   {
      KMSelectable pressObject = object;
      object.OnInteract += delegate () { keypadPress(pressedObject); return false; };
   }
   */ 
   ClapButton.OnInteract += delegate () { PressClapButton(); return false; };
   RestButton.OnInteract += delegate () { PressRestButton(); return false; };

   }

   void Start() { //Shit that you calculate, usually a majority if not all of the module
      DeterminePattern();
   }

   void DeterminePattern(){
      if(bomb.IsIndicatorOn("BOB")){
      correctPatternNumber = 1;
      Debug.LogFormat("[Clapping Music #{0}] Pattern 1 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern1[i];
         }

      } else if(bomb.GetBatteryCount()==0){
      correctPatternNumber = 2;   
      Debug.LogFormat("[Clapping Music #{0}] Pattern 2 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern2[i];
         }

      } else if(bomb.GetPortCount(Port.PS2) == 1){
      correctPatternNumber = 3;      
      Debug.LogFormat("[Clapping Music #{0}] Pattern 3 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern3[i];
         }

      } else if(bomb.GetSerialNumberNumbers().Last() == 0){
         correctPatternNumber = 4;
         Debug.LogFormat("[Clapping Music #{0}] Pattern 4 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern4[i];
         }
      } else if(bomb.GetPortCount(Port.Parallel) == 1){
         correctPatternNumber = 5;
         Debug.LogFormat("[Clapping Music #{0}] Pattern 5 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern5[i];
         }
      } else if(bomb.IsIndicatorOff("CLR")){
         correctPatternNumber = 6;
         Debug.LogFormat("[Clapping Music #{0}] Pattern 6 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern6[i];
         }
      } else if((bomb.GetSerialNumberNumbers().Last() == 2) || (bomb.GetSerialNumberNumbers().Last() == 3) || (bomb.GetSerialNumberNumbers().Last() == 5) || (bomb.GetSerialNumberNumbers().Last() == 7)){
         correctPatternNumber = 7;
         Debug.LogFormat("[Clapping Music #{0}] Pattern 7 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern7[i];
         }
      } else if(bomb.GetPortPlateCount() % 2 == 0){
         correctPatternNumber = 8;
         Debug.LogFormat("[Clapping Music #{0}] Pattern 8 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern8[i];
         }
      } else if(bomb.IsIndicatorOn("CLR") || bomb.IsIndicatorOn("NSA")){
         correctPatternNumber = 9;
        Debug.LogFormat("[Clapping Music #{0}] Pattern 9 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern9[i];
         }
      } else if(bomb.GetPortCount(Port.Parallel) == bomb.GetPortCount(Port.Serial)){
         correctPatternNumber = 10;
         Debug.LogFormat("[Clapping Music #{0}] Pattern 10 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern10[i];
         }
      } else if(bomb.GetPortCount(Port.DVI) % 2 == 0){
         correctPatternNumber = 11;
         Debug.LogFormat("[Clapping Music #{0}] Pattern 11 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern11[i];
         }
      } else {
         correctPatternNumber = 12;
         Debug.LogFormat("[Clapping Music #{0}] Pattern 12 is the correct pattern.", moduleId);
         for(int i = 0; i<12; i++){
            correctPattern[i] = pattern12[i];
         }   
      }
   }

void LogCorrectPatternArray() {
   correctPatternArray = "[ " + string.Join(", ", correctPattern) + " ]";
   Debug.LogFormat("[Clapping Music #{0}] The correct clapping pattern is: {1}. ", moduleId, correctPatternArray);
}

void LogAnsweredPatternArray() {
   answeredPatternArray = "[ " + string.Join(", ", clapAnswer) + " ]";
   Debug.LogFormat("[Clapping Music #{0}] You answered: {1}. ", moduleId, answeredPatternArray);
}

   void PressClapButton(){

      ClapButton.AddInteractionPunch();
      if (ModuleSolved){
         return;
      }

      audio.PlaySoundAtTransform("CLAP SOUND", transform);
      clapAnswer[stage] = "Clap";
      stage += 1;

      if (stage == 12){
         for(int j = 0; j<12; j++){
            if (string.Equals(clapAnswer[j], correctPattern[j], StringComparison.OrdinalIgnoreCase)){
         correct += 1;
      }

      }
         if (correct == 12){
            LogCorrectPatternArray();
            LogAnsweredPatternArray();
            Debug.LogFormat("[Clapping Music #{0}] Patterns matched! Module solved!", moduleId);
            Solve();
         } else{
            LogCorrectPatternArray();
            LogAnsweredPatternArray();
            Debug.LogFormat("[Clapping Music #{0}] Patterns did not match. Strike!", moduleId);
            Strike();
            Array.Clear(clapAnswer,0, clapAnswer.Length);
            Debug.Log("Reset!");
         }
         stage = 0;
         correct = 0;
      }
      }

   void PressRestButton(){

      RestButton.AddInteractionPunch();
      if (ModuleSolved){
         return;
      }

      audio.PlaySoundAtTransform("REST SOUND", transform);
      clapAnswer[stage] = "Rest";
      stage += 1;

      if (stage == 12){
         for(int a = 0; a<12; a++){
            if (string.Equals(clapAnswer[a], correctPattern[a], StringComparison.OrdinalIgnoreCase)){
         correct += 1;
      }
         }

         if (correct == 12){
            LogCorrectPatternArray();
            LogAnsweredPatternArray();
            Debug.LogFormat("[Clapping Music #{0}] Patterns matched! Module solved!", moduleId);
            Solve();
            
         } else{
            LogCorrectPatternArray();
            LogAnsweredPatternArray();
            Debug.LogFormat("[Clapping Music #{0}] Patterns did not match. Strike!", moduleId);
            Strike();
            Array.Clear(clapAnswer,0, clapAnswer.Length);
            Debug.Log("Reset!");
         }
         stage = 0;
         correct = 0;
      }
   }


   void Solve() {
      audio.PlaySoundAtTransform("SOLVED SOUND", transform);
      GetComponent<KMBombModule>().HandlePass();
      Debug.Log("Module Solved!");
   }

   void Strike() {
      GetComponent<KMBombModule>().HandleStrike();
      Debug.Log("Strike! Try again.");
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} C/R for clap/rest respectively. Chain commands via spaces.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      Command = Command.Trim().ToUpper();
      yield return null;
      string[] Commands = Command.Split(' ');
      for (int i = 0; i<Commands.Length; i++){
         Debug.Log(Commands[i]);
         if ((Commands[i] != "C" && Commands[i] != "R") || Commands[i].Length != 1){
            yield return "sendtochaterror I don't understand!";
            yield break;
         }
      }

      for (int i = 0; i< Commands.Length; i++){
         if(Commands[i] == "C"){
            PressClapButton();
         } else if (Commands[i] == "R"){
            PressRestButton();
         }
         yield return new WaitForSeconds(.1f);
      }
      
   }

   IEnumerator TwitchHandleForcedSolve () {

      for(int d = 0; d<12; d++){
         if(correctPattern[d] == "Clap"){
            PressClapButton();
         } else if (correctPattern[d] == "Rest"){
            PressRestButton();
         }
         yield return new WaitForSeconds(.1f);
      }
      yield return null;
   }
}
