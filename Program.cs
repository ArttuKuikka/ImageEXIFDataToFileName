// See https://aka.ms/new-console-template for more information
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Directory = System.IO.Directory;

Random rnd = new Random();

Console.WriteLine("Hello, World!");

Console.WriteLine("Input directory:");
var inDirectory = Console.ReadLine()?.Replace('"', char.MinValue);

Console.WriteLine("Output directory:");
var outDirectory = Console.ReadLine()?.Replace('"', char.MinValue);

if(inDirectory == null || !Directory.Exists(inDirectory))
{
    throw new FileNotFoundException(inDirectory);
}

if (outDirectory == null || !Directory.Exists(outDirectory))
{
    throw new FileNotFoundException(inDirectory);
}


string[] extensions = {".png", ".jpg"};

var files = Directory.GetFiles(inDirectory);
var imageFiles = new List<string>();

foreach(var extension in extensions)
{
    imageFiles.AddRange(files.Where(x => x.EndsWith(extension)));
}

foreach(var imageFile in imageFiles)
{
    var meta = ImageMetadataReader.ReadMetadata(imageFile);

    var fileDateTime = DateTime.MinValue;

    var subIfdDirectory = meta.OfType<ExifSubIfdDirectory>().FirstOrDefault();
    var successGettingDate = subIfdDirectory?.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out fileDateTime);

    if(!(successGettingDate ?? false))
    {
        fileDateTime = File.GetLastWriteTime(imageFile);
    }

    var ext = Path.GetExtension(imageFile);

    var randomFileNameAddOn = rnd.Next(0, 1000);

    var destFileName = fileDateTime.ToString("yyyyMMdd_HH-mm") + randomFileNameAddOn.ToString() + ext;
    if (!(successGettingDate ?? false))
    {
        destFileName = "NOEXIF-" + destFileName;
    }
    var fullDestPath = Path.Combine(outDirectory, destFileName);

    var inputFilePath = Path.Combine(inDirectory, imageFile);

    File.Copy(inputFilePath, fullDestPath);
    Console.WriteLine($"Copied {imageFile} to {destFileName}");

}
Console.WriteLine("Goodbye!!");