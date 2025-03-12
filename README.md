Cerebral Palsy Children's App Prototype
A prototype mobile application designed to assist children with cerebral palsy in cognitive and motor skill development.

![image](https://github.com/user-attachments/assets/0c9852fa-ae4f-4daf-8812-801b842b4768)




📌 About the Project
This project is a prototype of an interactive mobile application designed for children with cerebral palsy (CP). The goal is to create a fun and engaging experience that helps improve their motor coordination, cognitive skills, and visual perception through interactive shapes and animations.

The app is developed using .NET MAUI and follows the MVVM (Model-View-ViewModel) architecture for maintainability and scalability.

✨ Features
✅ Interactive Shape Matching Game – Children can move and match shapes to develop fine motor skills.
✅ Visual Feedback & Animations – Smooth animations for user engagement.
✅ Simple & Accessible UI – Designed with clarity and usability in mind.
✅ Automatic Shape Generation – Shapes appear at intervals to keep the game interactive.
✅ Game Over Detection – Ends when the user reaches the goal or the screen fills up.
✅ Touch & Gesture Controls – Optimized for drag-and-drop interactions.

📂 Project Structure

📦 CP_AppPrototype  
 ┃ ┣ 📂 Game  
 ┃ ┃ ┣ 📜 GameViewModel.cs  
 ┃ ┃ ┣ 📜 GameView.xaml   
 ┃ ┣ 📂 MainMenu  
 ┃ ┃ ┣ 📜 UserProfilesViewModel.cs  
 ┃ ┃ ┣ 📜 MainMenuView.xaml  
 ┃ ┃ ┗ 📜 MainMenuView.xaml.cs  
 ┃ ┣ 📂 SelectProfilesPage
 ┃ ┃ ┣ 📂 Converters
 ┃ ┃ ┃ ┗ 📜 EnumBooleanConverter.cs
 ┃ ┃ ┣ 📜 Gender.cs
 ┃ ┃ ┣ 📜 User.cs
 ┃ ┃ ┣ 📜 UserProfilesView.xaml
 ┃ ┃ ┣ 📜 UserProfilesView.xaml.cs
 ┃ ┃ ┣ 📜 UserProfilesViewModel.cs
 ┃ ┃ ┣ 📜 UserView.xaml
 ┃ ┃ ┣ 📜 UserView.xaml.cs
 ┃ ┃ ┗ 📜 UserViewModel.cs
 ┃ ┗ 📜 App.xaml
 ┗
📌 Key Files Explained

GameViewModel.cs – Manages game logic, shape generation, and game state.
GameView.xaml – UI layout for the game screen.
UserProfilesViewModel.cs – Handles user profile selection and management.
MainMenuView.xaml – The main entry point of the application.
