using Config.Stracture;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Dto;
using ReviewService.Repositories;
using ReviewService.Services;

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
        services.AddScoped<IReviewsRepo, ReviewsRepo>();
        services.AddScoped<IReviewsService, ReviewsService>();
    }

    private async Task<IResult> GetBusinessReviews([FromServices] IReviewsRepo reviewsRepo, [FromRoute] Guid businessId)
    {
        var result = await reviewsRepo.GetBusinessReviews(businessId);
        return result.Match<IResult>(
            data => Results.Ok(result.Data),
            exception => Results.NotFound(exception?.Message)
        );
    }

    private async Task<IResult> PostReview([FromServices] IReviewsService reviewsService, [FromBody] CreateReviewDto review)
    {
        var result = await reviewsService.PostReview(review);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception =>
            {
                if (exception?.Message == ExceptionMessages.REVIEW_EXISTS)
                    return Results.BadRequest(exception?.Message);
                else
                    return Results.NotFound(exception?.Message);
            }
        );
    }

    private async Task<IResult> UpdateReview([FromServices] IReviewsService reviewsService, [FromBody] UpdateReviewDto review)
    {
        var result = await reviewsService.UpdateReview(review);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception =>
            {
                if (exception?.Message == ExceptionMessages.UNAUTHORIZED)
                    return Results.Forbid();
                else
                    return Results.NotFound(exception?.Message);
            }
        );
    }

    private async Task<IResult> DeleteReview([FromServices] IReviewsService reviewsService, [FromRoute] Guid reviewId)
    {
        var result = await reviewsService.DeleteReview(reviewId);
        return result.Match<IResult>(
            data => Results.NoContent(),
            exception =>
            {
                if (exception?.Message == ExceptionMessages.UNAUTHORIZED)
                    return Results.Forbid();
                else
                    return Results.NotFound(exception?.Message);
            }
        );
    }
}