# Wardawn Prototype

Welcome to the Wardawn Prototype project! This project aims to develop a dynamic arena combat game where players can control a character, engage in battles with enemies, and collaborate with an AI companion, the Wardawn.

## Project Goals

- **Develop a controllable character** with movement, attack, dodge, and sprint capabilities.
- **Implement enemy AI** that can move, attack, and dodge.
- **Create a target switching system** based on the player's input.
- **Program the Wardawn AI** to fight autonomously in the background.
- **Establish health and stamina systems** for both the player character and enemies.
- **Implement invincibility frames (I-Frames)** for dodging to enhance combat mechanics.
- **Iterate towards a complex combat system**, eventually incorporating boss fights and advanced AI.

## Table of Contents

- [Project Goals](#project-goals)
- [Installation](#installation)
- [Usage](#usage)
- [Features](#features)
- [Development Workflow](#development-workflow)
- [To-Do List](#to-do-list)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgments](#acknowledgments)

## Installation

To get started with the Wardawn Prototype, follow these steps:

1. **Clone the repository:**

   ```bash
   git clone https://github.com/RBEW/WardawnPrototype.git
   ```

2. **Install Unity:**

   - Ensure you have Unity Hub installed. The recommended version for this project is Unity 2022.3.33f1 or later.
   - Open Unity Hub, select **Open**, and navigate to the cloned repository folder to open the project.

3. **Install Git LFS:**

   - Download and install [Git LFS](https://git-lfs.github.com/) to manage large files efficiently.
   - Run the following command in the project directory to initialize Git LFS:
   ```bash
   git lfs install
   ```
   

4. **Configure Unity Settings:**

   - Ensure all required Unity packages are installed and up to date.
   - Verify project settings, such as input and build settings, match the project's requirements.

## Usage

After setting up the project, you can explore and modify the existing features or add new ones:

1. **Run the Game:**

   - Open the Unity Editor.
   - Click **Play** to test the game in the Unity Editor.

2. **Control the Character:**

   - Use standard WASD or arrow keys for movement.
   - Attack using designated input keys.
   - Switch targets and engage with enemies.

3. **Develop Features:**

   - Open relevant scripts in your preferred code editor.
   - Modify and expand upon the existing codebase to add new functionalities.

## Features

### Core Features

- **Character Control:**
  - Basic movement (walking, sprinting, dodging).
  - Melee attacks and ranged abilities.
  - Health and stamina systems.

- **Enemy AI:**
  - Movement and attack patterns.
  - Target acquisition and dodging mechanics.

- **Target Switching:**
  - Analog stick-based target selection for precise combat engagement.

### Planned Features

- **Wardawn AI:**
  - Intelligent companion to assist in battles.
  - Autonomous combat abilities and coordination with the player.

- **Advanced Combat Mechanics:**
  - Invincibility frames during dodges for strategic depth.
  - Health systems for all entities, including Wardawn and bosses.

## Development Workflow

The development process follows a structured approach to ensure consistency and quality:

### Branching Strategy

- **`main` branch:** Stable, production-ready code.
- **`develop` branch:** Active development and integration of new features.
- **Feature branches:** Specific tasks or feature implementations.

### Task Management

- Use [Trello](https://trello.com/) for managing tasks and tracking progress.
- Update task statuses and priorities to reflect current development stages.

### Pull Requests

- Submit pull requests for code reviews before merging into the `develop` or `main` branches.
- Follow code review guidelines and address feedback promptly.

## To-Do List

Here's the current task list based on our Trello board:

### Backlog

- [ ] **Wardawn AI** - Program the companion AI to fight alongside the player.
- [ ] **Enemy AI** - Develop enemy behavior for combat and strategy.
- [ ] **Health for Wardawn** - Implement a health system for the AI companion.
- [ ] **Health for Enemy** - Add health indicators for enemy units.

### To Do

- [ ] **Character Attacks** - Program character attacks to hit enemy targets.
- [ ] **Moving Enemy** - Implement basic movement patterns for enemies.
- [ ] **Target Switching** - Enable target switching using the left analog direction.
- [ ] **Health System for Character** - Add a health system for the main character.
- [ ] **I-Frames for Dodging** - Implement invincibility frames during dodges.

### In Progress

- [x] **Controllable Character** - Develop character controls for movement and actions.
- [x] **Dodges and Sprint** - Implement dodging and sprinting mechanics.
- [x] **Stamina System** - Add a stamina system to manage energy for actions.

## Contributing

As a team member, you can contribute to the project by creating a new branch for your work directly in the repository. Follow these steps:

1. **Create a New Branch:**

   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make Your Changes:**

   - Implement the feature or fix the bug you're working on.
   - Ensure your code follows project conventions and is well-documented.

3. **Stage and Commit Your Changes:**

   ```bash
   git add .
   git commit -m "Describe your changes"
   ```

4. **Push Your Changes to the Repository:**

   ```bash
   git push origin feature/your-feature-name
   ```

5. **Create a Pull Request:**

   - Go to the GitHub repository page.
   - Navigate to the **Pull Requests** tab and click on **New Pull Request**.
   - Select your feature branch and submit a pull request for review.

6. **Review Process:**

   - Team members will review your code, provide feedback, and approve changes before merging.
   - Address any comments or requested changes promptly.

### Code of Conduct

- Be respectful and considerate in all interactions.
- Ensure code quality by following best practices and adhering to project guidelines.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Acknowledgments

- **Unity Community:** For providing resources and inspiration.
- **Contributors:** Thanks to all team members and collaborators who make this project possible.

---

### Commands for Running and Testing the Project

Here are some essential commands for working with the project:

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/RBEW/WardawnPrototype.git
   ```

2. **Checkout a Branch:**

   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Stage Changes:**

   ```bash
   git add .
   ```

4. **Commit Changes:**

   ```bash
   git commit -m "Describe your changes"
   ```

5. **Push Changes to GitHub:**

   ```bash
   git push origin feature/your-feature-name
   ```

6. **Pull Latest Changes:**

   ```bash
   git pull origin main
   ```

7. **Merge a Branch:**

   ```bash
   git checkout develop
   git merge feature/your-feature-name
   ```

8. **Resolve Merge Conflicts:**

   - Follow Git's instructions to resolve any conflicts.
   - Use `git status` to identify conflicts.
   - Edit conflicted files, then commit the resolved changes:

     ```bash
     git add .
     git commit -m "Resolved merge conflicts"
     ```

### Running the Game

1. **Open the Unity Project:**
   - Use Unity Hub to open the `WardawnPrototype` project.

2. **Play the Game:**
   - Click the **Play** button in the Unity Editor to start the game.

3. **Test Features:**
   - Use keyboard or controller input to test implemented features and mechanics.
