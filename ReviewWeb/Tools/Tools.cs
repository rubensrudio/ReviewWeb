using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

public static class Tools
{
    public static Image ResizeImage(Image originalFile, int newWidth, int maxHeight, bool onlyResizeIfWider)
    {
        Image fullsizeImage = originalFile;

        // Prevent using images internal thumbnail
        fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
        fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);

        if (onlyResizeIfWider)
        {
            if (fullsizeImage.Width <= newWidth)
            {
                newWidth = fullsizeImage.Width;
            }
        }

        int newHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
        if (newHeight > maxHeight)
        {
            // Resize with height instead
            newWidth = fullsizeImage.Width * maxHeight / fullsizeImage.Height;
            newHeight = maxHeight;
        }

        Image newImage = fullsizeImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);

        return newImage;
    }
}
