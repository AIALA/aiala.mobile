using aiala.mobile.Models;
using Redux;
using System.Collections.Generic;

namespace aiala.mobile.Actions
{
    public class LoadGallerySuccessAction : IAction
    {
        public LoadGallerySuccessAction(List<Picture> result)
        {
            Result = result;
        }

        public List<Picture> Result { get; }
    }
}
