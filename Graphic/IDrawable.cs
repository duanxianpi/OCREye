﻿namespace OCREye.Graphic
{
    using System.Windows.Media.Imaging;

    public interface IDrawable
    {
        void Draw(WriteableBitmap bitmap);
    }
}