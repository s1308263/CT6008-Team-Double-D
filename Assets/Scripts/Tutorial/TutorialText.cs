using UnityEngine;

public class TutorialText : MonoBehaviour {

    public string[] tutorialValues;

    private void Awake() {
        tutorialValues = new string[9];

        tutorialValues[0] = new string("At ease pilot. Welcome to basic training. Here you will learn the controls of your aircraft for use in the fight with the invading aliens that threaten our very existence.");

        tutorialValues[1] = new string("You are currently in 'Landing Mode', use the 'Left Thumbstick' or 'D-Pad' on your controller or 'W A S D' on the keyboard to move your aircraft, for the next lesson you may want to gain some height.");

        tutorialValues[2] = new string("Once you have gotten familiar with that, press 'Left Shoulder Button' or the 'Spacebar' to change into 'Dogfight Mode'. Once you are in this mode press the 'Right Trigger' on your controller or 'W' on the keyboard to thrust forward and use the 'Left Thumbstick' on your controller or 'A' and 'D' on the keyboard to rotate your ship.");

        tutorialValues[3] = new string("Now for the fun stuff. Press 'A' on the controller or 'Left Click' on the mouse to fire your main gun.");

        tutorialValues[4] = new string("Next, hold 'X' on the controller or 'Right Mouse Button' to charge your homing missiles and release to fire.");

        tutorialValues[5] = new string("Ready for the big guns? Press 'B' on the controller or click in the 'Wheel' on the mouse to fire your super weapon. Don't forget that you only get one of these per mission, so only use it when you have no other option.");

        tutorialValues[6] = new string("Now that you know how to defend yourself, I'm going to send a few practice enemies your way. Make sure to keep moving to make it harder for them to hit you.");

        tutorialValues[7] = new string("Well done for taking them down, not many pilots pass this quickly. For this last lesson I need you to find and rescue the civilian in need of help. Once you find him, you can pick him up when in *Landing Mode*, make sure to bring him back to base to ensure his safety.");

        tutorialValues[8] = new string("And... that's everything. Remember, the more you move the harder it is for the aliens to hit you and to rescue as many civilians as you can. We can't let those aliens win. Good luck pilot, you've got this.");
    }
}
