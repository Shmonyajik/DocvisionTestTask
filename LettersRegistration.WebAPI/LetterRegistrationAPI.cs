using FluentValidation;
using FluentValidation.Results;
using LettersRegistration.WebAPI.DAL;
using LettersRegistration.WebAPI.Domain;
using Serilog;
using System;

namespace LettersRegistration.WebAPI
{
    public static class LetterRegistrationAPI
    {
        private static readonly Serilog.ILogger logger = Log.Logger.ForContext<Letter>();
        public static void ConfigureApi(this WebApplication app)
        {
            app.MapGet("/letters",  async (IBaseRepository<Letter> letterRepository)
                => await GetAllLetters(letterRepository))
                .Produces<IEnumerable<Letter>>(StatusCodes.Status200OK)
                .WithName("GetAllLetters")
                .WithTags("Getters");

            app.MapGet("/letters/{id:int}", async (IBaseRepository<Letter> letterRepository, int id) =>
                 await GetLetterById(letterRepository, id))
                .Produces<Letter>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetLetterById")
                .WithTags("Getters");

            app.MapGet("/letters/{sender}", async (IBaseRepository<Letter> letterRepository, string sender) =>
                 await GetLetterBySender(letterRepository, sender))
                .Produces<IEnumerable<Letter>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetLetterBySender")
                .WithTags("Getters");

            app.MapPost("/letters", async ([FromServices] IBaseRepository<Letter> letterRepository,
                [FromServices] IValidator<Letter> validator,
                [FromBody] Letter letter) =>
                await CreateLetter(letterRepository, validator, letter))
                .Accepts<Letter>("application/json")
                .Produces<Letter>(StatusCodes.Status201Created)
                .WithName("CreateLetter")
                .WithTags("Creators");
        }

        public static async Task<IResult> GetAllLetters(IBaseRepository<Letter> letterRepository)
        {
            try
            {
                return Results.Ok(await letterRepository.GetAll());
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Results.Problem(ex.Message);
            }
            
        }

        public static async Task<IResult> GetLetterById(IBaseRepository<Letter> letterRepository, int id)
        {
            try
            {
                return await letterRepository.GetById(id) is Letter letter
                        ? Results.Ok(letter)
                        : Results.NotFound();
                
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Results.Problem(ex.Message);
            }
        }
        public static async Task<IResult> GetLetterBySender(IBaseRepository<Letter> letterRepository, string sender)
        {
            try
            {
                var letters = await letterRepository.GetBySender(sender); 
                if(letters.Count()>0)
                {
                    return Results.Ok(letters);
                }
                return Results.NoContent();
                        

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return Results.Problem(ex.Message);
            }
        }

        public static async Task<IResult> CreateLetter(IBaseRepository<Letter> letterRepository,
            IValidator<Letter> validator,
            Letter letter)
        {
            ValidationResult validationResult = await validator.ValidateAsync(letter);
            if(validationResult.IsValid)
            {
                letter.Id = 0;
                try
                {
                    await letterRepository.Create(letter);
                    //throw new ArgumentNullException();
                    return Results.Created($"/letters/{letter.Id}", letter);
                    

                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    
                    return Results.Problem(ex.Message);
                }
            }
            logger.Error(validationResult.ToString());
            return Results.ValidationProblem(validationResult.ToDictionary());
            
        }



    }
}
