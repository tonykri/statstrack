namespace BusinessService.Dto;

public class ImageDto
{
    public ImageDto(Stream PhotoData, string ContentType, string FileName)
    {
        this.ContentType = ContentType;
        this.PhotoData = PhotoData;
        this.FileName = FileName;
    }

    public Stream PhotoData { get; set; } 
    public string ContentType { get; set; }
    public string FileName { get; set; }
}