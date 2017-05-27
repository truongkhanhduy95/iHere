using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;

namespace Xamarin.Core.Droid
{
	public abstract class BaseRecyclerAdapter<TItemViewHolder, TItemData> : RecyclerView.Adapter, IDataSource<TItemData>
		where TItemViewHolder : BaseRecyclerViewHolder<TItemData>
		where TItemData : class
	{
		protected int ItemLayoutResId;

		private List<TItemData> _dataSource;

		public List<TItemData> DataSource
		{
			get { return _dataSource; }
			set
			{
				if (_dataSource == null)
				{
					_dataSource = new List<TItemData>();
				}
				_dataSource.Clear();
				_dataSource.AddRange(value);
				NotifyDataSetChanged();
			}
		}

		public override int ItemCount
		{
			get
			{
				if (DataSource != null)
				{
					return DataSource.Count;
				}
				return 0;
			}
		}

		private Action<IDataItemView<TItemData>> _itemLoad;
		private EventHandler<ItemClickEventArgs<TItemData>> _itemClicked;
		private EventHandler<ItemClickEventArgs<TItemData>> _itemControlClicked;

		public event Action<IDataItemView<TItemData>> ItemLoad
		{
			add { _itemLoad = value; }
			remove { _itemLoad = null; }
		}

		public event EventHandler<ItemClickEventArgs<TItemData>> ItemClicked
		{
			add { _itemClicked = value; }
			remove { _itemClicked = null; }
		}

		public event EventHandler<ItemClickEventArgs<TItemData>> ItemControlClicked
		{
			add { _itemControlClicked = value; }
			remove { _itemControlClicked = null; }
		}

		protected BaseRecyclerAdapter(List<TItemData> dataSource, int itemLayoutResId)
		{
			_dataSource = dataSource;
			ItemLayoutResId = itemLayoutResId;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			var itemView = LayoutInflater.From(parent.Context).Inflate(ItemLayoutResId, parent, false);
			if (itemView.Background == null)
			{
				var outValue = new TypedValue();
				parent.Context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, outValue, true);
				itemView.SetBackgroundResource(outValue.ResourceId);
				outValue.Dispose();
			}
			var viewHolder = (TItemViewHolder)Activator.CreateInstance(typeof(TItemViewHolder), itemView);
			return viewHolder;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var viewHolder = holder as TItemViewHolder;
			holder.ItemView.Tag = position;

			if (viewHolder != null && DataSource != null && DataSource.Count > position)
			{
				viewHolder.LoadData(DataSource[position], position);
				if (_itemLoad != null)
				{
					_itemLoad.Invoke(viewHolder);
				}
			}
		}

		public override void OnViewAttachedToWindow(Java.Lang.Object holder)
		{
			base.OnViewAttachedToWindow(holder);
			var viewHolder = holder as TItemViewHolder;
			if (viewHolder != null)
			{
				viewHolder.ItemView.Click += OnItemClicked;
				viewHolder.ControlClick += OnItemControlClicked;
			}
		}

		public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
		{
			base.OnViewDetachedFromWindow(holder);
			var viewHolder = holder as TItemViewHolder;
			if (viewHolder != null)
			{
				viewHolder.ItemView.Click -= OnItemClicked;
				viewHolder.ControlClick -= OnItemControlClicked;
			}
		}

		protected virtual void OnItemClicked(object sender, EventArgs e)
		{
			var itemView = sender as View;
			var position = (int)itemView.Tag;
			if (_itemClicked != null)
			{
				var args = new ItemClickEventArgs<TItemData>();
				args.Position = position;
				args.Data = _dataSource[position];
				_itemClicked.Invoke(itemView, args);
			}
		}

		protected void OnItemControlClicked(object sender, ItemClickEventArgs<TItemData> e)
		{
			var itemView = sender as View;
			if (_itemControlClicked != null)
			{
				_itemControlClicked.Invoke(itemView, e);
			}
		}
	}
}
