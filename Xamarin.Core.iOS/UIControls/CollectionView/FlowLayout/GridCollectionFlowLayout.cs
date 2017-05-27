using System;
using CoreGraphics;
using UIKit;

namespace Xamarin.Core.iOS
{
	public class GridCollectionFlowLayout : UICollectionViewFlowLayout
	{
		private int _columnCount = 1;
		private nfloat _cellHeight = 0;

		public GridCollectionFlowLayout(int column) : this(column, 0)
		{
		}

		public GridCollectionFlowLayout(int column, nfloat cellHeight)
		{
			_columnCount = column;
			_cellHeight = cellHeight;
		}

		public override CGSize ItemSize
		{
			get
			{
				if (CollectionView != null)
				{
					var totalSpacing = (MinimumInteritemSpacing * (_columnCount - 1)) + CollectionView.ContentInset.Left + CollectionView.ContentInset.Right;
					var cellWidth = (CollectionView.Frame.Width - totalSpacing) / _columnCount;
					var cellHeight = _cellHeight > 0 ? _cellHeight : cellWidth;
					var roundedCellWidth = Math.Floor(cellWidth * 100) / 100;

					return new CoreGraphics.CGSize(roundedCellWidth, cellHeight);
				}
				return base.ItemSize;
			}
		}
	}
}
