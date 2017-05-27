using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Java.Lang;
using Java.Lang.Reflect;

namespace Xamarin.Core.Droid
{
    /// <summary>
    /// Resource manager for Android
    /// </summary>
    public static class ResourcesUtil
    {
        public static Color GetColor(Context context, int resourceId)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var color = ContextCompat.GetColor(context, resourceId);
                return GetColor(color);
            }
            else
            {
                return context.Resources.GetColor(resourceId);
            }
        }

        public static Color GetColor(int argb)
        {
            var realColor = System.Drawing.Color.FromArgb(argb);
            return Color.Argb(realColor.A, realColor.R, realColor.G, realColor.B);
        }

        /// <summary>
        /// Gets the drawable.
        /// </summary>
        /// <param name="drawable">The drawable.</param>
        /// <returns></returns>
        public static int GetDrawable(string drawable)
        {
            return Application.Context.Resources.GetIdentifier(drawable.ToLower(), "drawable", Application.Context.PackageName);
        }

        /// <summary>
        /// Gets the layout.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <returns></returns>
        public static int GetLayout(string layout)
        {
            return Application.Context.Resources.GetIdentifier(layout, "layout", Application.Context.PackageName);
        }

        /// <summary>
        /// Gets the style.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public static int GetStyle(string style)
        {
            return Application.Context.Resources.GetIdentifier(style, "style", Application.Context.PackageName);
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static int GetString(string str)
        {
            return Application.Context.Resources.GetIdentifier(str, "string", Application.Context.PackageName);
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static int GetId(string id)
        {
            return Application.Context.Resources.GetIdentifier(id, "id", Application.Context.PackageName);
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static int GetColor(string color)
        {
            return Application.Context.Resources.GetIdentifier(color, "color", Application.Context.PackageName);
        }

        /// <summary>
        /// Gets the color of the hex.
        /// </summary>
        /// <returns>The hex color.</returns>
        /// <param name="color">Color.</param>

        public static string GetHexColor(string color)
        {
            return Application.Context.Resources.GetString(GetColor(color));
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        public static int GetIdentifier(string identifier)
        {
            return Application.Context.Resources.GetIdentifier(identifier, null, Application.Context.PackageName);
        }

        public static int[] GetStyable(string name)
        {
            Object rs = GetResourceStyle(Application.Context, name);
            return rs == null ? null : (int[])rs;
        }

        public static int GetAttributeStyable(string name)
        {
            Object rs = GetResourceStyle(Application.Context, name);
            return rs == null ? -1 : (int)rs;
        }

        private static Object GetResourceStyle(Context context, string name)
        {
            try
            {
                //use reflection to access the resource class
                Field[] fields2 = Class.ForName(context.PackageName + ".R$styleable").GetFields();

                //browse all fields
                foreach (Field f in fields2)
                {
                    //pick matching field
                    if (f.Name.Equals(name))
                    {
                        //return as int array
                        Object ret = f.Get(null);
                        return ret;
                    }
                }
            }
            catch (Throwable)
            {
            }

            return null;
        }

        public static float GetDimesion(int dimensionId)
        {
            //Warning Application.Context maybe crash. Must get dimension from currentContext.
            return Application.Context.Resources.GetDimension(dimensionId);
        }
    }
}
