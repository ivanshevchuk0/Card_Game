# Card Game

![image1](images/image1.png)

## Project Description

**Card Game** is a Unity card game prototype that implements core mechanics such as card comparison, elemental effects, abilities, and visual feedback. The project demonstrates architecture using prefabs, Scriptable Objects, events, and visual effects.

The game features two participants: the player and the computer. Each round, both reveal cards, compare values and elements, while abilities and effects add strategic depth to the gameplay.

---

## Screenshots

### Main Scene and Object Structure
![image1](images/image1.png)
_Scene hierarchy, game controller, spawns, and card prefabs._

### Battle Process and Console Errors
![image2](images/image2.png)
_Round comparison of cards, Unity console error and log display._

### Card Comparison and Elemental Effects
![image3](images/image3.png)
_Center: player vs computer card comparison, pop-up effects, and ability indication._

### Win Screen and Round Result
![image4](images/image4.png)
_Win message, card counts, round outcome, and visual effects._

---

## Core Gameplay Mechanics

### 1. Basic Card Battle System
- Player vs Computer card comparison
- Random card selection from hands
- Power value comparison to determine round winner
- Win/lose/draw conditions

### 2. Deck System
- Shuffled deck at game start
- Drawing cards for both players
- Card removal from deck when drawn
- Starting hand size configuration

### 3. Hand Management
- Visual representation of cards in hand
- Face-down display for opponent's cards
- Card count tracking

---

## Elemental Affinity System

### 4. Element Types
- Fire, Water, Earth elements
- Neutral (None) element type

### 5. Elemental Strengths/Weaknesses
- Fire beats Earth (2x power)
- Earth beats Water (2x power)
- Water beats Fire (2x power)
- Weak elements deal half damage

### 6. Elemental Visuals
- Colored particle effects during battles
- Element icons on cards
- Color-coded element indicators

---

## Special Ability System

### 7. Ability Types
- Draw Extra: Draw additional cards on win
- Steal Card: Take opponent's card on win
- Destroy on Loss: Remove opponent's card when losing
- Double Power: Boost power when losing
- Revive Random: Bring back a random card from deck

### 8. Ability Triggers
- On-win abilities
- On-loss abilities
- Conditional activation

### 9. Ability Visuals
- Pulsing ability indicators
- Unique icons for each ability type
- Highlight colors for ability cards
- Ability activation messages

---

## Visual Feedback Systems

### 10. Card Display
- Different positions for player/computer cards
- Center play area for battling cards
- Card sprites and artwork display

### 11. Game State Feedback
- Round result messages
- Ability activation notifications
- Card count displays
- Game over announcements

---

## Game Flow Mechanics

### 12. Round System
- Space bar to initiate rounds
- Automatic hand refresh after rounds
- Delay between rounds for effect viewing

### 13. Game Progression
- Automatic win condition checking
- Game reset after victory
- Continuous play loop

### 14. UI System
- Player/computer card counters
- Battle result text display
- Ability message popups

---

## Technical Mechanics

### 15. Object Pooling
- Card instantiation/destruction
- Effect particle management
- Tag-based card tracking

### 16. Game Balancing
- Configurable starting hand size
- Adjustable ability values
- Elemental multiplier tuning

---

## Technologies

- **ShaderLab** — visual effects and materials
- **C#** — game logic, controllers, UI
- **HLSL** — custom shaders
- **Mathematica** — game data generation/processing

---

## How to Run

1. Open the project in Unity 6.
2. Start the `SampleScene`.
3. Press Play — the game will start automatically.

---

## Planned Improvements

- Add more rules and modes
- Implement multiplayer
- Improve visual effects, UI, and animations
- Add new abilities and elements

---

## Author

**ivanshevchuk0**

---

Questions or suggestions? Create an issue or pull request!
