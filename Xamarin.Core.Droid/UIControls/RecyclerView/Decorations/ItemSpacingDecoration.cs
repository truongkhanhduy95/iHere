using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;

namespace Xamarin.Core.Droid
{
	public class ItemSpacingDecoration : RecyclerView.ItemDecoration
	{
		private readonly int _innerSpacing;
		private readonly int _outerSpacing;

		private readonly bool _isDynamicOuterSpacing;
		private readonly int _outerSpacingLeft;
		private readonly int _outerSpacingRight;
		private readonly int _outerSpacingTop;
		private readonly int _outerSpacingBottom;


		public ItemSpacingDecoration(int innerSpacing, int outerSpacing)
		{
			_innerSpacing = innerSpacing;
			_outerSpacing = outerSpacing;

			_isDynamicOuterSpacing = false;
			_outerSpacingLeft = 0;
			_outerSpacingRight = 0;
			_outerSpacingTop = 0;
			_outerSpacingBottom = 0;
		}

		public ItemSpacingDecoration(int innerSpacing, int leftOuterSpacing, int rightOuterSpacing, int topOuterSpacing, int bottomOuterSpacing)
		{
			_innerSpacing = innerSpacing;
			_outerSpacing = 0;

			_isDynamicOuterSpacing = true;
			_outerSpacingLeft = leftOuterSpacing;
			_outerSpacingRight = rightOuterSpacing;
			_outerSpacingTop = topOuterSpacing;
			_outerSpacingBottom = bottomOuterSpacing;
		}

		public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
		{
			base.GetItemOffsets(outRect, view, parent, state);
			int itemCount = parent.GetAdapter().ItemCount;
			int viewPosition = parent.GetChildLayoutPosition(view);


			outRect.Top = _innerSpacing / 2;
			outRect.Bottom = _innerSpacing / 2;
			outRect.Left = _innerSpacing / 2;
			outRect.Right = _innerSpacing / 2;

			if (parent.GetLayoutManager() is GridLayoutManager)
			{
				var layoutManager = parent.GetLayoutManager() as GridLayoutManager;
				int spanCount = layoutManager.SpanCount;

				var layoutParam = view.LayoutParameters;
				layoutParam.Width = (parent.Width - (_outerSpacing * 2)) / spanCount;
				view.LayoutParameters = layoutParam;

				bool isTopView = viewPosition < spanCount;
				if (isTopView)
				{
					if (_isDynamicOuterSpacing)
					{
						outRect.Top = _outerSpacingTop;
					}
					else
					{
						outRect.Top = _outerSpacing;
					}
				}

				bool isLeftView = (viewPosition % spanCount) == 0;
				if (isLeftView)
				{
					if (_isDynamicOuterSpacing)
					{
						outRect.Left = _outerSpacingLeft;
					}
					else
					{
						outRect.Left = _outerSpacing;
					}
				}

				bool isRightView = ((viewPosition + 1) % spanCount) == 0;
				if (isRightView)
				{
					if (_isDynamicOuterSpacing)
					{
						outRect.Right = _outerSpacingRight;
					}
					else
					{
						outRect.Right = _outerSpacing;
					}

				}

				bool isBottomView = (viewPosition + spanCount) > itemCount;
				if (isBottomView)
				{
					if (_isDynamicOuterSpacing)
					{
						outRect.Bottom = _outerSpacingBottom;
					}
					else
					{
						outRect.Bottom = _outerSpacing;
					}
				}
			}
			else if (parent.GetLayoutManager() is LinearLayoutManager)
			{
				var layoutManager = (LinearLayoutManager)parent.GetLayoutManager();

				if (layoutManager.Orientation == LinearLayoutManager.Vertical)
				{
					if (viewPosition == 0)
					{
						if (_isDynamicOuterSpacing)
						{
							outRect.Top = _outerSpacingTop;
						}
						else
						{
							outRect.Top = _outerSpacing;
						}
					}
					if (viewPosition == itemCount - 1)
					{
						if (_isDynamicOuterSpacing)
						{
							outRect.Bottom = _outerSpacingBottom;
						}
						else
						{
							outRect.Bottom = _outerSpacing;
						}
					}

					outRect.Left = _outerSpacing;
					outRect.Right = _outerSpacing;
				}
				else if (layoutManager.Orientation == LinearLayoutManager.Horizontal)
				{
					if (viewPosition == 0)
					{
						if (_isDynamicOuterSpacing)
						{
							outRect.Left = _outerSpacingLeft;
						}
						else
						{
							outRect.Left = _outerSpacing;
						}
					}
					if (viewPosition == itemCount - 1)
					{
						if (_isDynamicOuterSpacing)
						{
							outRect.Right = _outerSpacingRight;
						}
						else
						{
							outRect.Right = _outerSpacing;
						}
					}

					outRect.Top = _outerSpacing;
					outRect.Bottom = _outerSpacing;
				}
			}
		}
	}
}
