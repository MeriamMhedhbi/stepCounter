# Step Counter App

## Overview
The **Step Counter App** is a mobile application built using Unity that tracks user steps, calculates the distance walked, and estimates the number of calories burned. It stores daily step-related data in a local SQLite database and allows users to review past data. The app uses the device's accelerometer for step tracking and GPS to improve distance accuracy.

## Features
- **Step Tracking**: Uses the device's accelerometer to count steps.
- **Distance Calculation**: Combines step count and GPS-based calculations for accurate distance tracking.
- **Calorie Calculation**: Estimates the calories burned based on user profile and activity.
- **User Profile**: Allows users to input gender, height, and weight for personalized calorie calculations.
- **Data History**: Displays the last 3 days of activity data, including steps, distance, and calories burned.
- **SQLite Database**: Stores user data locally, including steps, distance, calories, and profile information.

## Technologies
- **Unity**: Main engine for building the app.
- **SQLite**: For local storage of user data.

## Setup Instructions

**Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/step-counter-app.git ```
