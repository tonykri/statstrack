using ReviewService.Dto;
using ReviewService.Models;
using ReviewService.Utils;

namespace ReviewService.Repositories;

public class ResponseRepo : IResponseRepo
{
    private readonly DataContext dataContext;
    private readonly ITokenDecoder tokenDecoder;
    public ResponseRepo(DataContext dataContext, ITokenDecoder tokenDecoder)
    {
        this.dataContext = dataContext;
        this.tokenDecoder = tokenDecoder;
    }

    public void PostResponse(CreateUpdateResponseDto response)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var business = dataContext.Businesses.FirstOrDefault(b => b.BusinessId == response.BusinessId);
            if(business is null)
                throw new NotFoundException("Business not found");
            if(business.UserId != userId)
                throw new NotAllowedException("Business does not belong to current user");
            var review = dataContext.Reviews.FirstOrDefault(r => r.Id == response.ReviewId);
            if(review is null)
                throw new NotFoundException("Review not found");
            if(review.Response is not null)
                throw new ReviewResExistsException("Response in current review exists");
            
            Response newResponse = new Response{
                ReviewId = review.Id,
                Review = review,
                Business = business,
                BusinessId = business.BusinessId,
                Content = response.Content
            };
            dataContext.Add(newResponse);
            dataContext.SaveChanges();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void UpdateReview(CreateUpdateResponseDto response)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var business = dataContext.Businesses.FirstOrDefault(b => b.BusinessId == response.BusinessId);
            if(business is null)
                throw new NotFoundException("Business not found");
            if(business.UserId != userId)
                throw new NotAllowedException("Business does not belong to current user");
            var review = dataContext.Reviews.FirstOrDefault(r => r.Id == response.ReviewId);
            if(review is null)
                throw new NotFoundException("Review not found");
            if(review.Response is null)
                throw new ReviewResExistsException("Response in current review does not exists");
            
            review.Response.Content = response.Content;
            review.Response.LastModified = DateTime.Now;
            dataContext.SaveChanges();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public void DeleteResponse(Guid reviewId)
    {
        try
        {
            Guid userId = tokenDecoder.GetUserId();
            var review = dataContext.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if(review is null)
                throw new NotFoundException("Review not found");
            if(review.Business.UserId != userId)
                throw new NotAllowedException("You are not allowed to delete this response");
            
            Response response = dataContext.Responses.First(r => r.ReviewId == reviewId);
            dataContext.Remove(response);
            dataContext.SaveChanges();
        }catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}