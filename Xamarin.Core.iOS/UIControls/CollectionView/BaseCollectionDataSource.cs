using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
	public class BaseCollectionDataSource<TCellData> : UICollectionViewSource, IDataSource<TCellData>
	{
		private readonly NSString _cellId;

		private List<TCellData> _dataSource;

		public List<TCellData> DataSource
		{
			get
			{
				return _dataSource;
			}
			set
			{
				if (_dataSource == null)
				{
					_dataSource = new List<TCellData>();
				}
				_dataSource.Clear();
				_dataSource.AddRange(value);
				if (OnDataSourceChanged != null)
				{
					OnDataSourceChanged.Invoke();
				}
			}
		}

		public event Action OnDataSourceChanged;
		public event Action<IDataItemView<TCellData>> ItemLoad;
		public event EventHandler<ItemClickEventArgs<TCellData>> ItemClicked;
		public event EventHandler<ItemClickEventArgs<TCellData>> ItemControlClicked;

		public BaseCollectionDataSource(NSString cellId, List<TCellData> dataSource) : base()
		{
			_cellId = cellId;
			DataSource = dataSource;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var reuseableCell = collectionView.DequeueReusableCell(_cellId, indexPath) as BaseCollectionViewCell<TCellData>;
			if (DataSource != null && DataSource.Count > indexPath.Row)
			{
				reuseableCell.LoadData(DataSource[indexPath.Row], indexPath.Row);
				reuseableCell.ControlClick = ItemControlClicked;
				if (ItemLoad != null)
				{
					ItemLoad.Invoke(reuseableCell);
				}
			}
			return reuseableCell;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			if (DataSource != null)
			{
				return DataSource.Count;
			}
			return 0;
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			if (ItemClicked != null)
			{
				var args = new ItemClickEventArgs<TCellData>();
				args.Position = indexPath.Row;
				args.Data = _dataSource[indexPath.Row];
				ItemClicked.Invoke(collectionView.CellForItem(indexPath), args);
			}
		}
	}
}
