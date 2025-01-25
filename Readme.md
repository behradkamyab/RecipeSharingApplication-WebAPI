# Recipe Sharing Application - WebAPI

![License](https://img.shields.io/badge/license-MIT-blue.svg)

A RESTful Web API for sharing and managing recipes. This application allows users to create, read, update, and delete recipes, as well as search for recipes based on various criteria. It is built with scalability and maintainability in mind, leveraging modern technologies and design patterns.

## Table of Contents

- [Technologies Used](#technologies-used)
- [Features](#features)
- [Design Patterns](#design-patterns)
- [Installation](#installation)
- [Usage](#usage)

---

## Technologies Used

- **Backend Framework**: ASP.NET Core
- **Database**: SQL Server (with Entity Framework Core for ORM)
- **Authentication**: JWT (JSON Web Tokens) - ASP.NET Identity
- **Dependency Injection**: Built-in ASP.NET Core DI Container
- **Caching**: In-Memory Caching
- **Testing**: xUnit (for unit testing)
- **Version Control**: Git
- **CI/CD**: GitHub Actions (optional)
- **FluentValidation**: Validation of incoming requests.

---

## Features

- **User Authentication**: Secure user registration and login using JWT.
- **Recipe Management**:
  - Create, read, update, and delete recipes.
  - Search recipes by name, ingredients, or tags.
- **Pagination**: Support for paginated responses for large datasets.
- **Validation**: Input validation for all API requests.
- **Real-Time Notifications**: Notify users in real time when their recipes are liked or when they are followed by other users.
- **Follow System**: Users can follow each other to stay updated on new recipes.
- **Like System**: Users can like recipes to show appreciation.
- **Modular Architecture**: The application is divided into multiple projects (Web API, Data Access Library, Service Library) for better maintainability.
---

## Design Patterns

- **Repository Pattern**: Used to abstract data access logic and promote separation of concerns.
- **Dependency Injection**: Leveraged for loose coupling and testability.
- **DTOs (Data Transfer Objects)**: Used to transfer data between layers while reducing unnecessary data exposure.
---

## Installation

Follow these steps to set up the project locally:

1. **Clone the repository**:
   ```bash
   git clone https://github.com/behradkamyab/RecipeSharingApplication-WebAPI.git
   cd RecipeSharingApplication-WebAPI
---
## API Endpoints

| Method | Endpoint                                                    | Description                                       |
|--------|-------------------------------------------------------------|---------------------------------------------------|
| POST   | `/api/auth/register`                                        | Register a new user.                              |
| POST   | `/api/auth/login`                                           | Log in and receive a JWT token.                   |
| POST   | `/api/recipe/create`                                        | Create new recipe                                 |
| DELETE | `/api/recipe/delete/{recipeId}`                             | delete a recipe by ID.                            |
| PUT    | `/api/recipe/update/{recipeId}`                             | Update an existing recipe.                        |
| GET    | `/api/recipe/{recipeId}`                                    | Get an existing recipe.                           |
| GET    | `/api/recipe/all/feed?PageNumber=""&PageSize=""`            | Get All recipes (paginated)                       |
| GET    | `/api/recipe/all/filter?name=""?category=""?cuisine=""`     | Search recipes by name or ingredients.            |
| GET    | `/api/recipe/all/created-by-user/`                          | Get all recipes created by user                   |
| POST   | `/api/recipe/{recipeId}/ingredient/create`                  | create ingredient for a recipe                    |
| DELETE | `/api/recipe/{recipeId}/ingredient/remove/{ingredientId}`   | Delete ingredient for a recipe                    |
| PUT    | `/api/recipe/{recipeId}/ingredient/update/{ingredientId}`   | Update ingredient for a recipe                    |
| GET    | `/api/recipe/{recipeId}/ingredient/{ingredientId}`          | Get ingredient for a recipe                       |
| GET    | `/api/recipe/{recipeId}/ingredients`                        | Get all ingredients for a recipe                  |
| POST   | `/api/recipe/{recipeId}/instruction/create`                 | Create instruction for a recipe                   |
| DELETE | `/api/recipe/{recipeId}/instruction/remove/{instructionId}` | Delete instruction for a recipe                   |
| PUT    | `/api/recipe/{recipeId}/instruction/update`                 | Update instruction for a recipe                   |
| POST   | `/api/recipe/{recipeId}/like`                               | Like a recipe                                     |
| DELETE | `/api/recipe/{recipeId}/like`                               | Delete Like for a recipe                          |
| GET    | `/api/recipe/{recipeId}/likes`                              | Get all numbers of likes for a recipe             |
| GET    | `/api/user/profile `                                        | Get user profile                                  |
| POST   | `/api/user/profile/bio`                                     | Change user bio                                   |
| POST   | `/api/user/favorites/add/{recipeId}`                        | Add a recipe to user favorites list               |
| DELETE | `/api/user/favorites/remove/{recipeId}`                     | Remove a recipe to user favorites list            |
| GET    | `/api/user/favorites/`                                      | Get all user favorite recipes                     |
| GET    | `/api/user/favorites/{recipeId}`                            | Get one user favorite recipes                     |
| POST   | `/api/user/follow`                                          | Follow a user                                     |
| POST   | `/api/user/unfollow`                                        | Unfollow a user                                   |
| GET    | `/api/User/followers?numbers=true`                          | Get all followers for a user (number of followers)|
| GET    | `/api/User/followings?numbers=true`                         | Get followings for a user (number of followings) |

