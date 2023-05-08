# Project Butterfly Garden

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

### Student Info

-   Name: Judy Derrick
-   Section: 02

## Simulation Design

A peaceful butterfly garden filled with pretty butterflies and bees.

### Controls
- Clicking changes all agent's states
- Depending on the state, moving the cursor either causes the bees to follow the user, or the butterflies to flee from the user

## Butterfly

Just having a good time witht all the other butterflies

### Scared Of The User

**Objective:** Flee from the user's mouse

#### Steering Behaviors

- Flee the user's mouse, Stay In Bounds, Wander, Flock with other butterflies
- Obstacles - bees, user's mouse
- Seperation - other butterflies(to the best they can)
   
#### State Transistions

- default, but if user has a flower mouse cursor and they click, the cursor will switch back to the default cursor and trigger the state
   
### Not Scared Anymore

**Objective:** Stop running away from the user and just wander around

#### Steering Behaviors

- Wander, Stay In Bounds, Flock(separate, cohere, align) with other butterflies
- Obstacles - bees
- Seperation - bees (obstacle), other butterflies(to the best they can)
   
#### State Transistions

- user clicks, makes cursor into a flower disguise

## Bee

Just flying around and pollinating flowers

### Flying around

**Objective:** Just fly around and have a chill time

#### Steering Behaviors

- Wander, Stay In Bounds
- Obstacles - other bees (too busy working to socialize)
- Seperation - other bees
   
#### State Transistions

- default, but if user has a flower mouse cursor and they click, the cursor will switch back to the default cursor and trigger the state
   
### Pollinate Time

**Objective:** Seek the flower (user mouse)

#### Steering Behaviors

- Stay In Bounds, Seek the user's mosue
- Obstacles - other bees
- Seperation - other bees
   
#### State Transistions

- user clicks, makes cursor into a flower disguise

## Sources

-   Background Music by <a href="https://pixabay.com/users/music_for_videos-26992513/?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=112623">Music_For_Videos</a> from <a href="https://pixabay.com/music//?utm_source=link-attribution&amp;utm_medium=referral&amp;utm_campaign=music&amp;utm_content=112623">Pixabay</a>
-   Background Photo by <a href="https://unsplash.com/it/@david_sea?utm_source=unsplash&utm_medium=referral&utm_content=creditCopyText">David Sea</a> on <a href="https://unsplash.com/photos/8fiU55KXLKU?utm_source=unsplash&utm_medium=referral&utm_content=creditCopyText">Unsplash</a>

## Make it Your Own

- Made my own butterfly and bee sprite images, made my own flower cursor sprite, added background music, and background image

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_
- currently the bees sometimes manage to fly upside down

### Requirements not completed

_If you did not complete a project requirement, notate that here_

