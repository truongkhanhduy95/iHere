using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Uri = Android.Net.Uri;

namespace Xamarin.Core.Droid
{
	public class CircleImageView : XamarinImageView
	{
		private const int ColordrawableDimension = 2;

		private static readonly ScaleType DefaultScaleType = ScaleType.CenterCrop;
		private static readonly Bitmap.Config BitmapConfig = Bitmap.Config.Argb8888;

		private readonly Paint _fillPaint = new Paint();
		private readonly Paint _bitmapPaint = new Paint();
		private readonly Paint _borderPaint = new Paint();

		private readonly RectF _borderRect = new RectF();
		private readonly RectF _drawableRect = new RectF();

		private readonly Matrix _shaderMatrix = new Matrix();

		private Bitmap _bitmap;
		private BitmapShader _bitmapShader;
		private int _bitmapHeight;
		private int _bitmapWidth;

		private int _fillColor = Color.Transparent;
		private int _borderColor = Color.Black;
		private int _borderWidth;
		private bool _borderOverlay;
		private float _borderRadius;
		private float _drawableRadius;

		private ColorFilter _colorFilter;

		private bool _isReady;
		private bool _isSetupPending;

		public int BorderColor
		{
			get { return _borderColor; }
			set
			{
				if (value == _borderColor)
				{
					return;
				}

				_borderColor = value;
				_borderPaint.Color = new Color(_borderColor);
				Invalidate();
			}
		}

		public string BorderColorString
		{
			set { BorderColor = Color.ParseColor(value).ToArgb(); }
		}

		public int BorderWidth
		{
			get { return _borderWidth; }
			set
			{
				if (value == _borderWidth)
				{
					return;
				}

				_borderWidth = value;
				Setup();
			}
		}

		public bool BorderOverlay
		{
			get { return _borderOverlay; }
			set
			{
				if (value == _borderOverlay)
				{
					return;
				}

				_borderOverlay = value;
				Setup();
			}
		}

		public int FillColor
		{
			get { return _fillColor; }
			set
			{
				if (value == _fillColor)
				{
					return;
				}

				_fillColor = value;
				_fillPaint.Color = new Color(_fillColor);
				Invalidate();
			}
		}

		public string FillColorString
		{
			set { FillColor = Color.ParseColor(value).ToArgb(); }
		}

		protected CircleImageView(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
		}

		public CircleImageView(Context context)
			: base(context)
		{
			Intialize(context);
		}

		public CircleImageView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			Intialize(context, attrs);
		}

		public CircleImageView(Context context, IAttributeSet attrs, int defStyleAttr)
			: base(context, attrs, defStyleAttr)
		{
			Intialize(context, attrs);
		}

		private void Intialize(Context context, IAttributeSet attrs = null)
		{
			base.SetScaleType(DefaultScaleType);
			_isReady = true;

			if (!_isSetupPending)
				return;
			Setup();
			_isSetupPending = false;
		}

		public override ScaleType GetScaleType()
		{
			base.GetScaleType();
			return DefaultScaleType;
		}

		public override void SetScaleType(ScaleType scaleType)
		{
			if (scaleType != DefaultScaleType)
			{
				throw new ArgumentException(string.Format("ScaleType {0} not supported.", scaleType));
			}
		}

		public override void SetAdjustViewBounds(bool adjustViewBounds)
		{
			if (adjustViewBounds)
			{
				throw new ArgumentException("adjustViewBounds not supported.");
			}
		}

		protected override void OnDraw(Canvas canvas)
		{
			if (_bitmap == null)
			{
				return;
			}

			if (_fillColor != Color.Transparent)
			{
				canvas.DrawCircle(Width / 2.0f, Height / 2.0f, _drawableRadius, _fillPaint);
			}
			canvas.DrawCircle(Width / 2.0f, Height / 2.0f, _drawableRadius, _bitmapPaint);
			if (_borderWidth != 0)
			{
				canvas.DrawCircle(Width / 2.0f, Height / 2.0f, _borderRadius, _borderPaint);
			}
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);
			Setup();
		}

		public override void SetImageBitmap(Bitmap bm)
		{
			base.SetImageBitmap(bm);
			_bitmap = bm;
			Setup();
		}

		public override void SetImageDrawable(Drawable drawable)
		{
			base.SetImageDrawable(drawable);
			_bitmap = GetBitmapFromDrawable(drawable);
			Setup();
		}

		public override void SetImageResource(int resId)
		{
			base.SetImageResource(resId);
			_bitmap = GetBitmapFromDrawable(Resources.GetDrawable(resId, Context.Theme));
			Setup();
		}

		public override void SetImageURI(Uri uri)
		{
			base.SetImageURI(uri);
			var stream = Application.Context.ContentResolver.OpenInputStream(uri);
			var drawable = Drawable.CreateFromStream(stream, uri.ToString());
			_bitmap = GetBitmapFromDrawable(drawable);
			Setup();
		}

		public override void SetColorFilter(ColorFilter cf)
		{
			base.SetColorFilter(cf);

			if (cf == _colorFilter)
			{
				return;
			}

			_colorFilter = cf;
			_bitmapPaint.SetColorFilter(_colorFilter);
			Invalidate();
		}

		private Bitmap GetBitmapFromDrawable(Drawable drawable)
		{
			if (drawable == null)
			{
				return null;
			}

			var bitmapDrawable = drawable as BitmapDrawable;
			if (bitmapDrawable != null)
			{
				return bitmapDrawable.Bitmap;
			}

			try
			{
				Bitmap bitmap;

				if (drawable is ColorDrawable)
				{
					bitmap = Bitmap.CreateBitmap(ColordrawableDimension, ColordrawableDimension, BitmapConfig);
				}
				else
				{
					bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, BitmapConfig);
				}

				var canvas = new Canvas(bitmap);
				drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
				drawable.Draw(canvas);
				return bitmap;
			}
			catch (OutOfMemoryException)
			{
				return null;
			}
		}

		private void Setup()
		{
			if (!_isReady)
			{
				_isSetupPending = true;
				return;
			}

			if (Width == 0 && Height == 0)
			{
				return;
			}

			if (_bitmap == null)
			{
				Invalidate();
				return;
			}

			_bitmapShader = new BitmapShader(_bitmap, Shader.TileMode.Clamp, Shader.TileMode.Clamp);

			_bitmapPaint.AntiAlias = true;
			_bitmapPaint.SetShader(_bitmapShader);

			_borderPaint.SetStyle(Paint.Style.Stroke);
			_borderPaint.AntiAlias = true;
			_borderPaint.Color = new Color(_borderColor);
			_borderPaint.StrokeWidth = _borderWidth;

			_fillPaint.SetStyle(Paint.Style.Fill);
			_fillPaint.AntiAlias = true;
			_fillPaint.Color = new Color(_fillColor);

			_bitmapHeight = _bitmap.Height;
			_bitmapWidth = _bitmap.Width;

			_borderRect.Set(0, 0, Width, Height);
			_borderRadius = Math.Min((_borderRect.Height() - _borderWidth) / 2.0f,
				(_borderRect.Width() - _borderWidth) / 2.0f);

			_drawableRect.Set(_borderRect);
			if (!_borderOverlay)
			{
				_drawableRect.Inset(_borderWidth, _borderWidth);
			}
			_drawableRadius = Math.Min(_drawableRect.Height() / 2.0f, _drawableRect.Width() / 2.0f);

			UpdateShaderMatrix();
			Invalidate();
		}

		private void UpdateShaderMatrix()
		{
			float scale;
			float dx = 0;
			float dy = 0;

			_shaderMatrix.Set(null);

			if (_bitmapWidth * _drawableRect.Height() > _drawableRect.Width() * _bitmapHeight)
			{
				scale = _drawableRect.Height() / _bitmapHeight;
				dx = (_drawableRect.Width() - _bitmapWidth * scale) * 0.5f;
			}
			else
			{
				scale = _drawableRect.Width() / _bitmapWidth;
				dy = (_drawableRect.Height() - _bitmapHeight * scale) * 0.5f;
			}

			_shaderMatrix.SetScale(scale, scale);
			_shaderMatrix.PostTranslate((int)(dx + 0.5f) + _drawableRect.Left, (int)(dy + 0.5f) + _drawableRect.Top);

			_bitmapShader.SetLocalMatrix(_shaderMatrix);
		}
	}
}
