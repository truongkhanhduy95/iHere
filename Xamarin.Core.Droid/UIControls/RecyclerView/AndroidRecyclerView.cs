using Android.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;

namespace Xamarin.Core.Droid
{
	public class AndroidRecyclerView : RecyclerView, IControl
	{
		private ItemDecoration _decoration;

		public int SpanCount { get; set; }

		public bool Scrollable { get; set; }

		public RecyclerLayoutType LayoutType
		{
			set
			{
				switch (value)
				{
					case RecyclerLayoutType.ListVertical:
						var layoutManagerVertical = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);
						SetLayoutManager(layoutManagerVertical);
						break;
					case RecyclerLayoutType.ListHorizonal:
						var layoutManagerHorizontal = new LinearLayoutManager(Context, LinearLayoutManager.Horizontal, false);
						SetLayoutManager(layoutManagerHorizontal);
						break;
					case RecyclerLayoutType.Grid:
						var layoutManagerGrid = new GridLayoutManager(Context, SpanCount);
						SetLayoutManager(layoutManagerGrid);
						break;
				}
			}
		}

		public bool IsVisible
		{
			get { return (Visibility == ViewStates.Visible); }
			set { Visibility = (value ? ViewStates.Visible : ViewStates.Gone); }
		}

		public AndroidRecyclerView(Context context, IAttributeSet attrs, int defStyle)
			: base(context, attrs, defStyle)
		{
			SpanCount = 2;
			Scrollable = true;
		}

		public AndroidRecyclerView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			SpanCount = 2;
			Scrollable = true;
		}

		public AndroidRecyclerView(Context context)
			: base(context)
		{
			SpanCount = 2;
			Scrollable = true;
		}

		public void SetItemSpacing(int innerSpacing, int outerSpacing)
		{
			if (_decoration != null)
			{
				RemoveItemDecoration(_decoration);
			}

			_decoration = new ItemSpacingDecoration(innerSpacing, outerSpacing);
			AddItemDecoration(_decoration);
		}

		public void SetItemSpacing(int innerSpacing, int leftOuterSpacing, int rightOuterSpacing, int topOuterSpacing, int bottomOuterSpacing)
		{
			if (_decoration != null)
			{
				RemoveItemDecoration(_decoration);
			}

			_decoration = new ItemSpacingDecoration(innerSpacing, leftOuterSpacing, rightOuterSpacing, topOuterSpacing, bottomOuterSpacing);
			AddItemDecoration(_decoration);
		}

		public override bool OnInterceptTouchEvent(MotionEvent ev)
		{
			if (!Scrollable)
			{
				return false;
			}
			return base.OnInterceptTouchEvent(ev);
		}
	}
}
