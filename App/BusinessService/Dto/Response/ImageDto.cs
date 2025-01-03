namespace BusinessService.Dto;

public class ImageDto
{
    public ImageDto(byte[] PhotoData, string ContentType, string FileName)
    {
        this.ContentType = ContentType;
        this.PhotoData = PhotoData;
        this.FileName = FileName;
    }

    public byte[] PhotoData { get; set; } 
    public string ContentType { get; set; }
    public string FileName { get; set; }
}