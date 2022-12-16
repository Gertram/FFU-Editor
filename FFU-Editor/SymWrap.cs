using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using FFULibrary;

namespace FFU_Editor
{
    internal class SymWrap
    {
        internal Sym Sym { get; set; }
        internal SymWrap(int width, int height) : this(new byte[width, height]) { }
        internal SymWrap(byte[,] image) : this(new Sym(image)) { }
        internal SymWrap(Sym sym)
        {
            Sym = sym;
        }
        internal Bitmap GetSymBitmap()
        {
            var bitmap = new Bitmap(Sym.Width, Sym.Height);

            for (var y = 0; y < Sym.Height; y++)
            {
                for (var x = 0; x < Sym.Width; x++)
                {
                    var val = Sym.Image[x, y]/**35*/;
                    bitmap.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }
            return bitmap;
        }

        internal void SetPadding(int padiing)
        {
            SetPadding(padiing, padiing);
        }
        internal void SetPadding(int left, int top, int right, int bottom)
        {
            var topPosition = FindTopPosition();

            var bottomPosition = FindBottomPosition();

            var leftPosition = FindLeftPosition();

            var rightPosition = FindRightPosition();

            var location = new Point(leftPosition, topPosition);

            var size = new Size(rightPosition - location.X + 1, bottomPosition - location.Y + 1);

            var region = new Size(size.Width + left + right, size.Height + top + bottom);

            CopyImage(region, new Point(left, top), new Rectangle(location, size));
        }
        internal SymWrap CopyImage(Size region, Point position, Rectangle source)
        {
            var dest = new Rectangle(position, source.Size);
            var temp = new byte[region.Width, region.Height];
            for (var y = 0; y < dest.Height; y++)
            {
                for (var x = 0; x < dest.Width; x++)
                {
                    if (dest.Left + x > region.Width || dest.Top + y > region.Height)
                    {
                        Console.WriteLine();
                    }
                    if (source.Left + x > source.Width || source.Top + y > source.Height)
                    {
                        Console.WriteLine();
                    }
                    temp[dest.Left + x, dest.Top + y] = Sym.Image[source.Left + x, source.Top + y];

                }
            }
            Sym.Image = temp;
            return this;
        }
        internal void SetHorizontalPadding(int left, int right)
        {
            var leftPosition = FindLeftPosition();

            var rightPosition = FindRightPosition();

            var location = new Point(leftPosition, 0);

            var size = new Size(rightPosition - location.X + 1, Sym.Height);

            var region = new Size(size.Width + left + right, Sym.Height);

            CopyImage(region, new Point(left, 0), new Rectangle(location, size));
        }
        internal void SetVerticalPadding(int top, int bottom)
        {
            var topPosition = FindTopPosition();

            var bottomPosition = FindBottomPosition();

            var location = new Point(0, topPosition);

            var size = new Size(Sym.Width, bottomPosition - location.Y + 1);

            var region = new Size(Sym.Width, size.Height + top + bottom);

            CopyImage(region, new Point(0, top), new Rectangle(location, size));
        }
        internal void SetPadding(int horizontal, int vertial)
        {
            SetPadding(horizontal, vertial, horizontal, vertial);
        }
        internal SymWrap AddPadding(int left, int top, int right, int bottom)
        {
            return CopyImage(new Size(left + right + Sym.Width, Sym.Height + top + bottom), new Point(left, top),
                new Rectangle(0, 0, Sym.Width, Sym.Height));
        }
        internal void AddPadding(int horizontal,int vertical)
        {
            AddPadding(horizontal, vertical, horizontal, vertical);
        }
        internal void AddPadding(int padding)
        {
            AddPadding(padding,padding);
        }
        internal void RemovePadding(int left,int top,int right,int bottom)
        {
            var size = new Size(Sym.Width - left - right, Sym.Height - top - bottom);
            CopyImage(size, new Point(0, 0), new Rectangle(new Point(left, top), size));
        }
        internal void RemovePadding(int horizontal,int vertical)
        {
            RemovePadding(horizontal, vertical, horizontal, vertical);
        }
        internal void RemovePadding(int padding)
        {
            RemovePadding(padding, padding);
        }
        internal int FindBottomPosition()
        {
            for (int y = Sym.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < Sym.Width; x++)
                {
                    if (Sym.Image[x, y] != 0)
                    {
                        return y;
                    }
                }
            }

            return -1;
        }
        internal int FindTopPosition()
        {
            for (int y = 0; y < Sym.Height; y++)
            {
                for (int x = 0; x < Sym.Width; x++)
                {
                    if (Sym.Image[x, y] != 0)
                    {
                        return y;
                    }
                }
            }

            return -1;
        }
        internal int FindLeftPosition()
        {
            for (int x = 0; x < Sym.Width; x++)
            {
                for (int y = 0; y < Sym.Height; y++)
                {
                    if (Sym.Image[x, y] != 0)
                    {
                        return x;
                    }
                }
            }
            return -1;
        }
        internal int FindRightPosition()
        {
            for (int x = Sym.Width - 1; x >= 0; x--)
            {
                for (int y = 0; y < Sym.Height; y++)
                {
                    if (Sym.Image[x, y] != 0)
                    {
                        return x;
                    }
                }
            }
            return -1;
        }
        internal int LeftPadding() => FindLeftPosition();
        internal int RightPadding() => Sym.Width - FindRightPosition() - 1;
        internal int TopPadding() => FindTopPosition();
        internal int BottomPadding() => Sym.Height - FindBottomPosition() - 1;

    }
}
