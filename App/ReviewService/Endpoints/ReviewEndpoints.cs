using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Dto;
using ReviewService.Repositories;

namespace ReviewService.Endpoints;

public class ReviewEndpoints : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("{businessId:guid}", GetBusinessReviews)
            .RequireAuthorization("completed_profile");
        app.MapPost("", PostReview)
            .RequireAuthorization("completed_profile");
        app.MapPut("", UpdateReview)
            .RequireAuthorization("completed_profile");
        app.MapDelete("{reviewId:guid}", DeleteReview)
            .RequireAuthorization("completed_profile");
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IReviewRepo, ReviewRepo>();
    }

    private IResult GetBusinessReviews([FromServices] IReviewRepo reviewRepo, [FromRoute] Guid businessId)
    {
        try
        {
            List<ReviewDto> reviews = reviewRepo.GetBusinessReviews(businessId);
            return Results.Ok(reviews);
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }
    }

    private IResult PostReview([FromServices] IReviewRepo reviewRepo, [FromBody] CreateReviewDto review)
    {
        try
        {
            reviewRepo.PostReview(review);
            return Results.NoContent();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(ReviewResExistsException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private IResult UpdateReview([FromServices] IReviewRepo reviewRepo, [FromBody] UpdateReviewDto review)
    {
        try
        {
            reviewRepo.UpdateReview(review);
            return Results.NoContent();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(NotAllowedException ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Forbid();
        }
    }

    private IResult DeleteReview([FromServices] IReviewRepo reviewRepo, [FromRoute] Guid reviewId)
    {
        try
        {
            reviewRepo.DeleteReview(reviewId);
            return Results.NoContent();
        }catch(NotFoundException ex)
        {
            return Results.NotFound(ex.Message);
        }catch(NotAllowedException ex)
        {
            Console.WriteLine(ex.Message);
            return Results.Forbid();
        }
    }
}