using System;
using UIKit;

namespace Xamarin.Core.iOS
{
	public abstract class BaseTableViewCell<TCellData> : UITableViewCell, IDataItemView<TCellData>
	{
		public TCellData ItemData { get; private set; }

		public int ItemPosition { get; private set; }

		public EventHandler<ItemClickEventArgs<TCellData>> ControlClick;

		protected BaseTableViewCell(IntPtr handle) : base(handle)
		{
		}

		public virtual void LoadData(TCellData data, int position)
		{
			ItemData = data;
			ItemPosition = position;
		}

		protected virtual void ControlTouchUpInside(object sender, EventArgs e)
		{
			if (ControlClick != null)
			{
				var args = new ItemClickEventArgs<TCellData>();
				args.Position = ItemPosition;
				args.Data = ItemData;
				ControlClick.Invoke(sender, args);
			}
		}

		public virtual IControl GetSharedControlInterface(string tag)
		{
			return null;
		}
	}
}
