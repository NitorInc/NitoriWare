Please take some time to read through this before you begin working on microgames. Let me know of any uncertainties.

First of all, this game is made using the [Unity engine](https://unity3d.com/get-unity/download). This little tutorial operates under the assumption that you know the very basics of programming in Unity. If you don't, there are like ten hundred million tutorials online on how to get started. If you've never used Unity before, I'd also recommend at least recreating a small simple game like Pong before you try to tackle this project.

**SUPER IMPORTANT:** You'll need to use Github for creating and submitting your microgame. It's easy to set up and allows you to work in the actual game project itself. Before you start programming formally, please download and install Github, [fork the repo](http://github.com/nitorinc/nitoriware/fork), and clone it on to your computer. If you need help with any of this, ping me (@Gman8r) on the server or just contact us on social media and I'll run you by the whole thing.

[Basics](#basics)  
[Design](#design)  
[Structure](#structure)  
[Programming](#programming)  
[Debugging](#debugging)  

## Basics
The rundown of how to get started making your game:
* I recommend getting approval for what microgame you want before starting it, otherwise there's no guarantee we'll accept it.
* You can pick anything from [this list of ideas](https://docs.google.com/spreadsheets/d/1vkHnZLs6rY1f5YMF1qd97nF23XA4wCfg_2mmj1yf7lk/edit?usp=sharing) or you can add your own to the list with this [form](https://docs.google.com/forms/d/e/1FAIpQLScuBuUwSzit4u06Q7u2HGn51B7J0M6uvg7m_voEUdSTZvb6OQ/viewform).
* Every microgame needs an ID, basically a name for the microgame that doesn't change (as opposed to the actual in-game name that can be changed). It should be one word separated by caps (i.e. BroomRace, KogasaScare). Also try not to pick something generic like "Reimu", or "Touhou", or "Platformer". Two words or more is usually preferred.
* **IMPORTANT:** Check [this list](https://docs.google.com/spreadsheets/d/1pAbRPJQfDOsKXkEJRFOXd7wY6Tizlwt5xRCTJZqj7SQ/edit?usp=sharing) out to make sure your ID hasn't already been taken.
* When you decide the ID, create a new branch and begin working on your game! Branch name should ideally be "Microgame/\<YourMicrogameID\>/\<goal-of-this-branch\>".
* You should submit a pull request once you have a basically functional game and you debug it using the methods in [Debugging](#debugging)
* **IMPORTANT:** Don't leave any empty folders in your commit! Unity will auto-create .meta files but git will not copy the folder itself over, leading to a struggle between git and unity over the meta existing or not. Either put a placeholder file in empty folders or just delete them before sumbmitting.
* After your first pull request, keep making a new branch and repeating the same process every time you want to implement a feature or change.

## Design
How your game should work mechanically and what purpose it should serve:
* If you're not at all familiar with WarioWare, go look at some gameplay videos. The concept is pretty easy to grasp.
* Each game should only ask a small task of the player in a very limited timeframe.
* Each game also has a short message displayed when it starts instructing the player what to do.
* Two possible control schemes currently exist: Touhou style keyboard controls (Arrows + z/x), and Mouse moving and left-clicking. The player will be told which control scheme to use before the microgame starts.
* Each game can either be 8 or 16 musical beats long. At 130 BPM this means either around 3.5 seconds or 7 seconds. The majority of microgames should be 8 beats, but some warrant a longer time.
* Each microgame ends with the player either passing or failing.
* Three difficulties need to be made for each microgame (Easy, Medium, Hard). Each difficulty has the same basic goal or objective, but will ask for more of the player as difficulty rises. All three difficulties will need to be saved as separate scenes from each other.
* The game speeds up after a few microgames, so each microgame should be designed with speed increase in mind.

Some design protips:
* The player should have to think about what to do, but the solution should still be intuitive.
* Microgames are more interesting when more than just the gameplay changes between difficulty levels. Making a new setting, new outcome, or new characters for each difficulty adds a lot more variety.

## Structure
Where to keep files and what to name them:
* Create a file in Assets/Resources/Microgames/ named after your game's ID, and keep ALL assets for the microgame, including scenes, in that folder.
* The structure within that folder can be changed however you like.
* As for scenes, each difficulty must be a separate scene named [Game ID]+[Difficulty Number], (i.e. ReimuCash1).
* As you create the scenes, add them to the project build path
* Open Assets/Resources/MicrogameInfo.txt and add your game's name, control scheme, and command message. The format should be easy to follow. (This is a temporary requirement while we work on an alternative)
* When you submit a pull request for your microgame, the only files that should be changed outside of its ID folder should be MicrogameInfo.txt and the build path.

## Programming
Guidelines for putting your microgame together:
* To begin actually constructing a microgame, use the template in Assets/Resources/Microgames/_Template/Template.unity and save it as a new scene (check [File Structure](#structure) for scene-naming convention).
* You're welcome to mess with the main camera, but do _not_ remove Microgame Controller!
* **IMPORTANT:** Every script you make specifically for your microgame must be named beginning with the game ID (i.e. KogasaScarePlayer.cs).
* I personally like to name all my microgame assets with that method to facilitate searching, but it's your decision.

The "Microgame Controller" prefab in the scene has an attached script (MicrogameController.cs) with important inspector variables:
* Command: The message displayed when the microgame starts (Must also be added to MicrogameInfo.txt)
* Control Scheme: What controls the player should be using for the microgame (Must also be added to MicrogameInfo.txt). "Touhou" is arrow keys + z/x, "Mouse" is moving and left-clicking the mouse
* Default Victory: Default victory status. Basically, if the game ends right after it starts, will the player have won or lost?
* Can End Early: Only used for 16 beat microgames, this will allow the game to be cut off early if the player wins or loses permanently
* Music Clip: The audio clip that will be played for music when the microgame starts (*Don't* play the music in the microgame manually. If you want to hear the music in the microgame scene, check off Debug Music in the Microgame Controller)
* The rest are debug variables covered in [Debugging](#debugging).
* **IMPORTANT:** Don't instantiate GameObjects at the beginning of the microgame using the Awake() function! Use Start()! Awake() will execute before the controller sets the active scene to the microgame one, meaning the objects will be created in the wrong scene.

How to handle player victory/loss:
* When the player has won or lost the game, call the SetVictory() function in MicrogameController. This can be called fro any script using MicrogameController.instance.setVictory() .
* The function has two boolean parameters. The first one is whether the player has won or lost, and the second should be set to true if this outcome will not be changed before the microgame ends (player has won/lost permanently).

Some notes on player input:
* For keyboard controls, using Input.getKey() is fine.
* For mouse controls, you can't control where the player's cursor starts. Keep that in mind when designing.
* **IMPORTANT FOR MOUSE CONTROLS**: To get the player's cursor in world coordinates, call CameraHelper.getCursorPosition() instead of converting from Input.MousePosition.
* The cursor is visible by default in mouse games, set Cursor.visible to false at the start of your microgame to override that (if you want a gameobject to follow the cursor instead of having it visible).

How to handle timing and increased speeds:
* Tie everything to Time.timeScale! Make all your movement framerate-independent and use Time.deltaTime if you're moving something manually in Update().
* The music will change pitch automatically, but you should change the pitch of your sound effects accordingly. Do this by multiplying the AudioSource's pitch by Time.timeScale every time you play a sound.
* Another timing note: the microgame loads slightly before the song starts, meaning it won't last *exactly* 8 (or 16) beats from when it starts. Read from MicrogameTimer.instance.beatsLeft if you want the absolute number of beats until the microgame ends.

You don't need these, but here are some additional classes that might be helpful:
* Assets/Scripts/Helper is full of helper classes. These all have static functions that help facilitate various tasks (i.e. manipulating 2D vectors, ignoring collisions with a tag, determining whether an object is offscreen).
* Assets/Scripts/Generic Microgame Behaviors has various MonoBehavior scripts that perform basic functionalities you'd often use in microgames (i.e. locking onto the player's cursor, having an object move back and forth, object pooling, sceenshake).

## Debugging
Playing your microgame scene by itself is referred to as "Debug Mode". The Microgame Controller object has some functionalities that will help smooth out debugging:
* Pressing R will reset the scene in debug mode, restarting the microgame completely without having to stop and restart from the editor.
* The MicrogameController script has several variables in the inspector beginning with "Debug". *Debug Objects is not to be messed with*, but the other ones will all change how the game is played in debug mode (mute music, don't display command, etc.) but won't affect its playback in the real game.
* The Main Camera has an inactive FPS display attacked to it. Feel free to activate it in debug mode but remember to disable it again before submitting your pull request.

Once your game is working just fine in Debug Mode, fire up the Nitori stage in Assets/Resources/Scenarios/Nitori/Nitori.unity and follow these steps to test your microgame in the actual game. This one is a bit hard to explain but bear with me:
* First, make sure you've added the game info to Assets/Resources/MicrogameInfo.txts and that you've added your microgame scenes to the build path, or this won't work,
* The scene hierarchy has a GameObject called Scenario at its root. Click on it and you'll notice an attached script called Scenario Controller.
* Look down a few variables until you see Microgame Pool. This is the list of microgames that the scene will load from and cycle through when it plays, including what difficulty the microgame will be the first time it's played. You can resize and edit it however you want to include your microgame.
* You can change the variables above Microgame Pool to change various things about how the microgames are loaded: Whether the game order is shuffled, whether the game speeds up after a couple microgames, whether the microgame gets harder after each full cycle, the speed the game plays at, and an option to mute music.
* If you're confused and you want to just test your microgame, use this setup: set shuffle and speed increase to off, and make your microgame the only element in Microgame Pool. If you want to test just one difficulty, set the base difficulty to whichever you wish and disable difficulty increase. If you want to test all three difficulties in a row, enable difficulty increase and set base difficulty to 1.
* The game will likely work in this scene if it worked in debug mode. If something unexpected goes awry, let me know.
* You should also test by building the project after you've done this before you submit your pull request.
* Once you're done with all that, revert the changes in Nitori.unity (or just don't commit them) and you're free to submit that pull request if everything's functional!
