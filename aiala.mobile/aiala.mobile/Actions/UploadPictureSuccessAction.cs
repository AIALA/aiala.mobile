using aiala.mobile.Models;
using Redux;

namespace aiala.mobile.Actions
{
    public class UploadPictureSuccessAction : IAction
    {
        public UploadPictureSuccessAction(Picture picture, string reference)
        {
            Picture = picture;
            Reference = reference;
        }

        public Picture Picture { get; }
        public string Reference { get; }
    }
}
