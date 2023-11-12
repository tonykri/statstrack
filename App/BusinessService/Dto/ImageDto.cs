namespace BusinessService.Dto;

public class ImageDto
{
    public ImageDto(byte[] PhotoData, string ContentType)
    {
        this.ContentType = ContentType;
        this.PhotoData = PhotoData;
    }

    public byte[] PhotoData { get; set; } 
    public string ContentType { get; set; }
}