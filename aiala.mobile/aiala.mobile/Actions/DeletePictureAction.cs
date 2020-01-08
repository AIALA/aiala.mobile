using Redux;
using System;
using System.Collections.Generic;
using System.Text;

namespace aiala.mobile.Actions
{
    public class DeletePictureAction : IAction
    {
        public DeletePictureAction(Guid pictureId)
        {
            PictureId = pictureId;
        }

        public Guid PictureId { get; }
    }
}
