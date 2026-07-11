using Microsoft.AspNetCore.Http;

namespace CommomTestUtilities.Requests
{
    public class FormFileBuilder
    {
        public static IFormFile Png()
        {
            var stream = File.OpenRead("Files/FilePng.png");

            return new FormFile(
                baseStream: stream,
                baseStreamOffset: 0,
                length: stream.Length,
                name: "File",
                fileName: "IMG0001.png")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };
        }

        public static IFormFile Jpg()
        {
            var stream = File.OpenRead("Files/FileJpg.jpg");

            return new FormFile(
                baseStream: stream,
                baseStreamOffset: 0,
                length: stream.Length,
                name: "File",
                fileName: "IMG0001.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };
        }

        public static IFormFile Txt()
        {
            var stream = File.OpenRead("Files/FileTxt.txt");

            return new FormFile(
                baseStream: stream,
                baseStreamOffset: 0,
                length: stream.Length,
                name: "File",
                fileName: "FILE001.txt")
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };
        }

        public static IList<IFormFile> ImageCollection() => [Png(), Jpg()];
    }
}
