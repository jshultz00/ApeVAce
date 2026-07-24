# KingKong

## Links
Assets: https://docs.google.com/document/d/1JU64dUjtbG-_SpR9aMOz3kA94F6t44sRXn-0gVJzWpI/edit?usp=sharing

## Monkey Team: TBD
- Make the monkey
- Animate model
- Health
- Make the city
- Background skyline
- Empire state building?
- Ledges

## Plane Team: TBD
-  Make the plane
-  Plane model
-  Propellers
-  Make the cockpit
-  Joystick
-  Throttle
-  Guns
-  HUD

## Stretch Goals:
-  Damage to the tower
-  Randomized ledges
-  Grabbing planes out of the air

## Scheduling:
-  Sprint 1
-  Placeholder assets
-  Something to represent player
-  Being able to look around, launch game

## Individual Roles
Github Manager: Justin

Video Editor: Jonah

Assets: Kritin

Unity Networking: Trevor

Brandon: N/A

## Sprint 1: Basic Unity Project Cloned?

Justin: Successful

Jonah: Successful

Kritin: Successful

Trevor: Successful

Brandon: Successful

## Sprint 2: Progress so far

Justin: Added network functionality. Couldn't get users to spawn in plane but they currently stand below the plane.

Jonah: Assissted with the formation of our city, edited video and summarized the group's progress during the sprint in the video.

Kritin: Created an entire city for the game via various assets from scratch. 

Trevor: Rudimentary joystick interaction, still needs work. Lots of trial and error. 

Brandon:

## Sprint 3: Progress

**Brandon**: Added ledge climbing and a VR scene "Tower" with randomly generated ledges for the player to climb to the top. (This is one of the core gameplay features of the game, necessary for the Kong player. Ledge script and tower scripts add to this functionality, allowing any rectangular object to have ledges randomly generated on it) All changes buildable and playable on objectTest branch

A video of the tower climbing:
https://youtu.be/jIuqqIUbXAQ

Also added rudimentary dart firing capability for the plane, with a projectile script to call Kong's damage function on hit when that's implemented.

**Trevor**: Created a reusable template for plane interactables. Importantly, the interactables can be removed from their parent without causing any problems, which is important because grabbing an object decouples it from its parent. Applied the interactable to the joystick and the throttle. The joystick is working exactly as intended. The player can grab the joystick and use it to control the rotation of the plane similarly to how joysticks work IRL. The throttle is more temperamental, and requires more testing before it works exactly as intended. Learned all about quaternions and inverse transforms. 


Packaged the plane into an asset, meaning we can drag and drop them into any scene in any project. All changes can be seen in main and in the plane testing branch. 

A video of the plane so far:
https://youtu.be/S2-RXRKpJjE
No idea why the colors look like that, everything looks much better in person.

**Justin**: Continued working with normcore networking. One user can currently play in multiplayer as a plane. The VR headset follows the motion of the plane (whose motion script was written by Trevor). However, after today's class, we realized our multiplayer might be easier to implement with PUN 2 and we have decided to switch from normcore to PUN 2. Thus, for the next sprint, we will be scrapping normcore and beginning a new functionality with PUN 2. The video of the plane above shows the current network functionality (notice the normcore avatar hands in the plane).

**Kritin**: Continued building the city and then had trouble previously with uploading on github due to file sizes. I believe I figured it out this time. Additionally, had a start on the UI (main menu). Just need to add XR functionality and scripts to that. It is a work in progress. Will need help from the CS guys in making sure the scripts are working properly and that it work in conjuction with the design elements moving forward. We plan to customize the background to more accuratley reflect OUR game as well as have the menu option with Start, Menu, and Quit. Believe it is a solid foundation for our project moving forward. Refrence video below for visuals.

Video of City and UI so far: https://youtu.be/NP7vWc345q0

**Jonah**: Have worked on the addition of UI alongside Kritin, as well as continuting to experiment the scope and size of our city that will be most conducive to the flying/combat experience. Strugling a bit with the implementation of the UI, specifically adding XR functionality. The group plans to review this in the coming days to get that part of the gaming experience up and running. Created and edited our biweekly sprint progress update video, which can be found right here: https://youtu.be/jJV_yscFFFE (Reference the youtube clips above to see video evidence of the progress that I mention in the video).

## Sprint 4

**Justin**: We completely scrapped normcore and switched over to pun2. I built the foundation for the networking, which was modified by Trevor to fully incorporate VR and be compatible with the Oculus headsets. The networking scenes can be found in the main branch of this repository. We have 3 spawn locations for the planes and 1 spawn location for the monkey and as of now, you can select whether you want to be a plane or a monkey. Based on that, you will spawn in the designated location. The plane also has full functionality but in the current scene the monkey does not. Next sprint, I will work on incorporating the networking into the actual city scene so that the planes can fly around the city. 

**Brandon**: Kong gameplay is completely finalized. Modified ledge spawning to reject positions that are too close to each other, and added logic to have ledges spawn on all four sides of the building. Only needs a pathfinding algorithm to determine if randomly generated ledges form a complete path from bottom to top (in most cases, a path exists). Added UI elements to the Kong player so that they can see their current height (UI Kong moves up as the real Kong moves up) and their current health (In example video below, Kong's health steadily decreases throughout the video (the inside of UI kong loses color). Created a script for Kong to facilitate this, as well as functions that determine win conditions for both teams. Last sprint will be spent implementing models for Kong's body and hands and integrating Kong with the other half of the game. Video: https://www.youtube.com/watch?v=hdYo5V42xJQ

**Trevor**: Worked with Justin to bugtest photon a little, plus completely overhauled the plane throttle system. The plane can now accelerate based on the distance from the throttle interactable to the throttle origin. From the perspective of the player, this means that pushing the throttle forward makes the plane accelerate forward, while pulling it backwards makes the plane accelerate backwards. Still have a few minor changes I'd like to make (planes dont fly backwards, plus hands would be nice) but the plane is completely flyable in any direction, and is fun to cruise around in. Also, weird bug with XR interactables lagging behind their parents. Might just have to deal with that. Currently changes are on main branch. Since the plane is an asset that is reused all over the project, and scene that uses a plane will be up to date. This includes planes that are spawned in using the multiplayer script. Check the "plane" scene for an example, but also the "multiplayer start" menu can use our UI to spawn planes. 

![image](https://user-images.githubusercontent.com/64612207/199861900-f51f6c03-7e25-4d80-9663-5c5e680a4798.png)

Here's the asset hierarchy and the placeholder cockpit art. The hierarchy is here to stay and took a while to figure out, but the joystick and throttle will be replaced soon

**Jonah and Kritin**: After testing, Kritin and Jonah discovered that we needed to rework the city a bit, because the amount of components in the city broke the game when we tested it. So, we reduced the components of the city. Our last step to do for the city is to increase the scale in order to still be able to expand the city. The city needed to be expanded to give the pilot more room to fly, the way it looked before this change simply was not the immersive experience we are seeking to create with this game.  The City scene can be viewed by going to scnes> City scene (test). There ae fewer building as well as hundreds of smaller assets were deleted. Here is the video for the city scene: https://youtu.be/0eGGMjXZlXU. Where the mouse is circling is where we plan on inserting the central tower where Kong (the user) will be climbing up. We will test next week whether this amount of reduced assets will allow the game to still function. If it doesn't more assets will be deleted. The CS guys were able to help us with the creation of the UI, although it is still very bare-bones right now and there is plenty of room to make it more aestically pleasing and fitting to the comprehensive gaming experience that we are building towards. 

Jonah created and edited our biweekly sprint progress update video, which can be found right here: https://youtu.be/JB0BxEx2u58 (Reference the youtube clips above to see video evidence of the progress that I mention in the video).

## Sprint 5

**Trevor**:
I did a whole lot this week. The plane is fully working and controls perfectly. The pilot can shoot bullets from the front of the plane by gripping the joystick in the plane and pressing the A button on the right controller. When firing, the controller receives haptic feedback. The bullets have a firerate that can be edited in the script. Firing a bullet adds a certain amount of heat to the gun. Once the gun reaches max heat, the pilot has to wait for the gun to cool all the way down before firing again. The gun cools down a set amount each frame. This prohibits the player from constantly firing, and gives incentive to aim precisely. 

The plane now has two dials on the dashboard. One represents the heat of the gun, and the other represents the velocity of the plane. The dial spins to represent the current values of each variable. Also, the propellor of the plane spins as a factor of the speed of the plane. 

The throttle now has a slower acceleration factor, meaning the player can more finely adjust their velocity. Also, the plane can't go backwards anymore. Grabbing interactables now makes the interactable a child of the controller. Pretty much ignores most of the prebuilt XR code. It really helps with rubber banding and general responsiveness.

The pilot now has hands that represent the location of their controllers. When the pilot grabs an interactabe, the hands add themselves to the model of the interactable's model, which makes it seem like the player is grabbing the interactable and not just an invisible cube (which is what they're really grabbing). This had some scaling issues resulting in stretchy hands, so I restructured the hierarchy of the plane in order to maintain a consistent scaling in the throttle and joystick models. The weird shaking bug has been addressed, it was a result of the physics timestep being out of sync with the headset refresh rate.

All I need to add before the final release is: Plane collisions, sound

https://youtu.be/u070Bch3k2g

In the video, I showcase the hand control, and then shooting the gun. Notice how the heat dial increases until hitting its maximum, and then I am unable to shoot until it cools down. Afterwards, I accelerate the plane and fly around before I slow to a stop and "land"

Still need to add collisions and sound, but just about done. If for some reason these changes aren't visible, they're preserved on the "tempBranch" branch. 

**Justin**:
After having some issues with our current network, I decided to try and create a completely new network. This network was supposed to have a waiting room that waited until the selected number of players joined the room. Then, the first person would spawn as Kong and the others as planes. However, after countless hours of work, testing, and debugging, I realized that this new network functionality was no better if not worse than the previous one. Thus, I decided to revert to the old network. Trevor and I tested it and individually, the plane and monkey work. However, we are still struggling to figure out how to separate the users from each other in multiplayer gameplay. At the moment, when two users are in the game, the players spawn to the same location and are not able to control their characters. However, when only one person is playing, they are able to control their characters. We believe this issue is occurring due to a lack of RPC functionality in our game, but we believe this will not be difficult to solve now that we know exactly what the issue is. I am currently developing a network in a non-VR version of our game and will convert the project to integrate VR as soon as I get it up and running.

**Brandon**:
Removed visible rays and replaced them with hands that animate between idle and grabbing, as well as adding code that highlights a ledge when it's aimed at. This is designed to make climbing more immersive while minimizing frustration from not having the exact raycast viewable anymore.

Additionally, I refactored Kong code so that some questionable design decisions are fixed (there used to be some issues with grabbing two different ledges at the same time). Used a workaround so that you can't grab the same ledge anymore (by disabling the collider), this isn't an optimal solution and I'll be spending time trying to find a better way. 

Lastly, I've added a 'jump' to Kong. This is activated by releasing the Grip button on the controller while in the apex of a climbing motion. To avoid a softlock scenario, Kong falls back to around where he originally was after the jump. This is designed so that ledges that are further apart may still be traversable, since latching onto a ledge after a jump resets Kong's momentum, allowing him to continue further up the tower. The jump is demonstrated best around 0:30 of the attached video. This theoretically should remove the need for guaranteeing a path to the top, since this move gives Kong a much greater degree of mobility than before.

https://youtu.be/0Ocs1J4Otuk

**Jonah and Kritin**:
This week, we first focused on erasing assets in the city. When we tested the game last week, it was constantly crashing, so we knew that the total amount of assets in the city had to be significantly reduced. Kritin started this process by reducing hundreds of minor assets, and we continued this process together by continuing to eliminate any assets that we deemed unecessary to the gaming experience. After erasing the assets to a point where the game did not crash when we started it, we added buildings and a the large skyscraper in the middle for kong to climb. Once we completed this, we realized that the city still looked small, and needed something to surrond it. The way the game was playing with just the city as the space where you could fly in felt limiting and not representative of the flying and freeing experience we are searching for in this virtual reality experience. So, we fixed this by adding an ocean around the city, extending the range in which one can fly and no longer making the city feel so small and confining. We love the aesthtic of the water around the city, as it feels almost like a small-scale version of Manhatten. Finally, Jonah made our weekly sprint video in order to provide a consicse and cohesive update of our group's progress for this sprint, linked here: https://youtu.be/Z169oX3UZEs
