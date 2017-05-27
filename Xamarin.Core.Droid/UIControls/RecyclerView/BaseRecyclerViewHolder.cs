using System;
using Android.Support.V7.Widget;
using Android.Views;

namespace Xamarin.Core.Droid
{
	public abstract class BaseRecyclerViewHolder<TItemData> : RecyclerView.ViewHolder, IDataItemView<TItemData>
	{
		public TItemData ItemData { get; private set; }

		public int ItemPosition { get; private set; }
		public EventHandler<ItemClickEventArgs<TItemData>> ControlClick;

		protected BaseRecyclerViewHolder(View itemView)
			: base(itemView)
		{
		}

		public virtual void LoadData(TItemData data, int position)
		{
			ItemData = data;
			ItemPosition = position;
		}

		protected virtual void ControlTouchUpInside(object sender, EventArgs e)
		{
			if (ControlClick != null)
			{
				var args = new ItemClickEventArgs<TItemData>();
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
