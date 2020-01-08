using Redux;
using System.IO;

namespace aiala.mobile.Actions
{
    public class UploadPictureAction : IAction
    {
        public UploadPictureAction(byte[] binaryContent, string reference, string contentType)
        {
            BinaryContent = binaryContent;
            Reference = reference;
            ContentType = contentType;
        }

        public byte[] BinaryContent { get; }

        public string Reference { get; }

        public string ContentType { get; }
    }
}
