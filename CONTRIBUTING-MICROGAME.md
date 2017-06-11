
Please take some time to read through this before you begin working on microgames. Let me know of any uncertainties.

First of all, this game is made using the [Unity engine](https://unity3d.com/get-unity/download). This little tutorial operates under the assumption that you know the very basics of programming in Unity. If you don't, there are like ten hundred million tutorials online on how to get started. If you've never used Unity before, I'd also recommend at least recreating a small simple game like Pong before you try to tackle this project.

**SUPER IMPORTANT:** You'll need to use Github for creating and submitting your microgame. It's easy to set up and allows you to work in the actual game project itself. Before you start programming formally, please download and install Github, [fork the repo](http://github.com/nitorinc/nitoriware/fork), and clone it on to your computer. If you need help with any of this, ping me (@Gman8r) on the server or just contact us on social media and I'll run you by the whole thing.

[Basics](#basics) 
[Design](#design)
[Structure](#structure)  
[Programming](#programming)  
[Debugging](#debugging)  

# Basics
The rundown of how to get started making your game:
* I recommend getting approval for what microgame you want before starting it, otherwise there's no guarantee we'll accept it.
* You can pick anything from [this list of ideas](https://docs.google.com/spreadsheets/d/1vkHnZLs6rY1f5YMF1qd97nF23XA4wCfg_2mmj1yf7lk/edit?usp=sharing) or you can add your own to the list with this [form](https://docs.google.com/forms/d/e/1FAIpQLScuBuUwSzit4u06Q7u2HGn51B7J0M6uvg7m_voEUdSTZvb6OQ/viewform).
* Every microgame needs an ID, basically a name for the microgame that doesn't change (as opposed to the actual in-game name that can be changed). It should be one word separated by caps (i.e. BroomRace, KogasaScare). Also try not to pick something generic like "Reimu", or "Touhou", or "Platformer". Two words or more is usually preferred.
* **IMPORTANT:** Check [this list](https://docs.google.com/spreadsheets/d/1pAbRPJQfDOsKXkEJRFOXd7wY6Tizlwt5xRCTJZqj7SQ/edit?usp=sharing) out to make sure your ID hasn't already been taken.
* When you decide the ID, create a new branch and begin working on your game! Branch name should ideally be "Microgame/\<YourMicrogameID\>/\<goal-of-this-branch\>".
* You should submit a pull request once you have a basically functional game and you debug it using the methods in [Debugging](#debugging)
* **IMPORTANT:** Don't leave any empty folders in your commit! Unity will auto-create .meta files but git will not copy the folder itself over, leading to a struggle between git and unity over the meta existing or not. Either put a placeholder file in empty folders or just delete them before sumbmitting.
* After your pull request is merged, update your repo from master and create a new branch every time you want to implement a feature or change, then send that as another pull request when you're done.
* If you want to know exactly what I will reject a pull request for, check out [this page](https://github.com/NitorInc/NitoriWare/wiki/Microgame-PR-Guidelines).

# Design
### How this game works
* If you're not at all familiar with WarioWare, go look at some gameplay videos. The concept is pretty easy to grasp.
* Each game should only ask a small task of the player in a very limited timeframe.
* Each game also has a short message displayed when it starts instructing the player what to do, referred to here as the "command".
* Two possible control schemes currently exist: Touhou style keyboard controls (Arrows + z/x), and Mouse moving and left-clicking. The player will be told which control scheme to use before the microgame starts.
* Each game can either be 8 or 16 musical beats long. At 130 BPM this means either around 3.5 seconds or 7 seconds. The majority of microgames should be 8 beats, but some warrant a longer time (often ones that are more puzzle-oriented).
* Each microgame ends with the player either passing or failing.
* When the player has won or lost the microgame, a voice clip will praise or scold the player accordingly.
* Three difficulties need to be made for each microgame (Easy, Medium, Hard). Each difficulty has the same basic goal or objective, but will ask for more of the player as difficulty rises. All three difficulties will need to be saved as separate scenes from each other.
* The game speeds up after a few microgames, so each microgame should be designed with speed increase in mind.
* The entire game is in 4:3 ratio, as will all microgames be.

### Some microgame design protips
* The player should have to think about what to do, but the solution should still be intuitive.
* Microgames are more interesting when more than just the gameplay changes between difficulty levels. Making a new setting, new outcome, or new characters for each difficulty adds a lot more variety.

# Structure
### Where to keep files and what to name them
* Create a file in Assets/Resources/Microgames/ named after your game's ID, and keep ALL assets for the microgame, including scenes, in that folder.
* The structure within that folder can be changed however you like.
* As for scenes, each difficulty must be a separate scene named [Game ID]+[Difficulty Number], (i.e. ReimuCash1).
* As you create the scenes, add them to the project build path
* Along with the scenes, three Traits assets are required in the same folder (More info on that in [Programming](#programming)).
* When you submit a pull request for your microgame, no files outside of your game's folder (except for the build path when adding your game) should be changed unless you run the changes by me beforehand.

# Programming

### Getting started
* To begin actually constructing a microgame, use the template in Assets/Resources/Microgames/_Template/Template.unity and save it as a new scene (check [File Structure](#structure) for scene-naming convention).
* You're welcome to mess with the main camera, but do _not_ remove Microgame Controller!
* **IMPORTANT:** Every script you make specifically for your microgame must be named beginning with the game ID (i.e. KogasaScarePlayer.cs).
* It's recommend that you name your assets beginning with the microgame ID as well.
* Next to the template stage, you'll notice three assets called Traits[1-3] in the same folder. Copy those to your microgame folder (You'll likely have to use Explorer).
* These contain certain traits of each of the three difficulties. View and edit them in the inspector by clicking on each one.
* For most microgames, most of these values will be exactly the same in each one, so multi-editing all three is recommended.
* You do NOT have to put these in any game scene. They are read automatically from the folder when your microgame is loaded.
* **IMPORTANT:** The traits themselves should NOT be edited by the game during runtime. There is a proper way to do this, so ask me if you need to.
* **IMPORTANT:** Don't instantiate GameObjects at the beginning of the microgame using the Awake() function! Use Start()! Otherwise the objects won't be destroyed properly when the game ends.

### Microgame traits
* Control Scheme: What controls the player should be using for the microgame. "Key" is arrow keys + z/x, "Mouse" is moving and left-clicking the mouse.
* Hide Cursor: Only applies to Mouse games. If true, the default cursor will not be visible when the microgame is loaded.
* Duration: Whether the microgame lasts 8 or 16 beats.
* Can End Early: Only used for 16 beat microgames (and pretty much true for every one). This will allow the game to be cut off early if the player wins or loses permanently.
* Command: The message displayed when the microgame starts.
* Default Victory: Default victory status. Basically, if the game ends right after it starts, will the player have won or lost?
* Music Clip: The audio clip that will be played for music when the microgame starts (*Don't* add the music in the microgame scene. If you want to hear the music in the microgame scene, enable "Play Music" in the Microgame Controller's debug settings)
* Voice Delays: Time between when the player has won or lost the microgame and when the voice clip congratulating or scolding them plays. If the game runs out of time, the voice clip is played immediately.

### How to handle player victory/loss
* When the player has won or lost the game, call the SetVictory() function in MicrogameController. This can be called from any script using MicrogameController.instance.setVictory(bool, bool).
* The function has two boolean parameters. The first one is whether the player has currently won or lost.
* The second one should be set to true if this outcome will not be changed before the microgame ends (player has won/lost permanently). This will trigger the voice clip after the delay time in Traits.

### Some notes on player input
* For keyboard controls, using Input.getKey() is fine.
* For mouse controls, you can't control where the player's cursor starts. Keep that in mind when designing.
* **IMPORTANT FOR MOUSE CONTROLS**: To get the player's cursor in world coordinates, call CameraHelper.getCursorPosition() instead of directly using Input.MousePosition or any other method.

### How to handle timing and increased speeds
* Tie everything to Time.timeScale! Make all your movement framerate-independent and use Time.deltaTime if you're moving something manually in Update().
* The music will change pitch automatically, but you should change the pitch of your sound effects accordingly. Do this by multiplying the AudioSource's pitch by Time.timeScale before you play a sound.
* Another timing note: the microgame loads slightly before the song starts, meaning it won't last *exactly* 8 (or 16) beats from when it starts. Read from MicrogameTimer.instance.beatsLeft if you want the absolute number of beats until the microgame ends.
* To properly emulate how long the game will wait before playing music, enable the debug setting "Simulate Start Delay".

### Using text in games
NitorInc. is being actively localized, but a lot of the microgame text is handled automatically. If your game is going to include text besides the microgame's name and the command text, refer to [this page](https://github.com/NitorInc/NitoriWare/wiki/Localizing-Text).

### Some additional assets that might be helpful
* Assets/Scripts/Helper is full of helper classes. These all have static functions that help facilitate various tasks (i.e. manipulating 2D vectors, ignoring collisions with a tag, determining whether an object is offscreen).
* Assets/Scripts/Generic Microgame Behaviors has various MonoBehavior scripts that perform basic functionalities you'd often use in microgames (i.e. snapping to the player's cursor, simple/comedic movements, object pooling, sceenshake).

### Good practices
(Not *mandatory*, per se, but keep these in mind)
* Make a commit for each step or change you make. This helps keep your workflow a lot more organized and makes it easier to handle mistakes.
* If you use the same object in all three of your scenes, consider making it a prefab. This will both cut down on the total size of your microgame and allow you to edit it without changing it in all three scenes. [Helpful tutorial](https://www.youtube.com/watch?v=0Jc287z4Qpg).
* It's best if a microgame folder's size is under 3-4 MB total. Don't worry about this so much when you're putting your game together or even sending a PR, but after the game is done consider doing some cleanup if you've went well above this..
* On that note, we don't need huge images and amazing quality textures. Don't be afraid to downsize them! For reference, a passable size for an entire-screen bg is 1024x768.
* Don't use GetComponent() in Update(). Use it in Awake() or Start() and store it in a variable, or assign it via inspector.

## Debugging

### Debugging in microgame scene (Debug Mode)
Playing your microgame scene by itself is referred to as "Debug Mode". The Microgame Controller object has some functionalities that will help smooth out debugging.
* Pressing R will reset the scene in debug mode, restarting the microgame completely without having to stop and restart from the editor.
* Pressing F will reset the scene, but play it one unit of speed faster, emulating the speed increase as the stage goes on.
* Pressing N will load the next difficulty of the same game in Debug Mode.
* The "Microgame Controller" object has debug options in the menu which can be expanded. These affect the scene in debug mode only, so change them however you wish to facilitate testing.
* Adjust the "Speed" slider in debug settings or use the F key to test higher speeds for your game. The game should still reasonably be winnable up to speeds 3 or 4.
* The Main Camera has an inactive FPS display attacked to it. Feel free to activate it when debugging but remember to disable it again before submitting your pull request.
* The game pauses/resumes when you press escape. There's no menu yet, it just freezes/unfreezes. Pausing should work fine for most microgames, but spend some time pausing and unpausing when you test to make sure.

### Debugging in Stage
**(Not required until you've finished all three difficulties)** Once your game works in Debug Mode, fire up the Nitori stage in Assets/Resources/Stages/Nitori/Nitori.unity and follow these steps to test your microgame in the actual game:
* The scene hierarchy has a GameObject called "Stage". Click on its child, "Controller", and you'll notice a script in the inspector called Stage Controller.
* Look down a few variables until you see "Microgame Pool". This is the list of microgames that the scene will load from and cycle through when it plays.
* To test your microgame, simply change the Size of the pool to 1 and put your game ID under Name. This will play all three difficulties in a row and then stay on the third difficulty, increasing speed each round.
* To lock the difficulty instead of having it increase, uncheck "Difficulty Increase On" above the Microgame Pool and set "Base Difficulty" under your game to the difficulty you wisht to test.
* The game will likely work in this scene if it worked in debug mode. If something unexpected goes awry, let me know.
* You should also test by building the project after you've done this before you submit your pull request.
* Once you're done with all that, revert any changes in Nitori.unity and the Stages folder in general (or just don't commit them) and you're free to submit that pull request if everything's functional!
