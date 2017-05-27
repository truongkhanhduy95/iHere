using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
	public abstract class BaseTableViewDataSource<TCellData> : UITableViewSource, IDataSource<TCellData>
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
		public event EventHandler<ItemClickEventArgs<TCellData>> ItemClicked;
		public event EventHandler<ItemClickEventArgs<TCellData>> ItemControlClicked;
		public event Action<IDataItemView<TCellData>> ItemLoad;

		protected BaseTableViewDataSource(NSString cellId, List<TCellData> dataSource) : base()
		{
			_cellId = cellId;
			DataSource = dataSource;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var reuseableCell = tableView.DequeueReusableCell(_cellId, indexPath);
			if (reuseableCell != null)
			{
				var cell = reuseableCell as BaseTableViewCell<TCellData>;
				if (DataSource != null && DataSource.Count > indexPath.Row)
				{
					cell.LoadData(DataSource[indexPath.Row], indexPath.Row);
					cell.ControlClick = ItemControlClicked;
					if (ItemLoad != null)
					{
						ItemLoad.Invoke(cell);
					}
				}
				return cell;
			}
			return null;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (DataSource != null)
			{
				return DataSource.Count;
			}
			return 0;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (ItemClicked != null)
			{
				var args = new ItemClickEventArgs<TCellData>();
				args.Position = indexPath.Row;
				args.Data = _dataSource[indexPath.Row];
				ItemClicked.Invoke(tableView.CellAt(indexPath), args);
			}
		}
	}
}
