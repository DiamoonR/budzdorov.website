using System;
using System.ComponentModel;

namespace health.Util
{
    public static class Constants
    {
        public const int MAX_IMAGE_WIDTH = 1000;
        public const int MIN_IMAGE_WIDTH = 400;
        public const int ARTICLE_PAGE_NUMBER = 20;
        public const int PAGER_NUMBER_OF_VISIBLE_LINKS = 3;

        public const string URL_ARTICLE = "/articles";
        public const string URL_HERB = "/medicinal-herbs";
        public const string URL_DISEASE = "/diseases";

        public const string PATH_TMP = "images/tmp";
        public const string PATH_UPLOAD_IMAGE = "images/uploads";
        public const string PATH_UPLOAD_BIG_IMAGE = "images/uploads/big";
    }
}

public static class EnumExtensions
{

    // This extension method is broken out so you can use a similar pattern with 
    // other MetaData elements in the future. This is your base method for each.
    public static T GetAttribute<T>(this Enum value) where T : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

        // check if no attributes have been specified.
        if (((Array)attributes).Length > 0)
        {
            return (T)attributes[0];
        }
        else
        {
            return null;
        }
    }

    // This method creates a specific call to the above method, requesting the
    // Description MetaData attribute.
    public static string ToName(this Enum value)
    {
        var attribute = value.GetAttribute<DescriptionAttribute>();
        return attribute == null ? value.ToString() : attribute.Description;
    }

    /// <summary>
    /// Find the enum from the description attribute.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="desc"></param>
    /// <returns></returns>
    public static T FromName<T>(this string desc) where T : struct
    {
        string attr;
        Boolean found = false;
        T result = (T)Enum.GetValues(typeof(T)).GetValue(0);

        foreach (object enumVal in Enum.GetValues(typeof(T)))
        {
            attr = ((Enum)enumVal).ToName();

            if (attr == desc)
            {
                result = (T)enumVal;
                found = true;
                break;
            }
        }

        if (!found)
        {
            throw new Exception();
        }

        return result;
    }
}