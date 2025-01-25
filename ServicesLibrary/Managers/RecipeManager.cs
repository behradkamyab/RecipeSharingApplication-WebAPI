using DataAccessLayerLibrary.Interfaces;
using ModelsLibrary.Models;
using ServicesLibrary.Mappers;
using ServicesLibrary.Interfaces;
using ServicesLibrary.Responses;
using SharedModelsLibrary.RecipeDto;
using DataAccessLayerLibrary.Queries;
using SharedModelsLibrary.RecipeDTOs;
using DataAccessLayerLibrary.Exceptions;
using SharedModelsLibrary.RecipeRequests;
using SharedModelsLibrary.IngredientRequests;
using SharedModelsLibrary.InstructionRequests;

namespace ServicesLibrary.Managers;

public class RecipeManager : IRecipeManager
{
    private readonly IRecipeRepository _recipeRepository;
 


    public RecipeManager(IRecipeRepository repo )
    {
        _recipeRepository = repo;

    }


    public async Task<RecipeManagerResponse<RecipeViewModel>> CreateRecipeAsync(string userId, CreateRecipeRequest recipe)     
    {
        try
        {

            var recipeModel = new RecipeModel(recipe.Name, recipe.Description, recipe.Category, recipe.Cuisine, userId);

            await _recipeRepository.AddRecipeAsync(recipeModel);

            if (recipe.Ingredients != null && recipe.Ingredients.Any())
            {
                foreach (var ing in recipe.Ingredients)
                {
                    await _recipeRepository.AddIngredientAsync(ing);
                }
            }

            if (recipe.Instructions != null && recipe.Instructions.Any())
            {
                foreach (var ins in recipe.Instructions)
                {
                    await _recipeRepository.AddInstructionAsync(ins);
                }
            }

            return new RecipeManagerResponse<RecipeViewModel>
            {
                Success = true,
                Message = "Recipe created successfully!",
                Data = recipeModel.AsRecipeViewModel()
            };
        }
        catch (DataAccessException)
        {

            return new RecipeManagerResponse<RecipeViewModel>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }
       
    }


    public async Task<RecipeManagerResponse<RecipeViewModel>> UpdateRecipeAsync(string userId, Guid recipeId,
        UpdateRecipeRequest recipeModel)
    {
        try
        {

            var recipe = await _recipeRepository.GetRecipeAsync(recipeId);
            if(recipe == null)
            {
                return new RecipeManagerResponse<RecipeViewModel>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };
            }


            if (recipe.UserId != userId)
                return new RecipeManagerResponse<RecipeViewModel>
                {
                    Success = false,
                    Message = "This recipe doesn't belong to the user",
                    Data = null
                };


            recipe.Name = recipeModel.Name ?? recipe.Name;
            recipe.Description = recipeModel.Description ?? recipe.Description;
            recipe.Cuisine = recipeModel.Cuisine ?? recipe.Cuisine;
            recipe.Category = recipeModel.Category ?? recipe.Category;

            await _recipeRepository.UpdateRecipeAsync(recipe);

            if (recipeModel.Ingredients != null && recipeModel.Ingredients.Any())
            {
                foreach (var ing in recipeModel.Ingredients)
                {
                    await _recipeRepository.UpdateIngredientAsync(ing);
                }
            }

            if (recipeModel.Instructions != null && recipeModel.Instructions.Any())
            {
                foreach (var ins in recipeModel.Instructions)
                {
                    await _recipeRepository.UpdateInstructionAsync(ins);
                }
            }
            return new RecipeManagerResponse<RecipeViewModel>
            {
                Success = true,
                Message = "Recipe updated successfully",
                Data = recipe.AsRecipeViewModel()
            };
        }
        catch (DataAccessException)
        {

            return new RecipeManagerResponse<RecipeViewModel>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }
       
    }


    public async Task<RecipeManagerResponse<string>> RemoveRecipeAsync(string userId, Guid recipeId)
    {
        try
        {

            var recipe = await _recipeRepository.GetRecipeAsync(recipeId);
            if (recipe == null)
            {
                return new RecipeManagerResponse<string>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };
            }


            if (recipe.UserId != userId)
                return new RecipeManagerResponse<string>
                {
                    Success = false,
                    Message = "This recipe doesn't belong to the user",
                    Data = null
                };

            await _recipeRepository.RemoveRecipeAsync(recipeId);

            return new RecipeManagerResponse<string>
            {
                Success = true,
                Message = "Recipe deleted!",
                Data = null
            };
        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<string>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };

        }
       
    }


    public async Task<RecipeManagerResponse<RecipeFullDetailsViewModel>> GetRecipeByIdAsync(Guid id)
    {
        try
        {
            var recipe = await _recipeRepository.GetFullDetailsRecipeAsync(id);
            if (recipe == null)
                return new RecipeManagerResponse<RecipeFullDetailsViewModel>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };

            return new RecipeManagerResponse<RecipeFullDetailsViewModel>
            {
                Success = true,
                Message = "Recipe fetched",
                Data = recipe.AsRecipeFullDetailsViewModel()
            };
        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<RecipeFullDetailsViewModel>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };

        }
        
    }

    public async Task<RecipeManagerResponse<IEnumerable<RecipeViewModel>>> GetAllRecipesFeedAsync(
       PageNumberQueryParameters query)
    {
        try
        {
            var recipes = await _recipeRepository.GetAllRecipesRandomlyAsync(query);

            if (recipes == null || !recipes.Any())
                return new RecipeManagerResponse<IEnumerable<RecipeViewModel>>
                {
                    Success = false,
                    Message = "no recipe found",
                    Data = null
                };
            var recipesViewModels = recipes.Select(r => r.AsRecipeViewModel());

            return new RecipeManagerResponse<IEnumerable<RecipeViewModel>>
            {
                Success = true,
                Message = "Recipes fetched",
                Data = recipesViewModels
            };
        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<IEnumerable<RecipeViewModel>>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }
          
    }


    public async Task<RecipeManagerResponse<IEnumerable<RecipeViewModel>>> GetAllFilterRecipesAsync(RecipeQueryParameters query)
    {

        try
        {
            var recipes = await _recipeRepository.GetAllFilterRecipesAsync(query);
            if (recipes == null || !recipes.Any())
                return new RecipeManagerResponse<IEnumerable<RecipeViewModel>>
                {
                    Success = false,
                    Message = "no recipe found",
                    Data = null
                };
            var recipesViewModels = recipes.Select(r => r.AsRecipeViewModel());
            return new RecipeManagerResponse<IEnumerable<RecipeViewModel>>
            {
                Success = true,
                Message = "Recipe fetched",
                Data = recipesViewModels
            };
        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<IEnumerable<RecipeViewModel>>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };

        }
    }

   

    public async Task<RecipeManagerResponse<IEnumerable<RecipeViewModel>>> GetAllRecipesCreatedByUserAsync(string userId)
    {

        try
        {
           
            var recipes = await _recipeRepository.GetAllRecipesCreatedByUser(userId);
            if (recipes == null || !recipes.Any())
            {
                return new RecipeManagerResponse<IEnumerable<RecipeViewModel>>
                {
                    Success = false,
                    Message = "recipe not found",
                    Data = null
                };
            }

            var recipesViewModels = recipes.Select(r => r.AsRecipeViewModel());

            return new RecipeManagerResponse<IEnumerable<RecipeViewModel>>
            {
                Success = true,
                Message = "Recipes fetched",
                Data = recipesViewModels
            };
        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<IEnumerable<RecipeViewModel>>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };

        }
       
    }
    public async Task<RecipeManagerResponse<Dictionary<string , int>>> GetAllNumbersOfRecipesCreatedByUserForOtherUsersAsync(List<string> userIds)
    {
        try
        {

                // Fetch the number of recipes created by each user in the list
                var recipeCounts = await _recipeRepository.GetAllNumbersOfRecipesCreatedByUserForOtherUsersAsync(userIds);

                return new RecipeManagerResponse<Dictionary<string, int>>()
                {
                    Success = true,
                    Message = "Recipe counts fetched successfully.",
                    Data = recipeCounts
                };   
         }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<Dictionary<string, int>>()
            {
                Success = false,
                Message = "Internal Server Error",

            };

        }
    }
    public async Task<RecipeManagerResponse<int>> GetAllNumbersOfRecipesCreatedByUserAsync(string userId)
    {
        try
        {

            var recipes = await _recipeRepository.GetAllRecipesCreatedByUser(userId);
            if (recipes == null || !recipes.Any())
            {
                return new RecipeManagerResponse<int>
                {
                    Success = false,
                    Message = "recipe not found",
                };
            }

            var counts = await _recipeRepository.GetAllNumbersOfRecipesCreatedByUserAsync(userId);

            return new RecipeManagerResponse<int>
            {
                Success = true,
                Message = "Recipes fetched",
                Data = counts
            };
        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<int>()
            {
                Success = false,
                Message = "Internal Server Error",

            };

        }
    }

    public async Task<RecipeManagerResponse<IngredientRecipe>> CreateIngredientAsync(CreateIngredientRequest ingredientRequest, Guid recipeId,
        string userId)
    {
        try
        {

            var recipe = await _recipeRepository.GetRecipeAsync(recipeId);
            if (recipe == null)
            {
                return new RecipeManagerResponse<IngredientRecipe>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };
            }


            if (recipe.UserId != userId)
                return new RecipeManagerResponse<IngredientRecipe>
                {
                    Success = false,
                    Message = "This recipe doesn't belong to the user",
                    Data = null
                };

            var ingredient = new IngredientRecipe
            {
                Id = Guid.NewGuid(),
                Name = ingredientRequest.Name,
                Quantity = ingredientRequest.Quantity,
                RecipeId = recipeId
            };
            await _recipeRepository.AddIngredientAsync(ingredient);
            return new RecipeManagerResponse<IngredientRecipe>
            {
                Success = true,
                Message = "Ingredient created successfully",
                Data = ingredient
            };

        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<IngredientRecipe>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }
    }



    public async Task<RecipeManagerResponse<string>> RemoveIngredientAsync(Guid ingredientId,Guid recipeId,
        string userId)
    {

        try
        {

            var recipe = await _recipeRepository.GetRecipeAsync(recipeId);
            if (recipe == null)
            {
                return new RecipeManagerResponse<string>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };
            }


            if (recipe.UserId != userId)
                return new RecipeManagerResponse<string>
                {
                    Success = false,
                    Message = "This recipe doesn't belong to the user",
                    Data = null
                };
            if (!await _recipeRepository.IsIngredientAvailable(ingredientId , recipeId))
                return new RecipeManagerResponse<string>
                {
                    Success = false,
                    Message = "Ingredient not found!",
                    Data = null
                };
            await _recipeRepository.RemoveIngredientAsync(ingredientId);
            return new RecipeManagerResponse<string>
            {
                Success = true,
                Message = "Ingredient deleted",
                Data = null
            };

        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<string>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }

    }


    public async Task<RecipeManagerResponse<IngredientRecipe>> UpdateIngredientAsync(UpdateIngredientRequest ingredientModel,
        Guid recipeId,
        string userId,
        Guid ingredientId)
    {
        try
        {

            var recipe = await _recipeRepository.GetRecipeAsync(recipeId);
            if (recipe == null)
            {
                return new RecipeManagerResponse<IngredientRecipe>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };
            }

            if (recipe.UserId != userId)
                return new RecipeManagerResponse<IngredientRecipe>
                {
                    Success = false,
                    Message = "This recipe doesn't belong to the user",
                    Data = null
                };
            if (!await _recipeRepository.IsIngredientAvailable(ingredientId , recipeId))
                return new RecipeManagerResponse<IngredientRecipe>
                {
                    Success = false,
                    Message = "Ingredient not found!",
                    Data = null
                };

            var ingredient = await _recipeRepository.GetIngredientByIdAsync(ingredientId);
           

            if (!string.IsNullOrEmpty(ingredientModel.Name))
            {
                ingredient.Name = ingredientModel.Name;
            }
            if (ingredientModel.Quantity != null)
            {
                ingredient.Quantity = (int)ingredientModel.Quantity;
            }


            await _recipeRepository.UpdateIngredientAsync(ingredient);
            return new RecipeManagerResponse<IngredientRecipe>
            {
                Success = true,
                Message = "Ingredient updated!",
                Data = ingredient
            };

        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<IngredientRecipe>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }
    }



    public async Task<RecipeManagerResponse<IngredientRecipe>> GetIngredientByIdAsync(Guid ingredientId, Guid recipeId)
    {
        try
        {
            if (!await _recipeRepository.IsRecipeAvailable(recipeId))
                return new RecipeManagerResponse<IngredientRecipe>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };
            if (!await _recipeRepository.IsIngredientAvailable(ingredientId, recipeId))
                return new RecipeManagerResponse<IngredientRecipe>
                {
                    Success = false,
                    Message = "Ingredient not found!",
                    Data = null
                };

            var ingredient = await _recipeRepository.GetIngredientByIdAsync(ingredientId);
            return new RecipeManagerResponse<IngredientRecipe>()
            {
                Success = true,
                Message = "Ingredient of the selected recipe, fetched!",
                Data = ingredient
            };
        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<IngredientRecipe>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }
    }


    public async Task<RecipeManagerResponse<IEnumerable<IngredientRecipe>>> GetAllIngredientsOfRecipeAsync(Guid recipeId)
    {
        try
        {
            if (!await _recipeRepository.IsRecipeAvailable(recipeId))
                return new RecipeManagerResponse<IEnumerable<IngredientRecipe>>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };

            var ingredients = await _recipeRepository.GetAllIgredientsOfRecipe(recipeId);
            if(ingredients == null)
            {
                return new RecipeManagerResponse<IEnumerable<IngredientRecipe>>()
                {
                    Success = false,
                    Message = "No ingredient found for this recipe",
                    Data = null
                };
            }

            return new RecipeManagerResponse<IEnumerable<IngredientRecipe>>()
            {
                Success = true,
                Message = "All of the ingredients of selected recipe, fetched!",
                Data = ingredients
            };
        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<IEnumerable<IngredientRecipe>>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }

    }


    public async Task<RecipeManagerResponse<InstructionRecipe>> CreateInstructionAsync(Guid recipeId, string content, string userId)
    {
        try
        {

            var recipe = await _recipeRepository.GetRecipeAsync(recipeId);
            if (recipe == null)
            {
                return new RecipeManagerResponse<InstructionRecipe>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };
            }

            if (recipe.UserId != userId)
                return new RecipeManagerResponse<InstructionRecipe>
                {
                    Success = false,
                    Message = "This recipe doesn't belong to the user",
                    Data = null
                };

            var instruction = new InstructionRecipe()
            {
                Id = Guid.NewGuid(),
                Step = await _recipeRepository.GetLastInstructionStep(recipeId) + 1,
                Content = content,
                RecipeId = recipeId,
            };



            await _recipeRepository.AddInstructionAsync(instruction);
            return new RecipeManagerResponse<InstructionRecipe>
            {
                Success = true,
                Message = "Instruction created successfully",
                Data = instruction
            };


        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<InstructionRecipe>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }
    }


    public async Task<RecipeManagerResponse<string>> RemoveInstructionAsync(Guid instructionId, Guid recipeId,string userId)
    {
        try
        {

            var recipe = await _recipeRepository.GetRecipeAsync(recipeId);
            if (recipe == null)
            {
                return new RecipeManagerResponse<string>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };
            }


            if (recipe.UserId != userId)
                return new RecipeManagerResponse<string>
                {
                    Success = false,
                    Message = "This recipe doesn't belong to the user",
                    Data = null
                };


            var instruction = await _recipeRepository.GetInstructionByIdAsync(instructionId);

            if (instruction == null || instruction.RecipeId != recipeId)
                return new RecipeManagerResponse<string>
                {
                    Success = false,
                    Message = "Instruction not found!",
                    Data = null
                };
            if (instruction.Step == await _recipeRepository.GetLastInstructionStep(recipeId))
            {

                await _recipeRepository.RemoveInstructionAsync(instructionId);
            }
            else
            {
                var instructionsToUpdate = await _recipeRepository.FindInstructionAsync(i => i.Step > instruction.Step);


                foreach (var ins in instructionsToUpdate)
                {
                    ins.Step -= 1;
                }


                await _recipeRepository.RemoveInstructionAsync(instructionId);
            }

            return new RecipeManagerResponse<string>
            {
                Success = true,
                Message = "Instruction deleted",
                Data = null
            };

        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<string>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }
    }


    public async Task<RecipeManagerResponse<InstructionRecipe>> UpdateInstructionAsync(UpdateInstructionRequest instructionModel,Guid recipeId,string userId)
    {

        try
        {
           
            var recipe = await _recipeRepository.GetRecipeAsync(recipeId);
            if (recipe == null)
            {
                return new RecipeManagerResponse<InstructionRecipe>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };
            }


            if (recipe.UserId != userId)
                return new RecipeManagerResponse<InstructionRecipe>
                {
                    Success = false,
                    Message = "This recipe doesn't belong to the user",
                    Data = null
                };


            var instruction = await _recipeRepository.GetInstructionByIdAsync(instructionModel.InstructionId);

            if (instruction == null || instruction.RecipeId != recipeId)
                return new RecipeManagerResponse<InstructionRecipe>
                {
                    Success = false,
                    Message = "Instruction not found!",
                    Data = null
                };

     
              instruction.Content = instructionModel.Content;
            await _recipeRepository.UpdateInstructionAsync(instruction);
            return new RecipeManagerResponse<InstructionRecipe>()
            {
                Success = true,
                Message = "Instruction Updated",
                Data = instruction
            };

        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<InstructionRecipe>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }

    }

    public async Task<RecipeManagerResponse<string>> AddLikeToRecipeAsync(string userId, Guid recipeId)
    {
        try
        {
            if (!await _recipeRepository.IsRecipeAvailable(recipeId))
            {
                return new RecipeManagerResponse<string>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };
            }

            var like = new Like()
            {
                UserId = userId,
                RecipeId = recipeId,
                LikedAt = DateTime.UtcNow
            };

            await _recipeRepository.AddLikeToRecipeAsync(like);
            return new RecipeManagerResponse<string>()
            {
                Success = true,
                Message = "You liked the recipe"
            };
        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<string>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }
    }

    public async Task<RecipeManagerResponse<string>> RemoveLikeFromRecipeAsync(string userId, Guid recipeId)
    {
        try
        {
            if (!await _recipeRepository.IsRecipeAvailable(recipeId))
            {
                return new RecipeManagerResponse<string>
                {
                    Success = false,
                    Message = "Recipe not found!",
                    Data = null
                };
            }
            var like = await _recipeRepository.GetLikeForRecipeAsync(userId, recipeId);
            if(like == null)
            {
                return new RecipeManagerResponse<string>()
                {
                    Success = false,
                    Message = "You don't like this recipe!"
                };
            }

            await _recipeRepository.RemoveLikeFromRecipeAsync(like);
            return new RecipeManagerResponse<string>()
            {
                Success = true,
                Message = "Like removed from this recipe"
            };
        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<string>()
            {
                Success = false,
                Message = "Internal Server Error",
                Data = null
            };
        }
    }

    public async Task<RecipeManagerResponse<int>> GetAllLikesAsync(Guid recipeId)
    {
        try
        {
            var likeCounts = await _recipeRepository.GetAllLikesForRecipeAsync(recipeId);
            return new RecipeManagerResponse<int>()
            {
                Success = true,
                Message = "number of likes for this recipe fetched",
                Data = likeCounts
            };
        }
        catch (DataAccessException)
        {
            return new RecipeManagerResponse<int>()
            {
                Success = false,
                Message = "Internal Server Error",
            };
        }
    }
}
